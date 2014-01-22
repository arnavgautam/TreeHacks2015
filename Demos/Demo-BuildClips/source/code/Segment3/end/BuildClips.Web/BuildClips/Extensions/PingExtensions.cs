namespace BuildClips.App_Start
{
    using System.Net;
    using System.Web.Mvc;

    public class PingController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
