namespace Notifications.Backend.Repositories
{
    using System.Collections.Generic;
    using Notifications.Backend.CloudServices.Notifications;

    public class WnsEndpointRepository : IWnsEndpointRepository
    {
        private readonly IEndpointRepository endpointRepository;

        public WnsEndpointRepository()
        {
            this.endpointRepository = NotificationServiceContext.Current.Configuration.StorageProvider;
        }

        public IEnumerable<Endpoint> GetAllEndpoints()
        {
            return this.endpointRepository.All();
        }

        public Endpoint GetEndpoint(string applicationId, string tileId, string clientId)
        {
            return this.endpointRepository.Find(e => e.ApplicationId == applicationId && e.TileId == tileId && e.ClientId == clientId);
        }
    }
}