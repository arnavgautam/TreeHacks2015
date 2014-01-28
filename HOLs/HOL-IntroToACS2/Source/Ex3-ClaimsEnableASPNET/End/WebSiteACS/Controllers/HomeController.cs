using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace WebSiteACS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;

            DateTime birthDay = DateTime.Parse(ClaimsPrincipal.Current.FindFirst(ClaimTypes.DateOfBirth).Value);

            int days = (birthDay.DayOfYear - DateTime.Today.DayOfYear);
            ViewBag.DaysToBirthDay = ((days < 0) ? days + 365 + (DateTime.IsLeapYear(DateTime.Today.Year) ? 1 : 0) : days).ToString();
            
            return View();
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

        public ActionResult SecretPage()
        {
            ViewBag.Message = "Secret page.";

            return View();
        }
    }
}
