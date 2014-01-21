namespace CloudSurvey.Controllers
{
    using System;
    using System.Web.Mvc;
    using CloudSurvey.Repositories;
    using CloudSurvey.Services;

    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.BaseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            return this.View();
        }

        public ActionResult Summary(string surveySlug)
        {
            ViewBag.SurveySlug = surveySlug;
            return this.View();
        }
    }
}