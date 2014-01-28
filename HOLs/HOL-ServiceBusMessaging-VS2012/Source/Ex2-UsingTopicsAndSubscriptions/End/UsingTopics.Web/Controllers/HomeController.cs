using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using UsingTopics.Web.Models;

namespace UsingTopics.Web.Controllers
{
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
            return View();
        }

        [HttpPost]
        public JsonResult CreateTopic(string topicName)
        {
            bool success;
            try
            {
                var topic = this.namespaceManager.CreateTopic(topicName);
                var allMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "AllMessages");
                var urgentMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "UrgentMessages", new SqlFilter("Urgent = '1'"));

                var ruleDescription = new RuleDescription()
                {
                    Filter = new SqlFilter("Important= '1' OR Priority = 'High'"),
                    Action = new SqlRuleAction("set Priority= 'High'")
                };
                var highPriorityMessagesSubscription = this.namespaceManager.CreateSubscription(topic.Path, "HighPriorityMessages", ruleDescription);
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return this.Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendMessage(string topicName, string message, bool messageIsUrgent, bool messageIsImportant)
        {
            TopicClient topicClient = this.messagingFactory.CreateTopicClient(topicName);
            var customMessage = new CustomMessage() { Body = message, Date = DateTime.Now };
            bool success = false;
            BrokeredMessage bm = null;

            try
            {
                bm = new BrokeredMessage(customMessage);
                bm.Properties["Urgent"] = messageIsUrgent ? "1" : "0";
                bm.Properties["Important"] = messageIsImportant ? "1" : "0";
                bm.Properties["Priority"] = "Low";
                topicClient.Send(bm);
                success = true;
            }
            catch (Exception)
            {
                // TODO: do something
            }
            finally
            {
                if (bm != null)
                {
                    bm.Dispose();
                }
            }

            return this.Json(success, JsonRequestBehavior.AllowGet);
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

            receivedMessage.Properties["Priority"] = receivedMessage.Properties["Important"].ToString() == "1" ? "High" : "Low";

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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Subscriptions(string topicName)
        {
            var subscriptions = this.namespaceManager.GetSubscriptions(topicName).Select(c => c.Name);
            return this.Json(subscriptions, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult TopicsWithSubscriptions()
        {
            var topics = this.namespaceManager.GetTopics().Select(c => c.Path).ToList();
            var topicsToReturn = new Dictionary<string, object>();
            topics.ForEach(c =>
            {
                var subscriptions = this.namespaceManager.GetSubscriptions(c).Select(d => new { Name = d.Name, MessageCount = d.MessageCount });
                topicsToReturn.Add(c, subscriptions);
            });

            return this.Json(topicsToReturn.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Filters(string topicName, string subscriptionName)
        {
            var rules = this.namespaceManager.GetRules(topicName, subscriptionName);
            var sqlFilters = new List<Tuple<string, string>>();

            foreach (var rule in rules)
            {
                var expression = rule.Filter as SqlFilter;
                var action = rule.Action as SqlRuleAction;

                if (expression != null)
                {
                    sqlFilters.Add(
                        new Tuple<string, string>(
                            expression.SqlExpression,
                            action != null ? action.SqlExpression : string.Empty));
                }
            }

            return this.Json(sqlFilters.Select(t => new { Filter = t.Item1, Action = t.Item2 }), JsonRequestBehavior.AllowGet);
        }

        public long GetMessageCount(string topicName, string subscriptionName)
        {
            var subscriptionDescription = this.namespaceManager.GetSubscription(topicName, subscriptionName);
            return subscriptionDescription.MessageCount;
        }
    }
}