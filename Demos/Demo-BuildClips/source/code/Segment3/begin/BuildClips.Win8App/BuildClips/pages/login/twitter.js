var TwitterAuthentication = WinJS.Class.define(
    function () { },
    {
        getAccessToken: function (callbackURL, consumerKey, consumerSecret, requestToken, requestTokenSecret, oauthVerifier, callback) {

            this.sendRequest('https://api.twitter.com/oauth/access_token', 'POST', function (response) {
                if (response === false) {
                    callback(false);
                } else {
                    var keyValPairs = response.split("&");

                    var userAccessToken, userAccessTokenSecret, twitterUserId, screenName;

                    for (var i = 0; i < keyValPairs.length; i++) {
                        var splits = keyValPairs[i].split("=");
                        switch (splits[0]) {
                            case "oauth_token":
                                userAccessToken = splits[1];
                                break;
                            case "oauth_token_secret":
                                userAccessTokenSecret = splits[1];
                                break;
                            case "user_id":
                                twitterUserId = splits[1];
                                break;
                            case "screen_name":
                                screenName = splits[1];
                                break;
                        }
                    }

                    callback({
                        oauth_token: userAccessToken,
                        oauth_token_secret: userAccessTokenSecret,
                        twitter_user_id: twitterUserId,
                        screen_name: screenName
                    });
                }
            },
            callbackURL,
            consumerKey,
            consumerSecret,
            requestToken,
            undefined,// requestTokenSecret
            oauthVerifier);
        },

        authenticate: function (consumerKey, consumerSecret, callbackURL, callback) {

            var that = this;

            this.getRequestToken(consumerKey, consumerSecret, callbackURL, function (requestTokenData) {

                startURI = new Windows.Foundation.Uri("https://api.twitter.com/oauth/authorize?oauth_token=" + requestTokenData.oauth_token);
                endURI = new Windows.Foundation.Uri(callbackURL);

                Windows.Security.Authentication.Web.WebAuthenticationBroker.authenticateAsync(
                Windows.Security.Authentication.Web.WebAuthenticationOptions.none, startURI, endURI)
                    .done(function (result) {
                        var twitterResponseData = result.responseData,
                            twitterResponseStatus = result.responseStatus;

                        if (twitterResponseStatus === Windows.Security.Authentication.Web.WebAuthenticationStatus.errorHttp) {
                            WinJS.log("Error authenticating: " + result.responseErrorDetail, "BuildClips", "error");
                            callback(false);
                        } else if (twitterResponseData) {
                            responseKeyValPairs = twitterResponseData.split("?")[1].split("&");

                            for (i = 0; i < responseKeyValPairs.length; i++) {
                                splits = responseKeyValPairs[i].split("=");
                                switch (splits[0]) {
                                    case "oauth_verifier":
                                        oauthVerifier = splits[1];
                                        break;
                                }
                            }
                            
                            that.getAccessToken(callbackURL, consumerKey, consumerSecret, requestTokenData.oauth_token, requestTokenData.oauth_token_secret, oauthVerifier, function (accessTokenData) {
                                accessTokenData.oauth_verifier = oauthVerifier;
                                callback(accessTokenData);
                                });
                        }
                    }, function (err) {
                        WinJS.log("Error authenticating: " + err.message, "BuildClips", "error");
                        callback(false);
                    });
            });
        },

        sendRequest: function (url, method, callback, callbackURL, consumerKey, consumerSecret, oauthToken, oauthTokenSecret, oauthVerifier) {

            var header = this.getAuthHeader(method, url, callbackURL, consumerKey, consumerSecret, oauthToken, oauthTokenSecret, oauthVerifier);

            try {
                var request = new XMLHttpRequest();
                request.open(method, url, true);
                request.onreadystatechange = function () {
                    if (request.readyState === 4) {
                        if (request.status === 200) {
                            callback(request.responseText);
                        } else {
                            callback(false);
                        }
                    }
                };

                request.setRequestHeader("Authorization", header);
                request.send();
            } catch (err) {
                WinJS.log("Error sending request: " + err.message, "BuildClips", "error");
                callback(false);
            }
        },

        getRequestToken: function (consumerKey, consumerSecret, callbackURL, callback) {

            var requestToken,
                requestTokenSecret;

            this.sendRequest('https://api.twitter.com/oauth/request_token', 'POST', function (response) {
                if (response) {
                    var keyValPairs = response.split("&");

                    for (var i = 0; i < keyValPairs.length; i++) {
                        var splits = keyValPairs[i].split("=");
                        switch (splits[0]) {
                            case "oauth_token":
                                requestToken = splits[1];
                                break;
                            case "oauth_token_secret":
                                requestTokenSecret = splits[1];
                                break;
                        }
                    }

                    callback({
                        oauth_token: requestToken,
                        oauth_token_secret: requestTokenSecret
                    });
                }
                else {
                    callback(false);
                }
            },
            callbackURL,
            consumerKey,
            consumerSecret);
        },

        getAuthHeader: function (method, url, callbackURL, consumerKey, consumerSecret, oauthToken, oauthTokenSecret, oauthVerifier) {
            var nonce = Math.floor(Math.random() * 1000000000);
            var timestamp = Math.round(new Date().getTime() / 1000.0);

            var signatureParams = {
                oauth_callback: !oauthTokenSecret ? encodeURIComponent(callbackURL) : "",
                oauth_consumer_key: consumerKey,
                oauth_nonce: nonce,
                oauth_signature_method: 'HMAC-SHA1',
                oauth_timestamp: timestamp,
                oauth_token: oauthToken,
                oauth_verifier: oauthVerifier,
                oauth_version: '1.0'
            };

            var key, allkeys = [];

            for (key in signatureParams) {
                if (signatureParams.hasOwnProperty(key)) {
                    allkeys[allkeys.length] = key;
                }
            }

            sortedKeys = allkeys.sort();

            var keyName, keyValue;

            var signatureParamsStr = "";

            for (i = 0; i < sortedKeys.length; i++) {
                keyName = sortedKeys[i];
                keyValue = signatureParams[sortedKeys[i]];
                if (keyValue && keyValue !== '') {
                    if (signatureParamsStr !== '') {
                        signatureParamsStr = signatureParamsStr + '&';
                    }
                    signatureParamsStr += keyName + '=' + keyValue;
                }
            }

            var signatureStr = method + "&" + encodeURIComponent(url) + "&" + encodeURIComponent(signatureParamsStr);

            var keyMaterialString = encodeURIComponent(consumerSecret) + "&";
            if (oauthTokenSecret) {
                keyMaterialString += encodeURIComponent(oauthTokenSecret);
            }

            var keyMaterial = Windows.Security.Cryptography.CryptographicBuffer.convertStringToBinary(keyMaterialString, Windows.Security.Cryptography.BinaryStringEncoding.Utf8);
            var macAlgorithmProvider = Windows.Security.Cryptography.Core.MacAlgorithmProvider.openAlgorithm("HMAC_SHA1");
            var cryptoKey = macAlgorithmProvider.createKey(keyMaterial);
            var tbs = Windows.Security.Cryptography.CryptographicBuffer.convertStringToBinary(signatureStr, Windows.Security.Cryptography.BinaryStringEncoding.Utf8);
            var signatureBuffer = Windows.Security.Cryptography.Core.CryptographicEngine.sign(cryptoKey, tbs);
            var signature = Windows.Security.Cryptography.CryptographicBuffer.encodeToBase64String(signatureBuffer);

            var headers = "OAuth " + (!oauthTokenSecret ? ("oauth_callback=\"" + encodeURIComponent(callbackURL) + "\", ") : "") +
                "oauth_consumer_key=\"" + consumerKey +
                "\", oauth_nonce=\"" + nonce +
                "\", oauth_signature=\"" + encodeURIComponent(signature) +
                "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + timestamp +
                (oauthToken ? ("\", oauth_token=\"" + oauthToken) : "") +
                (oauthVerifier ? ("\", oauth_verify=\"" + oauthVerifier) : "") +
                "\", oauth_version=\"1.0\"";

            return headers;
        }
    },
    {}
);

