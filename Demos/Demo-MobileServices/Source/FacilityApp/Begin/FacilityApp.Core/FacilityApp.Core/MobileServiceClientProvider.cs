namespace FacilityApp.Core
{
    using Microsoft.WindowsAzure.MobileServices;

    public static class MobileServiceClientProvider
    {
        private static MobileServiceClient mobileClient;

        public static MobileServiceClient MobileClient 
        {
            get { return mobileClient; }
        }

        public static void InitializeClient(string mobSvcUri, string appKey)
        {
            mobileClient = new MobileServiceClient(mobSvcUri, appKey);
        }
    }
}
