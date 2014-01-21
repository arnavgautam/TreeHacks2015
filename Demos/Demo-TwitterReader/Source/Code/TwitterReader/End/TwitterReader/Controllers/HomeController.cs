using System.Web.Mvc;

namespace TweetReader.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "My Twitter Reader";

            return View();
        }
    }
}
