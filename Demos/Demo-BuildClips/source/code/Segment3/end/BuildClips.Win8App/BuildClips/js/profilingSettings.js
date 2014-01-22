(function () {
    "use strict";

    WinJS.UI.Pages.define("/profilingSettings.html", {
        // This function is called whenever a user navigates to this page. It
        // populates the page elements with the app's data.
        ready: function (element, options) {

            var toggle = document.getElementById('toggleDisplayProfiling').winControl;

            toggle.checked = Data.profilingEnabled;

            toggle.addEventListener('change', function () {

                if (toggle.checked) {
                    Data.messagesElement.style.display = 'block';
                    Data.enableProfiling();
                }
                else {
                    Data.messagesElement.style.display = 'none'
                    Data.disableProfiling();
                }
            });

            var clearCache = document.getElementById('clearCache');

            clearCache.addEventListener('click', function () {
                Data.clearCache = true;
            });
        }
    });
})();
