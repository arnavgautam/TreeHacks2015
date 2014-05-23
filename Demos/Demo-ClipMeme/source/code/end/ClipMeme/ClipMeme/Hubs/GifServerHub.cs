namespace ClipMeme.Hubs
{
    using Microsoft.AspNet.SignalR;

    public class GifServerHub : Hub
    {
        private readonly IHubContext hub;

        public GifServerHub()
        {
            this.hub = GlobalHost.ConnectionManager.GetHubContext<GifClientHub>();
        }

        public void GifGenerationCompleted(string clientId, string url)
        {
            this.hub.Clients.Client(clientId).GifGenerationCompleted(url);
        }
    }
}