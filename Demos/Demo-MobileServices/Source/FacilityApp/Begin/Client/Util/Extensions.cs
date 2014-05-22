namespace MobileClient.Util
{
    using System;
    using System.Threading.Tasks;

    using FacilityApp.Core;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.MobileServices;

    using MobileClient.Common;

    using Newtonsoft.Json.Linq;

    public static class MyExtensions
    {
        public static async Task<MobileServiceUser> LoginWithActiveDirectoryAsync(this MobileServiceClient svcClient, AuthenticationResult token)
        {
            var payload = new JObject();
            payload["access_token"] = token.AccessToken;

            //await FacilityServiceBase.MobileClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
            if (token.AccessToken == null)
            {
                return null;
            }

            var authContext = new AuthenticationContext(ConfigurationHub.ReadConfigurationValue("AadAuthority"), false);

            // Get the sharepoint token
            var authenticationResult = await authContext.AcquireTokenByRefreshTokenAsync(token.RefreshToken, ConfigurationHub.ReadConfigurationValue("AadClientID"), ConfigurationHub.ReadConfigurationValue("SharePointResource"));
            State.SharePointToken = authenticationResult.AccessToken;
            return svcClient.CurrentUser;
        }
    }
}
