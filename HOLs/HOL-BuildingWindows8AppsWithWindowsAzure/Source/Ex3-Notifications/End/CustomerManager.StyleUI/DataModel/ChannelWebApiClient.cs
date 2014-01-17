using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using WebApi.Models;

namespace CustomerManager.StyleUI.DataModel
{
    class ChannelWebApiClient
    {
        public static async Task<Channel> RegisterChannel(Channel channel)
        {
            object channelServiceUrl;
            App.Current.Resources.TryGetValue("ChannelServiceUrl", out channelServiceUrl);

            using (HttpClient client = new HttpClient())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Channel));

                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, channel);
                    stream.Seek(0, SeekOrigin.Begin);

                    var json = new StreamReader(stream).ReadToEnd();

                    var response = await client.PostAsync(channelServiceUrl as string + "/create", new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();

                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        DataContractJsonSerializer responseSerializer = new DataContractJsonSerializer(typeof(Channel));
                        return responseSerializer.ReadObject(responseStream) as Channel;
                    }
                }
            }
        }
    }
}
