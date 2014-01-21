using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;

namespace ExpenseReport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ClaimsPrincipal cp = ClaimsPrincipal.Current;
            string fullname =
                   string.Format("{0} {1}", cp.FindFirst(ClaimTypes.GivenName).Value,
                   cp.FindFirst(ClaimTypes.Surname).Value);
            ViewBag.Message = string.Format("Dear {0}, welcome to the Expense Note App",
                              fullname);
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
    }
}
