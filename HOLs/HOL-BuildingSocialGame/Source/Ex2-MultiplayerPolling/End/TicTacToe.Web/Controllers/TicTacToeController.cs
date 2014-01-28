namespace TicTacToe.Web.Controllers
{
    using System.Web.Mvc;

    public class TicTacToeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
