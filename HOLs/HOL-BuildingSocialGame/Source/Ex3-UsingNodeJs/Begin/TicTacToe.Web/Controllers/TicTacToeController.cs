namespace TicTacToe.Web.Controllers
{
    using System.Web.Mvc;

    public class TicTacToeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
