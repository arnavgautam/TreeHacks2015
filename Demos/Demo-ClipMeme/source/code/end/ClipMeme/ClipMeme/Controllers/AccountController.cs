namespace ClipMeme.Controllers
{
    using System.Web.Mvc;

    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Login()
        {
            return this.View("Index");
        }
	}
}