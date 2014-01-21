(function () {
    "use strict";

    var url = Configuration.ApiBaseUrl + "api/videos";

    WinJS.Namespace.define("WebApi", {
        getVideos: function getVideos(onSuccess, onError) {
            WinJS.xhr({ url: url, responseType: "json", headers: getProfilingOptions() })
                .done(onSuccess, onError);
        },

        getVideo: function getVideo(id, errorHandling, comp, onSuccess, onError) {
            WinJS.xhr({ url: url + '/' + id, responseType: "json", headers: getProfilingOptions() })
                .done(
                        function (result) { onSuccess(result, comp) },
                        function () { onError(errorHandling) });
        },

        deleteVideo: function deleteVideo(id, onSuccess, onError) {
            WinJS.xhr({ url: url + '/' + id, responseType: "json", type: 'DELETE' })
                .done(onSuccess, onError);
        },

        uploadVideo: function uploadVideo(uploadStatusIndicator, uploader, contentParts, onSuccess, onError, onProgressUpdate) {
            var uri = new Windows.Foundation.Uri(url);
            
            uploader.createUploadAsync(uri, contentParts).then(function (uploadOperation) {
                
                uploadOperation.startAsync()
                    .then(
                            function () { onSuccess(uploadOperation, uploadStatusIndicator) },
                            function (err) { onError(err, uploadOperation, uploadStatusIndicator) },
                            function () { onProgressUpdate(uploadOperation) });
                uploadStatusIndicator.showUploading();
            });
        }
    });

    function getProfilingOptions() {
        var profilingOptions = "";

        if (Data.profilingEnabled) {
            profilingOptions += (profilingOptions.length > 0 ? "," : "") + "enable";
        }

        if (Data.clearCache) {
            profilingOptions += (profilingOptions.length > 0 ? "," : "") + "clear-cache";
        }

        return { "Set-Profiling-Options": profilingOptions };
    }
})();