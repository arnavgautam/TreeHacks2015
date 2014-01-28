namespace TicTacToe.Web.Controllers
{
    using System.Web.Mvc;

    public class TicTacToeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
