(function () {
    "use strict";

    // Using
    var Capture = Windows.Media.Capture;
    var Storage = Windows.Storage;
    var activation = Windows.ApplicationModel.Activation;

    // Globals
    var mediaCapture;
    var recording = false;
    var recordedFile;
    var livePreview;
    var pics;
    var videos;
    var flipView;
    var imageProcessor;
    var screensList;


    WinJS.UI.Pages.define("/pages/record/record.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {
            createOrOpenVideoFolder();
            Data.setUserBox();
            init();

            this._checkPing();
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
        }
    });

    function createOrOpenVideoFolder() {
        var videosLib = Storage.KnownFolders.videosLibrary;
        videosLib.createFolderAsync("My Video Recorder", Storage.CreationCollisionOption.openIfExists)
          .then(function (folder) {
              videos = folder;
          });
    }

    function init() {
        var deviceInfo = Windows.Devices.Enumeration.DeviceInformation;
        deviceInfo.findAllAsync(Windows.Devices.Enumeration.DeviceClass.videoCapture).then(function (devices) {
            if (devices.length > 0) {
                Windows.Media.MediaControl.addEventListener("soundlevelchanged", soundLevelChangedHandler);
                livePreview = document.getElementById("live-preview");
                livePreview.addEventListener("click", capture);
                startCamera();
            } else {
                document.getElementById("capture-status").innerText = "No camera device was found."
            }
        });
    }

    function startCamera() {
        mediaCapture = new Capture.MediaCapture();
        mediaCapture.initializeAsync().then(function () {
            livePreview.src = URL.createObjectURL(mediaCapture);
            livePreview.play();
        });
    }

    function capture() {
        if (!recording) {
            recording = true;
            startRecording();
        } else {
            recording = false;
            stopRecording();
        }
    }

    function startRecording() {
        document.getElementById("capture-status").innerText = "Recording.  Tap screen to stop."
        videos.createFileAsync("video.mp4", Windows.Storage.CreationCollisionOption.generateUniqueName)
          .then(function (file) {
              var profile =
                 Windows.Media.MediaProperties.MediaEncodingProfile
                        .createMp4(Windows.Media.MediaProperties.VideoEncodingQuality.auto);

              mediaCapture.startRecordToStorageFileAsync(profile, file).then(function () {
                  recordedFile = file;
              });
          });
    }

    function stopRecording() {
        mediaCapture.stopRecordAsync().then(function () {
            document.getElementById("capture-status").innerText = "Tap screen to capture media";
        });
    }

    function soundLevelChangedHandler() {
        if (Windows.Media.MediaControl.soundLevel !== Windows.Media.SoundLevel.muted) {
            startCamera();
        }
    }
})();
