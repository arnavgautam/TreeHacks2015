using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Win8Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public async void GetItems()
        {
            var serviceURI = "[YOUR-WINDOWS-AZURE-SERVICE-URI]/api/values";

            using (var client = new System.Net.Http.HttpClient())
            using (var response = await client.GetAsync(serviceURI))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var values = JsonArray.Parse(data);

                    var valueList = from v in values
                                    select new
                                    {
                                        Name = v.GetString()
                                    };

                    this.listValues.ItemsSource = valueList;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.GetItems();
        }
    }
}
