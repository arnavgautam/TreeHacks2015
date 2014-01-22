namespace CloudSurvey.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class SurveyFormController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string surveySlug)
        {
            this.ViewBag.SurveySlug = surveySlug;
            return this.View();
        }
    }
}