namespace MobileClient.Services
{
    using System;
    using System.Threading.Tasks;
    using FacilityApp.Core;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.MobileServices;

    using Newtonsoft.Json.Linq;

    public class FacilityService : FacilityServiceBase
    {
        public override async Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId)
        {
            var context = new AuthenticationContext(authorityId);
            var result = await context.AcquireTokenAsync(resourceId, clientId);

            // Build our token
            var token = JObject.FromObject(new
            {
                access_token = result.AccessToken,
            });

            // Request access to Azure Mobile Services
            await MobileServiceClientProvider.MobileClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, token);
            return result.AccessToken;
        }
    }
}
