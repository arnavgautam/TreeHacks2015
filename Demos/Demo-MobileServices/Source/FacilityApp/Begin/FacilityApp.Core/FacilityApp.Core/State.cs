namespace FacilityApp.Core
{
    public static class State
    {
        private static string sharePointToken = string.Empty;

        public static string SharePointToken 
        { 
            get 
            {
                return sharePointToken;
            }

            set
            {
                sharePointToken = value;
            }
        }
    }
}
