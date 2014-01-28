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
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;
    using Microsoft.ServiceBus;
    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WorkerRole : RoleEntryPoint
    {
        private ServiceHost serviceHost;

        public override void Run()
        {
            // Start service
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.AutoDetect;

            using (this.serviceHost = new ServiceHost(typeof(CrmPublicService)))
            {
                this.serviceHost.Open();

                Trace.TraceInformation("The worker role CrmPublicService is ready.");
                Trace.TraceInformation(string.Format("Listening at: {0}", this.serviceHost.Description.Endpoints[0].Address.Uri.AbsoluteUri));

                while (true)
                {
                    Thread.Sleep(30000);
                    Trace.TraceInformation("Working ...");
                }
            }
        }
        
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Diagnostic configuration
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromSeconds(120);
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Information;
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
            this.serviceHost.Close();
        }
    }
}