(function () {
    WinJS.Namespace.define('Configuration', {
        ApiBaseUrl: 'http://127.0.0.1:81/',
        FacebookClientId: '{YOUR-FACEBOOK-CLIENTID}',
        FacebookCallbackUrl: 'https://www.facebook.com/connect/login_success.html',
        TwitterConsumerKey: '{YOUR-TWITTER-CONSUMERKEY}',
        TwitterConsumerSecret: '{YOUR-TWITTER-SECRET}',
        TwitterCallbackUrl: 'https://www.twitter.com',
        DefaultUserFirstName: "Demo User",
        DefaultUserLastName: "",
        CheckPingInterval: 5000,
    });
})();
