namespace BuildClips
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.ServiceBus;
    using Microsoft.WindowsAzure.ServiceRuntime;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // SignalR backplane using the Windows Azure Service Bus
            GlobalHost.DependencyResolver.UseWindowsAzureServiceBus(
                      ConfigurationManager.AppSettings["ServiceBusConnectionString"],
                      topicCount: 5,
                      instanceCount: RoleEnvironment.CurrentRoleInstance.Role.Instances.Count);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}