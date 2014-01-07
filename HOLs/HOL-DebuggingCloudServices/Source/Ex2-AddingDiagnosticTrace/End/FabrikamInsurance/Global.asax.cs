namespace FabrikamInsurance
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.Storage;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ConfigureTraceListener();

            RoleEnvironment.Changed += this.RoleEnvironmentChanged;
            RoleEnvironment.Changing += this.RoleEnvironmentChanging;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // for any configuration setting change except EnableTableStorageTraceListener
            if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName != "EnableTableStorageTraceListener"))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        protected void RoleEnvironmentChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            // configure trace listener for any changes to EnableTableStorageTraceListener 
            if (e.Changes.OfType<RoleEnvironmentConfigurationSettingChange>().Any(change => change.ConfigurationSettingName == "EnableTableStorageTraceListener"))
            {
                ConfigureTraceListener();
            }
        }

        protected void Application_Error()
        {
            var lastError = Server.GetLastError();
            System.Diagnostics.Trace.TraceError(lastError.Message);
        }

        private static void ConfigureTraceListener()
        {
            bool enableTraceListener = false;
            string enableTraceListenerSetting = CloudConfigurationManager.GetSetting("EnableTableStorageTraceListener");
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
    }
}
