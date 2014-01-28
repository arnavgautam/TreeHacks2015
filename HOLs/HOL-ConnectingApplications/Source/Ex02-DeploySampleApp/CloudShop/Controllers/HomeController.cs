namespace CloudShop.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CloudShop.Models;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Index()
        {
            return Search(null);
        }

        [HttpPost]
        public ActionResult Add(string selectedProduct)
        {
            if (selectedProduct != null)
            {
                List<string> cart = this.Session["Cart"] as List<string> ?? new List<string>();
                cart.Add(selectedProduct);
                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(string SearchCriteria)
        {
            Services.IProductRepository productRepository = new Services.ProductsRepository();
            var products = string.IsNullOrEmpty(SearchCriteria) ?
                productRepository.GetProducts() : productRepository.Search(SearchCriteria);

            // add all products currently not in session
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            var model = new IndexViewModel()
            {
                Products = filteredProducts,
                SearchCriteria = SearchCriteria
            };

            return View("Index", model);
        }

        public ActionResult Checkout()
        {
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var model = new IndexViewModel()
            {
                Products = itemsInSession
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Remove(string Products)
        {
            if (Products != null)
            {
                var itemsInSession = this.Session["Cart"] as List<string>;
                if (itemsInSession != null)
                {
                    itemsInSession.Remove(Products);
                }
            }

            return RedirectToAction("Checkout");
        }
    }
}