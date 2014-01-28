[assembly: WebActivator.PreApplicationStartMethod(typeof(Notifications.Backend.App_Start.NotificationService), "PreStart")]

namespace Notifications.Backend.App_Start
{
    using System.Configuration;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Routing;
    using Microsoft.WindowsAzure;
    using Notifications.Backend.CloudServices.Notifications;
    using Notifications.Backend.CloudServices.Notifications.WindowsAzure;

    public static class NotificationService
    {
        public static void PreStart()
        {
            // Configure the default values for the Push Notifications Registration Service:
            // - Anonymous access
            // - Windows Azure Storage Tables as the storage provider
            NotificationServiceContext.Current.Configure(
                c =>
                {
                    // TODO: Specify additional authentication rules
                    c.AuthenticateRequest = currentRequest => true;

                    // TODO: Specify a rule for whether users can access the Management Operations (get, all)
                    c.AuthorizeManagementRequest = currentRequest => true;

                    // TODO: Specify a rule for authorizing users when registring (register, unregister)
                    c.AuthorizeRegistrationRequest = AuthorizeUserRequest;

                    // TODO: Replace with your own Windows Azure Storage account name and key, or read it from a configuration file
                    CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSettingPublisher) =>
                    {
                        var connectionString = ConfigurationManager.AppSettings[configName];
                        configSettingPublisher(connectionString);
                    });

                    //var act = new CloudStorageAccount(new StorageCredentialsAccountAndKey("ajholwp","DMZ6Gaec53G7EF4u4HcsgUjZCRutx0pKxwi40s+u1DRPwUow+UaZSb1fhbpdTFsDpsuOYMZqai/Bv83suRfVUQ=="), true);
                    var act = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                    c.StorageProvider = new WindowsAzureEndpointRepository(act);

                    // TODO: Specify the handlers you want for ASP.NET Web API Registration Service (authentication, logging, etc)
                    // c.DelegatingHandlers = new[] { // Your DelegatingHandler instances };
                });

            RouteTable.Routes.MapRegistrationServiceRoute("endpoints");
        }

        public static bool AuthorizeUserRequest(HttpActionContext context)
        {
            var configuration = NotificationServiceContext.Current.Configuration;
            var repository = configuration.StorageProvider;
            var message = context.Request;

            Endpoint requestedEndpoint = null;

            if (message.Method != HttpMethod.Delete)
            {
                var readTask = message.Content.ReadAsAsync<Endpoint>();
                readTask.Wait();

                var endpoint = readTask.Result;
                if (endpoint != null)
                {
                    requestedEndpoint = repository.Find(endpoint.ApplicationId, endpoint.TileId, endpoint.ClientId);
                }

                // Since the content is now disposed, we need to restore it so it reaches the action
                message.Content = new ObjectContent<Endpoint>(endpoint, context.ControllerContext.Configuration.Formatters[0]);
            }
            else
            {
                var applicationId = context.ControllerContext.RouteData.Values["applicationId"] as string;
                var tileId = context.ControllerContext.RouteData.Values["tileId"] as string;
                var clientId = context.ControllerContext.RouteData.Values["clientId"] as string;

                requestedEndpoint = repository.Find(applicationId, tileId, clientId);
            }

            return requestedEndpoint == null || requestedEndpoint.UserId == configuration.MapUsername(message);
        }
    }
}