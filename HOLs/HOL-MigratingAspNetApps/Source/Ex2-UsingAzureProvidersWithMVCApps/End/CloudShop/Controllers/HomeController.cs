using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudShop.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var products = this.HttpContext.Application["Products"] as List<string>;
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();

            // add all products currently not in session
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            //// Add additional filters here
            // filter product list for home users
            if (User.IsInRole("Home"))
            {
                filteredProducts = filteredProducts.Where(item => item.Contains("Home"));
            }

            return this.View(filteredProducts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
