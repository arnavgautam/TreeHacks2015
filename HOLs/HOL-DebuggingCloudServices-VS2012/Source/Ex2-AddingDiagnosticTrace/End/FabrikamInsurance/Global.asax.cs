namespace FabrikamInsurance
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private static void ConfigureTraceListener()
        {
            bool enableTraceListener = false;
            string enableTraceListenerSetting = RoleEnvironment.GetConfigurationSettingValue("EnableTableStorageTraceListener");
            if (bool.TryParse(enableTraceListenerSetting, out enableTraceListener))
            {
                if (enableTraceListener)
                {
                    AzureDiagnostics.TableStorageTraceListener listener =
                      new AzureDiagnostics.TableStorageTraceListener("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString")
                      {
                          Name = "TableStorageTraceListener"
                      };

                    System.Diagnostics.Trace.Listeners.Add(listener);
                    System.Diagnostics.Trace.AutoFlush = true;
                }
                else
                {
                    System.Diagnostics.Trace.Listeners.Remove("TableStorageTraceListener");
                }
            }
        }

        private void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            // configure trace listener for any changes to EnableTableStorageTraceListener 
            if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName == "EnableTableStorageTraceListener"))
            {
                ConfigureTraceListener();
            }
        }

        protected void Application_Start()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
            });

            ConfigureTraceListener();

            RoleEnvironment.Changed += this.RoleEnvironmentChanged;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var lastError = Server.GetLastError();
            System.Diagnostics.Trace.TraceError(lastError.Message);
        }
    }
}