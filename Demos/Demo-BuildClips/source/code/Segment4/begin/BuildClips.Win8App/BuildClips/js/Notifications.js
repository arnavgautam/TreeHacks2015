(function () {
    "use strict";

    var connection = null;

    WinJS.Namespace.define("Notifications", {
        connect: connect
    });

    function connect() {
        if (connection == null) {
            connection = $.hubConnection(Configuration.ApiBaseUrl);
            var hub = connection.createHubProxy("Notifier");
            hub.on("onVideoUpdate", function (video) {
                Data.updateVideoItem(video);
            });

            connection.start({ waitForPageLoad: false });
        }
    }

    connect();
})();
