(function () {
    "use strict";

    var id;
    var comments = [];
    var cookiename = "comments_" + id;
    var applicationData = Windows.Storage.ApplicationData.current;

    function loadComments() {
        var comments = getComments();
        comments.forEach(function (e) {
            renderComment(e)
        });
    }

    function newComment(e) {
        var now = new Date();
        var userValue = document.getElementById("full-name");
        var commentValue = document.getElementById("comment");

        var comment = {
            user: userValue.innerHTML,
            content: commentValue.innerHTML,
            date: (now.getMonth() + 1) + "/" + now.getDate() + "/" + now.getFullYear()
        }

        comments.push(comment);

        renderComment(comment);
        
        applicationData.localSettings.values[cookiename] = JSON.stringify(comments.slice(comments.length - 4, comments.length));
        document.getElementById("comment").innerHTML = "";
    }

    function renderComment(comment) {
        var templateElement = document.getElementById("commentTemplate");
        var renderElement = document.getElementById("inner-comments-container");
        templateElement.winControl.render(comment, renderElement);
        renderElement.scrollTop = comments.length * 150;
    }

    function getComments() {
        return [];
    }

    function errorHandling() {
        var errMsg = document.getElementById("videoFailedToLoadMsg");
        if (errMsg) errMsg.className = "error";
    }

    WinJS.UI.Pages.define("/pages/videoDetails/videoDetails.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            var errMsg = document.getElementById("videoFailedToLoadMsg");
            if (errMsg) errMsg.className = "";

            var commentBox = document.getElementById("comment")
            if (commentBox) commentBox.value = "";

            Data.setUserBox();
            Data.resolveItemReference(options.item, errorHandling).done(function (video) {
                var mediaPlayer = document.getElementById("mediaPlayer");
                if (!mediaPlayer) return;

                // initialize playlist
                mediaPlayer.winControl.playlistPlugin.playlist = [{ data: video, src: video.EncodedVideoUrl || video.SourceVideoUrl }];
                mediaPlayer.winControl.playlistPlugin.currentPlaylistItemIndex = 0;

                id = video.id;

                var getValueOrDefault = function (currentValue, defaultValue) {
                    if (currentValue === undefined || currentValue == null) {
                        return defaultValue;
                    }

                    return currentValue;
                };

                video.Title = getValueOrDefault(video.Title, 'no title');
                video.Description = getValueOrDefault(video.Description, 'no description');

                loadComments();
                
                // process bindings
                WinJS.Binding.processAll(element, video);                
            }.bind(this));

            this._checkPing();

            var doComment = document.getElementById("doComment");
            doComment.addEventListener('click', function () {
                newComment();
            }.bind(this));
        },
        _checkPing: function () {
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
        // This function is called whenever a user navigates away from this page.
        // It resets the page and disposes of the media player control.
        unload: function () {
            if (mediaPlayer) {
                mediaPlayer.winControl.dispose();
                mediaPlayer.winControl = null;
            }
        }
    });
})();
