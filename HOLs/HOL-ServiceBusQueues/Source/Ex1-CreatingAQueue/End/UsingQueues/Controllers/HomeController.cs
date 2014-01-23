using Microsoft.ServiceBus;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UsingQueues.Controllers
{
    public class HomeController : Controller
    {
        private NamespaceManager namespaceManager;

        public HomeController()
        {
            var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceName");
            var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
            var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

            Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

            this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateQueue(string queueName)
        {
            try
            {
                var queueDescription = this.namespaceManager.CreateQueue(queueName);
                return this.Json(queueDescription, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Queues()
        {
            var queues = this.namespaceManager.GetQueues().Select(c => new { Name = c.Path, Messages = c.MessageCount }).ToArray();
            return this.Json(queues, JsonRequestBehavior.AllowGet);
        }

        public long GetMessageCount(string queueName)
        {
            var queueDescription = this.namespaceManager.GetQueue(queueName);
            return queueDescription.MessageCount;
        }
    }
}