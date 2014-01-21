using System;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace EventBuddy.WindowsPhone.Helpers
{
    public static class TwitterHelper
    {
        private static Action<TwitterUser> callbackAction;
        private static Action failureCallbackAction;

        public static void RetrieveUserInformation(string userId, Action<TwitterUser> callback, Action failureCallback)
        {
            var client = new WebClient();
            var url = string.Format("http://api.twitter.com/1/users/lookup.xml?user_id={0}", userId);

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
                var responseString = args.Result;

                var document = XDocument.Parse(responseString, LoadOptions.None);

                var screenName = document.Root.Descendants().Where(e => e.Name == "screen_name").First().Value;
                string imageUrl = document.Root.Descendants().Where(e => e.Name == "profile_image_url").First().Value;
                string handle = string.Format("@{0}", screenName);

                callbackAction(new TwitterUser { Handle = handle, PictureUrl = imageUrl });
            }
            catch (Exception)
            {
                failureCallbackAction();
            }
        }
    }
}