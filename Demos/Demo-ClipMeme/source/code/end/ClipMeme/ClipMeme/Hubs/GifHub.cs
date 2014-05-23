namespace ClipMeme.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class GifServerHub : Hub
    {
        private readonly IHubContext _hub;

        public GifServerHub()
        {
            _hub = GlobalHost.ConnectionManager.GetHubContext<GifClientHub>();
        }

        public void GifGenerationCompleted(string clientId, string blobName)
        {
            _hub.Clients.All.GifGenerationCompleted(blobName);
        }
    }

    public class GifClientHub : Hub
    {
    }
}