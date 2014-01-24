namespace CloudShop.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using CloudShop.Models;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bool enableCache = (bool)this.Session["EnableCache"];
            bool enableLocalCache = (bool)this.Session["EnableLocalCache"];

            // retrieve product catalog from repository and measure the elapsed time
            Services.IProductRepository productRepository = new Services.ProductsRepository(enableCache, enableLocalCache);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var products = productRepository.GetProducts();
            stopWatch.Stop();

            // add all products currently not in session
            var itemsInSession = this.Session["Cart"] as List<string> ?? new List<string>();
            var filteredProducts = products.Where(item => !itemsInSession.Contains(item));

            IndexViewModel model = new IndexViewModel()
            {
                Products = filteredProducts,
                ElapsedTime = stopWatch.ElapsedMilliseconds,
                IsCacheEnabled = enableCache,
                IsLocalCacheEnabled = enableLocalCache,
                ObjectId = products.GetHashCode().ToString()
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

        public ActionResult EnableCache(bool enabled)
        {
            this.Session["EnableCache"] = enabled;
            return this.RedirectToAction("Index");
        }

        public ActionResult EnableLocalCache(bool enabled)
        {
            this.Session["EnableLocalCache"] = enabled;
            return this.RedirectToAction("Index");
        }
    }
}
