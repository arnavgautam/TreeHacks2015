using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.AspNet.SignalR.Client.Hubs;
using BuildClips.Services;
using BuildClips.Services.Models;

namespace BackgroundService
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("BackgroundService entry point called", "Information");

            // Connect to SignalR
            var connection = new HubConnection(CloudConfigurationManager.GetSetting("ApiBaseUrl"));
            var proxy = connection.CreateHubProxy("Notifier");
            connection.Start().Wait();

            while (true)
            {
                Thread.Sleep(5000);

                var service = new VideoService();
                Trace.WriteLine("Getting Media Services active jobs", "Information");
                var activeJobs = service.GetActiveJobs();

                foreach (var video in activeJobs.ToList())
                {
                    proxy.Invoke(
                        "VideoUpdated", 
                        (video.JobStatus == JobStatus.Completed) ? service.Publish(video.Id) : video);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
