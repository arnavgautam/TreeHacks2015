namespace UsingTopics.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using UsingTopics.Models;

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

        [HttpPost]
        public JsonResult CreateTopic(string topicName)
        {
            var topic = this.namespaceManager.CreateTopic(topicName);
            var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
            var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages");

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

        [HttpPost]
        public void SendMessage(string topicName, string messageBody, bool isUrgent, bool isImportant)
        {
            TopicClient topicClient = this.messagingFactory.CreateTopicClient(topicName);
            var customMessage = new CustomMessage() { Body = messageBody, Date = DateTime.Now };
            var bm = new BrokeredMessage(customMessage);
            bm.Properties["Urgent"] = isUrgent ? "1" : "0";
            bm.Properties["Important"] = isImportant ? "1" : "0";

            // Force message priority to "Low". Subscription filters will change the value automatically if applies
            bm.Properties["Priority"] = "Low";
            topicClient.Send(bm);

            if (bm != null)
            {
                bm.Dispose();
            }
        }

        [HttpGet, OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult RetrieveMessage(string topicName, string subscriptionName)
        {
            SubscriptionClient subscriptionClient = this.messagingFactory.CreateSubscriptionClient(topicName, subscriptionName, ReceiveMode.PeekLock);
            BrokeredMessage receivedMessage = subscriptionClient.Receive(new TimeSpan(0, 0, 30));

            if (receivedMessage == null)
            {
                return this.Json(null, JsonRequestBehavior.AllowGet);
            }

            var receivedCustomMessage = receivedMessage.GetBody<CustomMessage>();

            receivedMessage.Properties["Priority"] = receivedMessage.Properties["Priority"].ToString() == "1" ? "High" : "Low";

            var brokeredMsgProperties = new Dictionary<string, object>();
            brokeredMsgProperties.Add("Size", receivedMessage.Size);
            brokeredMsgProperties.Add("MessageId", receivedMessage.MessageId.Substring(0, 15) + "...");
            brokeredMsgProperties.Add("TimeToLive", receivedMessage.TimeToLive.TotalSeconds);
            brokeredMsgProperties.Add("EnqueuedTimeUtc", receivedMessage.EnqueuedTimeUtc.ToString("yyyy-MM-dd HH:mm:ss"));
            brokeredMsgProperties.Add("ExpiresAtUtc", receivedMessage.ExpiresAtUtc.ToString("yyyy-MM-dd HH:mm:ss"));

            var messageInfo = new
            {
                Label = receivedMessage.Label,
                Date = receivedCustomMessage.Date,
                Message = receivedCustomMessage.Body,
                Properties = receivedMessage.Properties.ToArray(),
                BrokeredMsgProperties = brokeredMsgProperties.ToArray()
            };

            receivedMessage.Complete();
            return this.Json(messageInfo, JsonRequestBehavior.AllowGet);
        }
    }
}