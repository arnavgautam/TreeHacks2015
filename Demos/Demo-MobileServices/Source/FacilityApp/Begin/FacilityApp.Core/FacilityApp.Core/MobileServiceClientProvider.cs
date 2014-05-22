using Microsoft.WindowsAzure.MobileServices;

namespace FacilityApp.Core
{
    public static class MobileServiceClientProvider
    {
        public static MobileServiceClient MobileClient;

        public static void InitializeClient(string mobSvcUri, string appKey)
        {
            MobileClient = new MobileServiceClient(mobSvcUri, appKey);
        }
    }
}
