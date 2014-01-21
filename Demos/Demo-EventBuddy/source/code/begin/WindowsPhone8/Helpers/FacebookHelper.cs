using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace EventBuddy.WindowsPhone.Helpers
{
    public static class FacebookHelper
    {
        private static Action<FacebookUser> callbackAction;
        private static Action failureCallbackAction;
        private static string facebookUrl = "http://graph.facebook.com/{0}";
        
        public static void RetrieveUserInformation(string userId, Action<FacebookUser> callback, Action failureCallback)
        {
            var client = new WebClient();
            var url = string.Format(facebookUrl, userId);
            callbackAction = callback;
            failureCallbackAction = failureCallback;
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri(url));
        }

        public static void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                failureCallbackAction();
            }

            try
            {
                var jsonResult = JObject.Parse(args.Result);
                var fullName = jsonResult.SelectToken("name").ToString();
                var userName = jsonResult.SelectToken("username").ToString();
                var userId = jsonResult.SelectToken("id").ToString();
                var url = string.Format(facebookUrl, userId);
                var imageUrl = string.Format("{0}/{1}", url, "picture");

                callbackAction(new FacebookUser { UserName = userName, FullName = fullName, PictureUrl = imageUrl });
            }
            catch (Exception)
            {
                failureCallbackAction();
            }
        }
    }
}
