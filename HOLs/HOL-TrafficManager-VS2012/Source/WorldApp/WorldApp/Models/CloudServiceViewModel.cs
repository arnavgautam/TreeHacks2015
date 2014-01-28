namespace WorldApp.Models
{
    using System.Collections.Generic;
    using WorldApp.Services;

    public class CloudServiceViewModel
    {
        public CloudServiceViewModel()
        {
            this.HostedServices = new Dictionary<string, CloudServiceStatus>();
        }

        public string HttpHost { get; set; }

        public string ClientIPAddress { get; set; }

        public string ClientHostName { get; set; }

        public string ServerName { get; set; }

        public string CurrentRegion { get; set; }

        public string DnsTtl { get; set; }

        public IDictionary<string, CloudServiceStatus> HostedServices { get; private set; }
    }
}