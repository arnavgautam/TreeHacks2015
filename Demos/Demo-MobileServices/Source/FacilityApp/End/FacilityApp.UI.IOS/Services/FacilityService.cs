namespace FacilityApp.UI.IOS.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using adalbinding;
    using FacilityApp.Core;
    using Microsoft.WindowsAzure.MobileServices;
    using Newtonsoft.Json.Linq;

    public class FacilityService : FacilityServiceBase
    {
        public Action LoginCompletedAction { get; set; }

        public override Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId)
        {
            ADALBuild.GetToken(
                clearCache,
                authorityId,
                redirectUri,
                resourceId,
                clientId,
                async (result) =>
                {
                    JObject payload = new JObject();
                    payload["access_token"] = result.ToString();
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