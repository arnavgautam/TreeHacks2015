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

using System.Runtime.Serialization;
using System.ServiceModel;

namespace RelyingParty
{
    [ServiceContract]
    [ServiceKnownType(typeof(WeatherInfo))]
    public interface IWeatherService
    {
        [OperationContract]
        WeatherInfo GetThreeDaysForecast(int zipCode);
        
        [OperationContract]
        WeatherInfo GetTenDaysForecast(int zipCode);
    }

    [DataContract]
    public class WeatherInfo
    {
        [DataMember]
        public Weather[] Forecast { get; set; }

        [DataMember]
        public string Observatory { get; set; }
    }

    [DataContract]
    public enum Weather
    {
        [EnumMember]
        Sunny,

        [EnumMember]
        Cloudy,

        [EnumMember]
        Snowy,

        [EnumMember]
        Rainy
    }
}