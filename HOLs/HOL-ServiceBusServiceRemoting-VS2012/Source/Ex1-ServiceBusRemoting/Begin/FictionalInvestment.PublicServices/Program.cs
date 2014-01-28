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

namespace FictionalInvestment.PublicServices
{
    using System;
    using System.ServiceModel;

    internal class Program
    {
        internal static void Main()
        {
            using (ServiceHost serviceHost = new ServiceHost(typeof(CrmPublicService)))
            {
                serviceHost.Open();

                Console.WriteLine("The service CrmPublicService is ready.");
                Console.WriteLine(string.Format("Listening at: {0}", serviceHost.Description.Endpoints[0].Address.Uri.AbsoluteUri));
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
