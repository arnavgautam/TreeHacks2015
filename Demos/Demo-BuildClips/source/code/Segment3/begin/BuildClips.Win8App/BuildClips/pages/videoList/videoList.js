(function () {
    "use strict";

    var appView = Windows.UI.ViewManagement.ApplicationView;
    var appViewState = Windows.UI.ViewManagement.ApplicationViewState;
    var nav = WinJS.Navigation;
    var ui = WinJS.UI;

    WinJS.Namespace.define("VideoItemConverters", {
        statusInProgressConverter: WinJS.Binding.converter(function (status) {
            return (status == 0) ? "block" : "none";
        }),

        statusCompletedConverter: WinJS.Binding.converter(function (status) {
            return (status == 1) ? "block" : "none";
        }),

        statusTimespanConverter: WinJS.Binding.converter(function (statusTimespan) {
            function getMonthName(month) {
                var monthNames = ["JAN", "FEB", "MARC", "APR", "MAY", "JUN",
                "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

                return monthNames[month];
            }

            function getHourAMPM(date) {
                var hours = date.getHours();
                var minutes = date.getMinutes();
                var seconds = date.getSeconds();
                var ampm = hours >= 12 ? 'pm' : 'am';
                var hours = hours % 12;
                hours = hours ? hours : 12; // the hour '0' should be '12'
                minutes = minutes < 10 ? '0' + minutes : minutes;
                return hours + ':' + minutes + ':' + (seconds > 9 ? seconds : "0" + seconds) + ' ' + ampm;
            }

            return statusTimespan.getDate() + " "
                 + getMonthName(statusTimespan.getMonth()) + " "
                 + statusTimespan.getFullYear() + " - "
                 + getHourAMPM(statusTimespan);
        })
    })

    ui.Pages.define("/pages/videoList/videoList.html", {
        // Navigates to the groupHeaderPage. Called from the groupHeaders,
        // keyboard shortcut and iteminvoked.
        navigateToItem: function (key) {
            nav.navigate("/pages/videoDetails/videoDetails.html", { item: key });
        },

        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {

            // Cleaning list of videos and previous page state (ADDED LAST TIME) 
            document.getElementById("videoCounters").style.display = "none";
            Data.itemsRetrieved = false;
            Data.items.length = 0;

            // Get list of videos
            Data.getAllItems();

            var listView = element.querySelector(".groupeditemslist").winControl;
            listView.itemTemplate = this._itemTemplate;
            listView.oniteminvoked = this._itemInvoked.bind(this);

            // Set up a keyboard shortcut (ctrl + alt + m) to navigate to the
            // current group when not in snapped mode.
            listView.addEventListener("keydown", function (e) {
                if (appView.value !== appViewState.snapped && e.ctrlKey && e.keyCode === WinJS.Utilities.Key.m && e.altKey) {
                    var data = listView.itemDataSource.list.getAt(listView.currentItem.index);
                    this.navigateToItem(data.group.key);
                    e.preventDefault();
                    e.stopImmediatePropagation();
                }
            }.bind(this), true);

            var filtersFlyout = document.getElementById("filtersFlyout");
            var filters = document.getElementById("filters");

            filtersFlyout.winControl.anchor = filters;
            filtersFlyout.winControl.placement = 'top';
            filtersFlyout.winControl.alignment = 'right';

            document.getElementById("upload").addEventListener("click", function (e) {
                nav.navigate("/pages/upload/upload.html");
            });
            document.getElementById("capture").addEventListener("click", function (e) {
                nav.navigate("/pages/record/record.html");
            });
            document.getElementById("refresh").addEventListener("click", function (e) {
                Data.getAllItems();
            });

            filters.addEventListener("click", function (e) {
                filtersFlyout.winControl.show();
            });

            $("#filterSlider").slider({ min: 0, max: 5 });

            var tags = document.getElementById("tagsFilter").children;

            for (var tagIndex = 0; tagIndex < tags.length; tagIndex++) {
                var tag = tags[tagIndex];

                tag.addEventListener('click', function (e) {
                    if (WinJS.Utilities.hasClass(e.currentTarget, 'active'))
                        WinJS.Utilities.removeClass(e.currentTarget, 'active');
                    else
                        WinJS.Utilities.addClass(e.currentTarget, 'active');
                });
            }

            this._initializeLayout(listView, appView.value);
            listView.element.focus();

            Data.setUserBox();
            
            this._checkPing();
        },

        // This function updates the page layout in response to viewState changes.
        updateLayout: function (element, viewState, lastViewState) {
            /// <param name="element" domElement="true" />

            var listView = element.querySelector(".groupeditemslist").winControl;
            if (lastViewState !== viewState) {
                if (lastViewState === appViewState.snapped || viewState === appViewState.snapped) {
                    var handler = function (e) {
                        listView.removeEventListener("contentanimating", handler, false);
                        e.preventDefault();
                    }
                    listView.addEventListener("contentanimating", handler, false);
                    this._initializeLayout(listView, viewState);
                }
            }
        },

        // This function updates the ListView with new layouts
        _initializeLayout: function (listView, viewState) {
            /// <param name="listView" value="WinJS.UI.ListView.prototype" />

            if (viewState === appViewState.snapped) {
                listView.itemDataSource = Data.items.dataSource;
                listView.layout = new ui.ListLayout();
            } else {
                listView.itemDataSource = Data.items.dataSource;
                listView.layout = new ui.GridLayout({ groupHeaderPosition: "top" });
            }

            listView.addEventListener("loadingstatechanged", this._updateCounters);
        },

        _itemTemplate: function (itemPromise) {
            var itemTemplate = document.querySelector(".itemtemplate").winControl;

            return itemPromise.then(function (item) {
                var itemTemplatePromise = itemTemplate.render(item.data);

                if (item.data.status == 0) {
                    // Encoding progress bar...
                    itemTemplatePromise.done(function (div) {
                        setTimeout(function () {
                            var progress = div.querySelectorAll("progress");

                            for (var i in progress) {
                                if (progress[i].style) {
                                    progress[i].style.visibility = "hidden";
                                }
                            }
                        }, 5000);
                    });
                }

                return itemTemplatePromise;
            })
        },

        _itemInvoked: function (args) {
            var item = Data.items.getAt(args.detail.itemIndex);
            nav.navigate("/pages/videoDetails/videoDetails.html", { item: Data.getItemReference(item) });
        },

        _checkPing: function() {
            var successMethod = this._success;
            var errorMethod = this._error;

            window.setInterval(function () {
                Data.ping(successMethod, errorMethod)
            },
                Configuration.CheckPingInterval);
        },

        _success: function (result) {
            var userBox = document.getElementById('user-image');
            if (userBox) userBox.style.border = '1px solid #0e850d';
        },

        _error: function (result, xx) {
            var userBox = document.getElementById('user-image');
            if (userBox) userBox.style.border = '1px solid red';
        },

        _updateCounters: function () {
            var listView = document.querySelector(".groupeditemslist").winControl;
            var videoCounters = document.getElementById("videoCounters");

            if (Data.itemsRetrieved && listView.loadingState === "complete") {
                var myVideosCount = document.getElementById("myVideosCount");
                var allVideosCount = document.getElementById("allVideosCount");

                var myVideosCountInSelect = document.getElementById("myVideosCountInSelect");
                var allVideosCountInSelect = document.getElementById("allVideosCountInSelect");

                listView.itemDataSource.getCount().done(function (count) {
                    myVideosCount.textContent = count;
                    allVideosCount.textContent = (count + 8);

                    myVideosCountInSelect.textContent = myVideosCountInSelect.attributes.getNamedItem("data-caption").value;
                    myVideosCountInSelect.textContent += " (" + count + ")";

                    allVideosCountInSelect.textContent = allVideosCountInSelect.attributes.getNamedItem("data-caption").value;
                    allVideosCountInSelect.textContent += " (" + (count + 8) + ")";

                    videoCounters.style.display = "block";
                });
            }
        }
    });
})();
