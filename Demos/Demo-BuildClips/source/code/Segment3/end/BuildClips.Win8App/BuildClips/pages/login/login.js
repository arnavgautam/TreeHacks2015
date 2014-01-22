(function () {
    "use strict";

    WinJS.UI.Pages.define("/pages/login/login.html", {
        ready: function (element, options) {
            document.getElementById("facebook").addEventListener("click", launchFacebookWebAuth, false);
            document.getElementById("twitter").addEventListener("click", launchTwitterWebAuth, false);
        },
    });

    var authzInProgress = false;

    function launchFacebookWebAuth() {
        var facebookURL = "https://www.facebook.com/dialog/oauth?client_id=";

        facebookURL += Configuration.FacebookClientId + "&redirect_uri=" + encodeURIComponent(Configuration.FacebookCallbackUrl) + "&scope=read_stream&display=popup&response_type=token";

        if (authzInProgress) {
            WinJS.log("Authorization already in Progress...", "BuildClips", "info");
            return;
        }

        var startURI = new Windows.Foundation.Uri(facebookURL);
        var endURI = new Windows.Foundation.Uri(Configuration.FacebookCallbackUrl);

        setUserImageUrl('/images/demo-user-facebook.png');

        authzInProgress = true;
        Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAsync(
            Windows.Security.Authentication.Web.WebAuthenticationOptions.none, startURI, endURI)
            .done(function (result) {
                switch (result.responseStatus) {
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.success:
                        var tokenIndex = result.responseData.indexOf("access_token=");
                        var expIndex = result.responseData.indexOf("&expires_in");
                        if (tokenIndex != -1 && expIndex != -1) {
                            var accessToken = result.responseData.substring(tokenIndex + 13, expIndex);
                            // Store the access token into the app data store.
                            Windows.Storage.ApplicationData.current.localSettings.values["access_token"] = accessToken;

                            setUserImageUrl('https://graph.facebook.com/me/picture/?access_token=' + accessToken);

                            WinJS.xhr({ url: 'https://graph.facebook.com/me/?access_token=' + accessToken, responseType: "json" })
                             .done(
                                 function complete(result) {
                                     var userData = JSON.parse(result.responseText);
                                     setUserFirstAndLastName(userData.first_name, userData.last_name);

                                     WinJS.Navigation.navigate("/pages/videoList/videoList.html");
                                 },
                                 function error(result) {
                                     setDefaultProfileUserDataAndContinue(result.responseText);
                                 });
                        }
                        break;
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp:
                        WinJS.log("Error returned by WebAuth broker: " + result.responseErrorDetail, "BuildClips", "error");
                        break;
                    case Windows.Security.Authentication.Web.WebAuthenticationStatus.userCancel:
                        break;
                    default:
                        WinJS.log("Error returned by WebAuth broker: " + result.responseStatus, "BuildClips", "error");
                        break;
                }
                authzInProgress = false;
            }, function (err) {
                WinJS.log("Error returned by WebAuth broker: " + err.responseText, "BuildClips", "error");
                authzInProgress = false;
            });
    }

    function launchTwitterWebAuth() {
        setUserImageUrl('/images/demo-user-twitter.png');

        var twitterAuth = new TwitterAuthentication();
        twitterAuth.authenticate(Configuration.TwitterConsumerKey, Configuration.TwitterConsumerSecret, Configuration.TwitterCallbackUrl, function (accessTokenData) {
            twitterAuth.sendRequest('https://api.twitter.com/1.1/account/verify_credentials.json',
                'GET',
                function (result) {
                    if (result) {
                        var userData = JSON.parse(result);

                        setUserFirstAndLastName(userData.name, "@" + userData.screen_name);
                        setUserImageUrl(userData.profile_image_url);

                        WinJS.Navigation.navigate("/pages/videoList/videoList.html");
                    }
                    else {
                        setDefaultProfileUserDataAndContinue(result.responseText);
                    }
                    authzInProgress = false;
                },
                Configuration.TwitterCallbackUrl,
                Configuration.TwitterConsumerKey,
                Configuration.TwitterConsumerSecret,
                accessTokenData.oauth_token,
                accessTokenData.oauth_token_secret);
        });

    }

    function setDefaultProfileUserDataAndContinue(errorMsg) {
        setUserFirstAndLastName(Configuration.DefaultUserFirstName, Configuration.DefaultUserLastName);

        var message = "An error occurred while retrieving user's profile data";
        if (errorMsg)
        {
            message += ": " + errorMsg;
        }

        WinJS.log(message, "BuildClips", "error");

        WinJS.Navigation.navigate("/pages/videoList/videoList.html");
    }

    function setUserImageUrl(imageUrl) {
        Windows.Storage.ApplicationData.current.localSettings.values["UserImage"] = imageUrl;
    }

    function setUserFirstAndLastName(firstName, lastName) {
        Windows.Storage.ApplicationData.current.localSettings.values["UserFirstName"] = firstName;
        Windows.Storage.ApplicationData.current.localSettings.values["UserLastName"] = lastName;
    }

})();
