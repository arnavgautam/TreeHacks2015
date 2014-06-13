namespace MobileClient.Services
{
    using System;
    using System.Threading.Tasks;
    using FacilityApp.Core;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.WindowsAzure.MobileServices;
    using MobileClient.Common;
    using Newtonsoft.Json.Linq;

    public class FacilityService : FacilityServiceBase
    {
        public override async Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId)
        {
            return string.Empty;
        }
    }
}
