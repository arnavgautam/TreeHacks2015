namespace ClipMeme.Hubs
{
    using System;
    using System.Diagnostics;
    using Microsoft.AspNet.SignalR;

    public class GifServerHub : Hub
    {
        public void GifGenerationCompleted(string clientId, string url)
        {
            Trace.TraceError(string.Format("GifGenerationCompleted {0} : {1}", url, DateTime.Now));

            this.Clients.All.GifGenerationCompleted(url);
        }
    }
}