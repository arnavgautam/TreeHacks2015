namespace MobileClient.Util
{
    using Windows.Networking.Connectivity;

    public static class NetworkHelper
    {
        public static bool IsConnected()
        {
            var connections = NetworkInformation.GetInternetConnectionProfile();
            return connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
        }
    }
}
