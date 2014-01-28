using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Web.Configuration;
using Microsoft.Samples.SocialGames;
using Microsoft.Samples.SocialGames.Common.Storage;
using Microsoft.Samples.SocialGames.Entities;
using Microsoft.Samples.SocialGames.Extensions;
using Microsoft.Samples.SocialGames.Repositories;
using Microsoft.Samples.SocialGames.Web.Services;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using TicTacToe.Web.WebApi;

namespace TicTacToe.Web
{
    //// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    //// visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                string configuration = RoleEnvironment.IsAvailable ?
                    RoleEnvironment.GetConfigurationSettingValue(configName) :
                    ConfigurationManager.AppSettings[configName];

                configSetter(configuration);
            });

            // Setup AutoFac
            var builder = new ContainerBuilder();
            this.DependencySetup(builder);
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Setup WCF Web API Config
            var resolver = new AutofacDependencyResolver(container);
            GlobalConfiguration.Configuration.ServiceResolver.SetResolver(
                t => resolver.GetService(t), t => resolver.GetServices(t));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            FederatedAuthentication.ServiceConfigurationCreated += this.OnServiceConfigurationCreated;

            // Call Initializers
            var initializers = DependencyResolver.Current.GetServices<IStorageInitializer>();
            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }
        }

        protected void DependencySetup(ContainerBuilder builder)
        {
            // Cloud Storage Account
            builder.RegisterInstance<CloudStorageAccount>(CloudStorageAccount.FromConfigurationSetting("DataConnectionString"));

            // Queues
            builder.RegisterQueue<GameActionStatisticsMessage>(ConfigurationConstants.GameActionStatisticsQueue)
                .AsImplementedInterfaces();
            builder.RegisterQueue<GameActionNotificationMessage>(ConfigurationConstants.GameActionNotificationsQueue)
                .AsImplementedInterfaces();
            builder.RegisterQueue<InviteMessage>(ConfigurationConstants.InvitesQueue)
                .AsImplementedInterfaces();

            // Blobs
            builder.RegisterBlob<UserProfile>(ConfigurationConstants.UsersContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<UserSession>(ConfigurationConstants.UserSessionsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<Friends>(ConfigurationConstants.FriendsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<NotificationStatus>(ConfigurationConstants.NotificationsContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<Game>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<GameQueue>(ConfigurationConstants.GamesQueuesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();
            builder.RegisterBlob<UserProfile>(ConfigurationConstants.GamesContainerName, true /* jsonpSupport */)
                .AsImplementedInterfaces();

            // Tables
            builder.RegisterTable<UserStats>(ConfigurationConstants.UserStatsTableName, true /* jsonpSupport */)
                .AsImplementedInterfaces();

            // Repositories
            builder.RegisterType<GameActionNotificationQueue>().AsImplementedInterfaces();
            builder.RegisterType<GameActionStatisticsQueue>().AsImplementedInterfaces();
            builder.RegisterType<GameRepository>().AsImplementedInterfaces();
            builder.RegisterType<IdentityProviderRepository>().AsImplementedInterfaces();
            builder.RegisterType<NotificationRepository>().AsImplementedInterfaces();
            builder.RegisterType<UserRepository>().AsImplementedInterfaces();
            builder.RegisterType<StatisticsRepository>().AsImplementedInterfaces();

            // Controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Services
            builder.RegisterType<AuthService>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<EventService>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<GameService>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<HttpContextUserProvider>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<UserService>().AsImplementedInterfaces().AsSelf();

            // Api
            builder.RegisterType<AuthController>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<GameController>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<UserController>().AsImplementedInterfaces().AsSelf();
            builder.RegisterType<EventController>().AsImplementedInterfaces().AsSelf();
        }

        private void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            var sessionTransforms = new List<CookieTransform>(new CookieTransform[] { new DeflateCookieTransform() });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }
    }
}