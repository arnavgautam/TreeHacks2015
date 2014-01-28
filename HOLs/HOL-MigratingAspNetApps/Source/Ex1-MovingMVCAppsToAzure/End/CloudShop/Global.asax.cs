using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CloudShop
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });
            
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            this.LoadProducts();
        }

        private void LoadProducts()
        {
            this.Application["Products"] = new List<string>
            {
                "Microsoft Office 2007 Ultimate",
                "Microsoft Office Communications Server Enterprise CAL",
                "Microsoft Core CAL - License & software assurance - 1 user CAL",
                "Windows Server 2008 Enterprise",
                "Windows Vista Home Premium (Upgrade)",
                "Windows XP Home Edition w/SP2 (OEM)",
                "Windows Home Server - 10 Client (OEM License)",
                "Console XBOX 360 Arcade" 
            };
        }
    }
}