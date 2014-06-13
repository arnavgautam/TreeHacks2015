using System.Configuration;

namespace ClipMeme.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            ViewBag.Username = ConfigurationManager.AppSettings["DisplayName"];

            return this.View();
        }
    }
}
