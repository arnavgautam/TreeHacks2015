// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.ServiceModel;

namespace RelyingParty
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class WeatherService : IWeatherService
    {
        public WeatherInfo GetThreeDaysForecast(int zipCode)
        {
            return this.GetForecast(3, zipCode);
        }

        public WeatherInfo GetTenDaysForecast(int zipCode)
        {
            return this.GetForecast(10, zipCode);
        }

        protected WeatherInfo GetForecast(int days, int zipCode)
        {
            Weather[] forecast = new Weather[days];
            Random rand = new Random(zipCode + DateTime.Today.DayOfYear);

            for (int i = 0; i < days; i++)
            {
                Weather weather = (Weather)(rand.Next() % 4);
                forecast[i] = weather;
            }

            WeatherInfo weatherInfo = new WeatherInfo
            {
                Forecast = forecast,
                Observatory = OperationContext.Current.EndpointDispatcher.ChannelDispatcher.Listener.Uri.AbsoluteUri
            };

            // Uncomment it to verify load balancing
            // System.Threading.Thread.Sleep(3 * 1000);
            return weatherInfo;
        }
    }
}
