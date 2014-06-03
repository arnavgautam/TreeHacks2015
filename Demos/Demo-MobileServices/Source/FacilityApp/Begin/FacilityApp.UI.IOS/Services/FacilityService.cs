namespace FacilityApp.UI.IOS.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BindingNewADAL;
    using FacilityApp.Core;
    using Microsoft.WindowsAzure.MobileServices;
    using Newtonsoft.Json.Linq;

    public class FacilityService : FacilityServiceBase
    {
        public Action LoginCompletedAction { get; set; }

        public override Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId)
        {
            ADAuthenticationError error;
            var context = ADAuthenticationContext.AuthenticationContextWithAuthority(authorityId, out error);
            var redirectUrl = new Uri(redirectUri);

            if (clearCache)
            {
                context.TokenCacheStore.RemoveAll();
            }

            context.AcquireTokenWithResource(resourceId, clientId, redirectUrl, async (result) =>
            {
                JObject payload = new JObject();
                payload["access_token"] = result.AccessToken;
                try
                {
                    await MobileServiceClientProvider.MobileClient.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
                }
                catch (Exception)
                {
                }

                this.LoginCompletedAction();
            });

            return Task.FromResult(string.Empty);
        }
    }
}