namespace FacilityApp.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class FacilityServiceBase
    {
        public async Task<IEnumerable<FacilityRequest>> GetRequestsAsync()
        {
            return await MobileServiceClientProvider.MobileClient.GetTable<FacilityRequest>().ReadAsync();
        }

        public async Task InsertRequestAsync(FacilityRequest job)
        {
            await MobileServiceClientProvider.MobileClient.GetTable<FacilityRequest>().InsertAsync(job);
        }

        public async Task UpdateRequestAsync(FacilityRequest job)
        {
            await MobileServiceClientProvider.MobileClient.GetTable<FacilityRequest>().UpdateAsync(job);
        }

        public abstract Task<string> LoginAsync(bool clearCache, string authorityId, string redirectUri, string resourceId, string clientId);
    }
}
