(function () {
    "use strict";

    var pingPath = "ping";

    var list = new WinJS.Binding.List();
    var Storage = Windows.Storage;
        
    var messagesElement,
        messagesHistoryElement;

    var profilingInformationHistory = [];

    var tracingInformationHistory = [];

    var profilingTemplateElement,
        tracingTemplateElement;
    
    WinJS.Namespace.define("Data", {
        itemsRetrieved: false,
        networkConnectivity: true,
        items: list,
        getItemReference: getItemReference,
        resolveItemReference: resolveItemReference,
        getAllItems: getAllItems,
        pingUrl: Configuration.ApiBaseUrl + pingPath,
        messagesElement: messagesElement,
        messagesHistoryElement: messagesHistoryElement,
        profilingEnabled: false,
        enableProfiling: enableProfiling,
        disableProfiling: disableProfiling,
        profilingTemplateElement: profilingTemplateElement,
        displayTracingMessage: displayTracingMessage,
        updateVideoItem: updateVideoItem,
        tracingTemplateElement: tracingTemplateElement,
        ping: ping,
        setUserBox: setUserBox,
        clearCache: {
            get: function () {
                var clearCache = this._clearCache || false;
                this._clearCache = false;
                return clearCache;
            },
            set: function () {
                this._clearCache = true;
            }
        }
    });

    // Retrieve the list of items from the Web API service
    function getAllItems() {
        WebApi.getVideos(GetVideosOnSuccess, GetVideosOnError);
    }

    function GetVideosOnSuccess(result) {
        setProfilingTime('GetAll', getExecutionTime(result));

        var videos = JSON.parse(result.responseText);

        list.length = 0;

        videos.forEach(function (video, i) {
            list.push(getListItem(video));
        });

        Data.itemsRetrieved = true;
    }

    function GetVideosOnError(result, xx) {
        setProfilingTime('GetAll(Failed)', 0);
        WinJS.log && WinJS.log("The service request to retrieve the list of videos has failed. Error: " + result.statusText, "BuildClips", "error");
    }

    // Get a reference for an item, using the group key and item title as a
    // unique reference to the item that can be easily serialized.
    function getItemReference(item) {
        return [item.id, item.title];
    }

    function enableProfiling() {
        tracingInformationHistory = [];

        Data.messagesElement.textContent = 'Profiling Enabled';
        Data.messagesHistoryElement.textContent = 'No profiling history information available';
        
        Data.profilingEnabled = true;
    }

    function disableProfiling() {
        profilingInformationHistory = [];
        Data.messagesElement.style.display = 'none';

        Data.profilingEnabled = false;
    }

    function setProfilingTime(operation, timeElapsed) {
        if (!Data.profilingEnabled) return;

        Data.messagesElement.textContent = '';

        var profilingInformation = { time: new Date(), operation: operation, timeElapsed: timeElapsed };

        Data.profilingTemplateElement.winControl.render(
            profilingInformation,
            Data.messagesElement);

        profilingInformationHistory.push(profilingInformation);

        if (profilingInformationHistory.length > 1) {
            Data.messagesHistoryElement.textContent = '';
        }

        for (var profilingHistoryIndex = profilingInformationHistory.length - 1; profilingHistoryIndex > 0; profilingHistoryIndex--) {
            Data.profilingTemplateElement.winControl.render(
                profilingInformationHistory[profilingHistoryIndex - 1],
                Data.messagesHistoryElement);
        }

        if (profilingInformationHistory.length == 4) {
            profilingInformationHistory.shift();
        }
    }

    // Get a unique item id from the provided string array and
    // retrieve the video details from the Web API service.
    function resolveItemReference(reference, errorHandling) {
        var itemId = reference ? reference[0] : Data.items.getAt(0).id;

        return new WinJS.Promise(function (comp, err, prog) {
            WebApi.getVideo(itemId, errorHandling, comp, resolveItemReferenceOnSuccess, resolveItemReferenceOnError);
        });
    }

    function resolveItemReferenceOnSuccess(result, comp) {
        setProfilingTime('Get', getExecutionTime(result));
        var video = JSON.parse(result.responseText);
        comp(video);
    }

    function resolveItemReferenceOnError(errorHandling) {
        setProfilingTime('Get(Failed)', 0);
        errorHandling();
    }

    function getListItem(video) {
        var item = {
            id: video.Id,
            status: (video.JobId === undefined || video.JobId === null) ? 1 : 0,
            statusTimespan: new Date(),
            title: getValueOrDefault(video.Title, 'no title'),
            description: getValueOrDefault(video.Description, 'no description'),
            backgroundImage: getValueOrDefault(video.ThumbnailUrl, '/images/nothumbnail.png'),
            heart: getFromCache(video).heart,
            eye: getFromCache(video).eye,
            comment: getFromCache(video).comment,
            videoTime: getFromCache(video).videoTime,
            videoDate: getFromCache(video).videoDate
        };

        return item;
    }

    function getValueOrDefault (currentValue, defaultValue) {
        if (currentValue === undefined || currentValue === null) {
            return defaultValue;
        }

        return currentValue;
    };

    function getFromCache(video) {

        if (cacheDictionary[video.Id] === undefined) {
            
            if (video.ThumbnailUrl === undefined || video.ThumbnailUrl === null) {
                cacheDictionary[video.Id] = {
                    heart: 0,
                    eye: 0,
                    comment: 0,
                    videoTime: Math.floor((Math.random() * 5) + 1) + ":" + ("0" + Math.floor((Math.random() * 59) + 1)).slice(-2),
                    videoDate: "OCT 31 2012"
                }
            }
            else {
                cacheDictionary[video.Id] = {
                    heart: Math.floor((Math.random() * 100) + 1),
                    eye: Math.floor((Math.random() * 1000) + 101),
                    comment: Math.floor((Math.random() * 10) + 1),
                    videoTime: Math.floor((Math.random() * 5) + 1) + ":" + ("0" + Math.floor((Math.random() * 59) + 1)).slice(-2),
                    videoDate: "OCT " + ("0" + Math.floor((Math.random() * 31) + 1)).slice(-2) + " 2012"
                }
            }
        }

        return cacheDictionary[video.Id];
    }

    function updateVideoItem(updatedVideo) {
        var videoToUpdateIndex = -1;

        Data.items.forEach(function (video, index) {
            if (video.id == updatedVideo.Id) {
                videoToUpdateIndex = index;
            }
        });

        if (videoToUpdateIndex != -1) {
            var videoToUpdate = getListItem(updatedVideo);
            Data.items.setAt(videoToUpdateIndex, videoToUpdate);
        }
        else {
            Data.getAllItems();
        }
    }

    function displayTracingMessage(message) {
        if (Data.profilingEnabled) return;

        Data.messagesElement.textContent = '';

        var tracingMessage = { message: message };

        Data.tracingTemplateElement.winControl.render(
            tracingMessage,
            Data.messagesElement);

        tracingInformationHistory.push(tracingMessage);

        if (tracingInformationHistory.length > 1) {
            Data.messagesHistoryElement.textContent = '';
        }
        else {
            Data.messagesHistoryElement.textContent = 'No tracing history information available';
        }

        for (var tracingHistoryIndex = tracingInformationHistory.length - 1; tracingHistoryIndex > 0; tracingHistoryIndex--) {
            Data.tracingTemplateElement.winControl.render(
                tracingInformationHistory[tracingHistoryIndex - 1],
                Data.messagesHistoryElement);
        }

        if (tracingInformationHistory.length == 4) {
            tracingInformationHistory.shift();
        }

        Data.messagesElement.style.display = 'block';
    }

    function setUserBox() {
        var userImage = document.getElementById('user-image');
        var firstName = document.getElementById('first-name');
        var lastName = document.getElementById('last-name');
        var fullName = document.getElementById('full-name');

        var userFirstName = Windows.Storage.ApplicationData.current.localSettings.values["UserFirstName"];
        var userLastName = Windows.Storage.ApplicationData.current.localSettings.values["UserLastName"];

        if (userImage) userImage.style.backgroundImage = "url(" + Windows.Storage.ApplicationData.current.localSettings.values["UserImage"] + ")";
        if (firstName) firstName.innerHTML = userFirstName;
        if (lastName) lastName.innerHTML = userLastName;
        if (fullName) fullName.innerHTML = userFirstName + " " + userLastName;
    }

    function getExecutionTime(result) {
        return result.getResponseHeader("Server-Execution-Time");
    }

    function ping(complete, error) {
        var url = Data.pingUrl;
        WinJS.xhr({ url: url, responseType: "json", headers: { "Cache-Control": "no-cache", "If-Modified-Since": "Mon, 27 Mar 1972 00:00:00 GMT" } })
             .done(
                function (result) {
                    Data.networkConnectivity = true;
                    complete(result)
                },
                function (result, xx) {
                    Data.networkConnectivity = false;
                    error(result, xx)
                });
    }

    var cacheDictionary = {};

})();
