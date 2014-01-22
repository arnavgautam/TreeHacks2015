namespace UsingTopics.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.ServiceBus;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using UsingTopics.Models;

    public class HomeController : Controller
    {
        private NamespaceManager namespaceManager;

        public HomeController()
        {
            var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceAddress");
            var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
            var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

            Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

            this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
        }

        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public JsonResult CreateTopic(string topicName)
        {
            var topic = this.namespaceManager.CreateTopic(topicName);
            var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");

            return this.Json(topicName, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Topics()
        {
            var topics = this.namespaceManager.GetTopics().Select(c => c.Path);
            return this.Json(topics, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Subscriptions(string topicName)
        {
            var subscriptions = this.namespaceManager.GetSubscriptions(topicName).Select(c => new { Name = c.Name, MessageCount = c.MessageCount });
            return this.Json(subscriptions, JsonRequestBehavior.AllowGet);
        }
    }
}