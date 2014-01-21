namespace MyFixIt
{
    using System.Configuration;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.ActiveDirectory;
    using Microsoft.Owin.Security.Cookies;

    using Owin;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            // Use a cookie to temporarily store information about a user this.logging in with a third party this.login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable this.logging in with third party this.login providers
            ////app.UseMicrosoftAccountAuthentication(
            ////    clientId: "",
            ////    clientSecret: "");

            ////app.UseTwitterAuthentication(
            ////   consumerKey: "",
            ////   consumerSecret: "");

            ////app.UseFacebookAuthentication(
            ////   appId: "",
            ////   appSecret: "");

            app.UseGoogleAuthentication();

            app.UseWindowsAzureActiveDirectoryBearerAuthentication(new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Audience = ConfigurationManager.AppSettings["ida:AudienceUri"],
                Realm = ConfigurationManager.AppSettings["ida:realm"],
                Tenant = "nbeniad.onmicrosoft.com",
                AuthenticationType = System.IdentityModel.Services.FederatedAuthentication.WSFederationAuthenticationModule.AuthenticationType,
                Description = new AuthenticationDescription
                {
                    AuthenticationType = "Azure",
                    Caption = "Azure"
                }
            });
        }
    }
}