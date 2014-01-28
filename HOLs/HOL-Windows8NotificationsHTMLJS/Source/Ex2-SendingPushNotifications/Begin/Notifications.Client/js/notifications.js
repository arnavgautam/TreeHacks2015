//
// Sample code adapted from the Windows Azure Toolkit for Windows 8
// Get the full toolkit from http://watwindows8.codeplex.com
//

var channel;
var serverUrl = "http://[YOUR_WEBSITE_DOMAIN]/endpoints";
var appId = "MyApp1";
var deviceId = "MyDevice1";
var userId = "UserId1";
var tileId = "MyTile";

function openNotificationsChannel() {
    var pushNotifications = Windows.Networking.PushNotifications;
    UpdateStatusMessage("1. Requesting Channel from WNS: ");
    var channelOperation = pushNotifications.PushNotificationChannelManager.createPushNotificationChannelForApplicationAsync();

    return channelOperation.then(function (newChannel) {
        channel = newChannel;
        UpdateStatusMessage("    Channel URI returned by WNS: " + channel.uri);
        updateChannelUri(channel.uri, channel.expirationTime, 1);
    },
    function (error) {    
            UpdateStatusMessage("   Could not create a channel (error number: " + error.number + " - " + error.message + ")");
    });
}

function updateChannelUri(channel, channelExpiration, callNumber) {
    if (channel) {
        var payload =
        {
            ApplicationId: appId,
            ChannelUri: channel,
            Expiry: channelExpiration.toString(),
            DeviceId: deviceId,
            UserId: userId,
            TileId: tileId,
            ClientId: deviceId 
        };

        UpdateStatusMessage("2. Attempting to registering channel URI with Notification App Server at " + serverUrl);
        var xhr = new WinJS.xhr({
            type: "PUT",
            url: serverUrl,
            headers: { "Content-Type": "application/json; charset=utf-8" },
            data: JSON.stringify(payload)
        }).then(function (req) {
            UpdateStatusMessage("   Channel URI successfully sent to Notification App Server.");
        },
    function (req) {           
                UpdateStatusMessage("   Could not send Channel URI to Notification App Server");
        });
    }
}

function closeNotificationsChannel() {
    if (channel) {
        try {
            UpdateStatusMessage("Deleting User Channel URI from server");
            var xhr = new WinJS.xhr({
                type: "DELETE",
                url: serverUrl + "/" + appId + "/" + deviceId,
                headers: { "Content-Type": "application/json; charset=utf-8" }
            }).then(function (req) {
                UpdateStatusMessage("Channel URI deleted from server!");
                channel.close();
                channel = null;
                UpdateStatusMessage("Channel has been closed and deleted from server!");
            },
                function (req) {
                    UpdateStatusMessage("Could not send delete request to server");
                });
        } catch (e) {
            UpdateStatusMessage(e.message);
        }
    } else {
        UpdateStatusMessage("No channel is open.");
    }
}

function UpdateStatusMessage(message) {    
    var item = document.createElement("div");
    item.innerText = message;
    document.getElementById("statusMessage").appendChild(item);
}