using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventBuddy.Helpers
{
    public static class TwitterHelper
    {
        public static async Task<TwitterUser> RetrieveUserInformation(string userId)
        {
            var client = new HttpClient();
            var url = string.Format("http://api.twitter.com/1/users/lookup.xml?user_id={0}", userId);
            var response = await client.GetAsync(new Uri(url));
            var responseString = await response.Content.ReadAsStringAsync();

            var document = XDocument.Parse(responseString, LoadOptions.None);

            var screenName = document.Root.Descendants().Where(e => e.Name == "screen_name").First().Value;
            string imageUrl = document.Root.Descendants().Where(e => e.Name == "profile_image_url").First().Value;
            string handle = string.Format("@{0}", screenName);

            return new TwitterUser { Handle = handle, PictureUrl = imageUrl };
        }
    }
}