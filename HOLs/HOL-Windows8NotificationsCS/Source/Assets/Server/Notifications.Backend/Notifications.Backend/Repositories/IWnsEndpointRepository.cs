namespace Notifications.Backend.Repositories
{
    using System;
    using System.Collections.Generic;
    using Notifications.Backend.CloudServices.Notifications;

    public interface IWnsEndpointRepository
    {
        Endpoint GetEndpoint(string applicationId, string tileId, string clientId);

        IEnumerable<Endpoint> GetAllEndpoints();
    }
}