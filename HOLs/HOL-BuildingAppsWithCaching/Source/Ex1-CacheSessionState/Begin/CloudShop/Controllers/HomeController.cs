namespace CloudShop.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using CloudShop.Models;
    using System.Net;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class HomeController : Controller
    {
        private List<string> Cart
        {
            get
            {
                if (this.Session["Cart"] as List<string> == null)
                    this.Session["Cart"] = new List<string>();
                return this.Session["Cart"] as List<string>;
            }
        }

        public ActionResult Index()
        {
            Services.IProductRepository productRepository = new Services.ProductsRepository();
            var products = productRepository.GetProducts();

            // add all products currently not in session
            var filteredProducts = products.Where(p => !this.Cart.Contains(p));

            IndexViewModel model = new IndexViewModel()
            {
                Products = filteredProducts
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(string selectedItem)
        {
            if (selectedItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.Cart.Add(selectedItem);
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            return View(this.Cart);
        }

        [HttpPost]
        public ActionResult Remove(string selectedItem)
        {
            if (selectedItem == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else if (!this.Cart.Contains(selectedItem))
            {
                return HttpNotFound();
            }
            
            this.Cart.Remove(selectedItem);
            return RedirectToAction("Checkout");
        }

        public EmptyResult Recycle()
        {
            RoleEnvironment.RequestRecycle();
            return new EmptyResult();
        }
    }
}
