namespace MobileClient.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Storage.Streams;

    public class SharePointProvider
    {
        public static async Task<Stream> GetUserPhoto(string sharepointUri, string token, string userName)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    Func<HttpRequestMessage> requestCreator = () =>
                    {
                        var uploadRequest = new HttpRequestMessage(HttpMethod.Get, string.Format("{0}/_api/web/getfolderbyserverrelativeurl('User%20Photos')/Folders('Profile%20Pictures')/Files('{1}_SThumb.jpg')/$value", sharepointUri, userName));
                        return uploadRequest;
                    };

                    using (var uploadRequest = requestCreator.Invoke())
                    {
                        uploadRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        var response = await client.SendAsync(uploadRequest);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return await response.Content.ReadAsStreamAsync();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return null;
        }
    }
}
