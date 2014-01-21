(function () {
    "use strict";

    var page = WinJS.UI.Pages.define("/pages/upload/upload.html", {

        ready: function (element, options) {
            id("startUpload").addEventListener("click", uploadFile, false);
            id("tagsContainer").addEventListener("click", validateTagsSelection, false);
            id("tags").addEventListener("change", validateTagsSelection, false);
            id("videoDescription").value = "";
           
            WinJS.Utilities.query(".tag").listen("click", toggleTag);
            WinJS.Utilities.query("#private").listen("click", makePrivate);
            WinJS.Utilities.query("#public").listen("click", makePublic);

            Data.setUserBox();

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

    function makePrivate(e) {
        $(e.target).addClass('selected');
        $("#public").removeClass('selected');
    }

    function makePublic(e) {
        $(e.target).addClass('selected');
        $("#private").removeClass('selected');
    }

    function toggleTag(e) {
        var el = $(e.target);
        if (el.hasClass('active')) el.removeClass('active');
        else el.addClass('active');
    }

    function validateTagsSelection() {
        var validationForm = id("fileUpload");
        var firstActiveTag = validationForm.querySelector(".tag.active");
        var otherTagsField = id("tags");

        var isValid = !!firstActiveTag || !!otherTagsField.value;

        if (!isValid) {
            WinJS.Utilities.addClass(validationForm.querySelector(".tags-validation-container"), "invalid");
        } else {
            WinJS.Utilities.removeClass(validationForm.querySelector(".tags-validation-container"), "invalid");
        }

        return isValid;
    }

    function validateUploadForm(validationForm) {
        var isFormValid = validationForm.checkValidity();
        var isTagSelectionValid = validateTagsSelection();
            
        return isFormValid && isTagSelectionValid;
    }

    function uploadFile() {
        var validationForm = id("fileUpload");
        if (!validateUploadForm(validationForm)) {
            WinJS.Utilities.addClass(validationForm, "show-validations");
            return
        } else if (WinJS.Utilities.hasClass(validationForm, "show-validations")) {
            WinJS.Utilities.removeClass(validationForm, "show-validations");
        }

        // Verify that we are currently not snapped, or that we can unsnap to open the picker.
        var currentState = Windows.UI.ViewManagement.ApplicationView.value;
        if (currentState === Windows.UI.ViewManagement.ApplicationViewState.snapped && !Windows.UI.ViewManagement.ApplicationView.tryUnsnap()) {
            displayError("Error: File picker cannot be opened in snapped mode. Please unsnap first.");
        }

        var filePicker = new Windows.Storage.Pickers.FileOpenPicker();
        filePicker.fileTypeFilter.replaceAll(["*"]);
        filePicker.pickSingleFileAsync().then(function (file) {
            if (!file) {
                displayError("Error: No file selected.");
                return;
            }

            var title = id("videoTitle").value;
            var description = id("videoDescription").value;

            var upload = new UploadOperation();
            upload.startUpload(file, title, description);
        });
    }

    function createUploadStatusIndicator(){
        return {
            progress: document.getElementById("uploadProgress"),
            uploadStatus: document.getElementById("uploadStatus"),
            uploadButton: document.getElementById("startUpload"),

            showUploading: function () {
                this.progress.style.display = "inline-block";
                this.uploadButton.disabled = true;
                this.uploadButton.innerHTML = "uploading...";
            },

            showCompleted: function (valid) {
                this.progress.style.display = "";
                if (valid) {
                    this.uploadButton.innerHTML = "completed!!!";
                }
                else {
                    this.uploadButton.innerHTML = "completed";
                }
            },

            showError: function (errorMessage) {
                this.progress.style.display = "";
                this.uploadStatus.innerHTML = "Communication Error - please check your network connection.";
                this.uploadStatus.setAttribute("class", "error");
                this.uploadButton.disabled = false;
                this.uploadButton.innerHTML = "upload";
            }
        };
    };
        
    // Class associated with each upload.
    function UploadOperation() {
        var upload = null;
        var uploadStatusIndicator = createUploadStatusIndicator();

        this.startUpload = function (file, title, description) {

            var uploader = new Windows.Networking.BackgroundTransfer.BackgroundUploader();
            var contentParts = [];
            
            // File part
            var filePart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("videoFile", file.name);
            filePart.setHeader("Content-Type", file.contentType);
            filePart.setFile(file);
            contentParts.push(filePart);

            // Title part
            var titlePart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("title");
            titlePart.setText(title == "" ? null : title);
            contentParts.push(titlePart);

            // Description part
            var descriptionPart = new Windows.Networking.BackgroundTransfer.BackgroundTransferContentPart("description");
            descriptionPart.setText(description == "" ? null : description);
            contentParts.push(descriptionPart);

            WebApi.uploadVideo(uploadStatusIndicator, uploader, contentParts, complete, error, progress);
        };

        // Returns true if this is the upload identified by the guid.
        this.hasGuid = function (guid) {
            return upload.guid === guid;
        };

        // Progress callback.
        function progress(upload) {
            // Output all attributes of the progress parameter.
            printLog(upload.guid + " - Progress: ");
            var currentProgress = upload.progress;
            for (var att in currentProgress) {
                printLog(att + ": " + currentProgress[att] + ", ");
            }
            printLog("<br/>");

            // Handle various pause status conditions. This will never happen when using POST verb (the default)
            // but may when using PUT. Application can change verb used by using method property of BackgroundUploader class.
            if (currentProgress.status === Windows.Networking.BackgroundTransfer.BackgroundTransferStatus.pausedCostedNetwork) {
                printLog("Upload " + upload.guid + " paused because of costed network <br\>");
            } else if (currentProgress.status === Windows.Networking.BackgroundTransfer.BackgroundTransferStatus.pausedNoNetwork) {
                printLog("Upload " + upload.guid + " paused because network is unavailable.<br\>");
            }
        }

        // Completion callback.
        function complete(upload, uploadStatusIndicator) {
            printLog(upload.guid + " - upload complete. Status code: " + upload.getResponseInformation().statusCode + "<br/>");
            displayStatus(upload.guid + " - upload complete.");
            uploadStatusIndicator.showCompleted(true);
        }

        // Error callback.
        function error(err, upload, uploadStatusIndicator) {
            if (upload) {
                printLog(upload.guid + " - upload completed with error. Error message: '" + err.message + "'<br/>");
            }
            displayException(err);
            uploadStatusIndicator.showError(err.message);
        }
    }

    function displayException(err) {
        var message;
        if (err.stack) {
            message = err.stack;
        }
        else {
            message = err.message;
        }

        var errorStatus = Windows.Networking.BackgroundTransfer.BackgroundTransferError.getStatus(err.number);
        if (errorStatus === Windows.Web.WebErrorStatus.cannotConnect) {
            message = "App cannot connect. Network may be down, connection was refused or the host is unreachable.";
        }

        displayError(message);
    }

    // Print helper function.
    function printLog(/*@type(String)*/txt) {
        var console = document.getElementById("outputConsole");
        if (console) {
            console.innerHTML += txt;
        }
    }

    function displayError(/*@type(String)*/message) {
        WinJS.log && WinJS.log(message, "BuildClips", "error");
    }

    function displayStatus(/*@type(String)*/message) {
        WinJS.log && WinJS.log(message, "BuildClips", "status");
    }

    function id(elementId) {
        return document.getElementById(elementId);
    }
})();