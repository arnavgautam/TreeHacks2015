namespace TicTacToe.Web.Controllers
{
    using System.Web.Mvc;

    public class TicTacToeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            this.SetConfigurationData();
            return View();
        }

        private void SetConfigurationData()
        {
            this.ViewBag.BlobUrl = System.Configuration.ConfigurationManager.AppSettings["BlobUrl"];
            this.ViewBag.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];
            this.ViewBag.NodeJsUrl = System.Configuration.ConfigurationManager.AppSettings["NodeJsUrl"];
        }
    }
}
