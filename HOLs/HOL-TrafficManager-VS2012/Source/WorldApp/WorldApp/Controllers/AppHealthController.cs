namespace WorldApp.Controllers
{
    using Microsoft.WindowsAzure;
    using System.Web.Mvc;
    using WorldApp.Services;

    public class AppHealthController : Controller
    {
        public ActionResult Index()
        {
            string serviceUrlPrefix = CloudConfigurationManager.GetSetting("HostedServiceUrlPrefix");

            ServiceManager manager = new ServiceManager();
            bool appIsOnline = manager.GetHostedServiceStatus(serviceUrlPrefix);
            if (!appIsOnline)
            {
                Response.Close();
            }
            
            return new EmptyResult();
        }
    }
}
