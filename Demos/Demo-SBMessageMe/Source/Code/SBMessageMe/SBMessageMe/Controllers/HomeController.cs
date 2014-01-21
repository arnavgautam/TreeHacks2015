using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBMessageMe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMessage(string msg)
        {
            // Create Queue Client
            var queueClient = QueueClient.Create("demomessages");

            // Create and Send Message to Queue
            var brokeredMessage = new BrokeredMessage(msg);
            queueClient.Send(brokeredMessage);

            // Redisplay home-page
            return RedirectToAction("Index");
        }
    }
}
