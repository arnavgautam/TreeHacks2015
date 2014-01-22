namespace BuildClips
{
    using System.Configuration;
    using Microsoft.Web.WebPages.OAuth;

    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166
            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: ConfigurationManager.AppSettings["MicrosoftClientId"],
                clientSecret: ConfigurationManager.AppSettings["MicrosoftClientSecret"]);

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: ConfigurationManager.AppSettings["TwitterConsumerKey"],
                consumerSecret: ConfigurationManager.AppSettings["TwitterConsumerSecret"]);

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["FacebookApplicationId"],
                appSecret: ConfigurationManager.AppSettings["FacebookApplicationSecret"]);

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}