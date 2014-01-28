using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using CloudShop;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace CloudShop
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });

            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();

            this.LoadProducts();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

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
