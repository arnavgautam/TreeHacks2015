namespace UsingTopics.Controllers
{
    using System;
    using System.Web.Mvc;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class HomeController : Controller
    {
        private NamespaceManager namespaceManager;
        private MessagingFactory messagingFactory;

        public HomeController()
        {
            var baseAddress = RoleEnvironment.GetConfigurationSettingValue("namespaceAddress");
            var issuerName = RoleEnvironment.GetConfigurationSettingValue("issuerName");
            var issuerKey = RoleEnvironment.GetConfigurationSettingValue("issuerKey");

            Uri namespaceAddress = ServiceBusEnvironment.CreateServiceUri("sb", baseAddress, string.Empty);

            this.namespaceManager = new NamespaceManager(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
            this.messagingFactory = MessagingFactory.Create(namespaceAddress, TokenProvider.CreateSharedSecretTokenProvider(issuerName, issuerKey));
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}