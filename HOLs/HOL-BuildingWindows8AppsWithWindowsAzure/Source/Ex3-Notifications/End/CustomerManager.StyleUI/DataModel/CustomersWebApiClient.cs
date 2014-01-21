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
    class CustomersWebApiClient
    {
        public static async Task<IEnumerable<Customer>> GetCustomers()
        {
            object serviceUrl;
            App.Current.Resources.TryGetValue("ServiceUrl", out serviceUrl);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(serviceUrl as string);

                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<Customer>));
                    return serializer.ReadObject(stream) as IEnumerable<Customer>;
                }
            }
        }

        public static async void CreateCustomer(Customer customer)
        {
            object serviceUrl;
            App.Current.Resources.TryGetValue("ServiceUrl", out serviceUrl);

            using (HttpClient client = new HttpClient())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Customer));

                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, customer);
                    stream.Seek(0, SeekOrigin.Begin);

                    var json = new StreamReader(stream).ReadToEnd();

                    var response = await client.PostAsync(serviceUrl as string, new StringContent(json, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
