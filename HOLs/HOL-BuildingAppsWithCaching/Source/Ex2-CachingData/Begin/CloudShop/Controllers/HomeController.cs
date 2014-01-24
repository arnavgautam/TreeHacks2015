namespace CloudShop.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CloudShop.Models;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Services.IProductRepository productRepository = new Services.ProductsRepository();
            var products = productRepository.GetProducts();

            // add all products currently not in session
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            IndexViewModel model = new IndexViewModel()
            {
                Products = filteredProducts
            };

            return this.View(model);
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

        public EmptyResult Recycle()
        {
            RoleEnvironment.RequestRecycle();
            return new EmptyResult();
        }
    }
}
