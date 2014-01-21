using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Data.Json;

namespace EventBuddy.Helpers
{
    public static class FacebookHelper
    {
        public static async Task<FacebookUser> RetrieveUserInformation(string userId)
        {
            var client = new HttpClient();
            var url = string.Format("http://graph.facebook.com/{0}", userId);
            var response = await client.GetAsync(new Uri(url));
            var responseString = await response.Content.ReadAsStringAsync();

            var jsonParsed = JsonValue.Parse(responseString);
            
            var fullName = jsonParsed.GetObject().GetNamedString("name");
            var userName = jsonParsed.GetObject().GetNamedString("username");
            var imageUrl = string.Format("{0}/{1}", url, "picture");

            return new FacebookUser { UserName = userName, FullName = fullName, PictureUrl = imageUrl };
        }
    }
}
