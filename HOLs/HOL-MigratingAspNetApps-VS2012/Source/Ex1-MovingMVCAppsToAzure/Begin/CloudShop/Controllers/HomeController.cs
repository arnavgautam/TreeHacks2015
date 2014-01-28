using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudShop.Controllers
{
	[HandleError]
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return this.View();
        }

        public ActionResult Index()
        {
            var products = this.HttpContext.Application["Products"] as List<string>;
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();

            // add all products currently not in session
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            //// Add additional filters here

            return this.View(filteredProducts);
        }

        [HttpPost]
        public ActionResult Add(string selectedItem)
        {
            if (selectedItem != null)
            {
                List<string> cart = this.Session["Cart"] as List<string> ?? new List<string>();
                cart.Add(selectedItem);
                this.Session["Cart"] = cart;
            }

            return this.RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            return this.View(itemsInSession);
        }

        [HttpPost]
        public ActionResult Remove(string selectedItem)
        {
            if (selectedItem != null)
            {
                var itemsInSession = this.Session["Cart"] as List<string>;
                if (itemsInSession != null)
                {
                    itemsInSession.Remove(selectedItem);
                }
            }

            return this.RedirectToAction("Checkout");
        }
    }
}
