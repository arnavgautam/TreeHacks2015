namespace CloudSurvey
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "SurveysApi",
                routeTemplate: "api/surveys/{id}",
                defaults: new { Controller = "Survey", id = RouteParameter.Optional });

            routes.MapHttpRoute(
                name: "SubmissionsApi",
                routeTemplate: "api/surveys/{surveyId}/submissions/{submissionId}",
                defaults: new { Controller = "SurveySubmission", submissionId = RouteParameter.Optional });

            routes.MapRoute(
                name: "SurveySummary",
                url: "Summary/{surveySlug}",
                defaults: new { controller = "Admin", action = "Summary" });

            routes.MapRoute(
                name: "Help",
                url: "help",
                defaults: new { controller = "Help", action = "Index" });

            routes.MapRoute(
                name: "SurveyForm",
                url: "{surveySlug}",
                defaults: new { controller = "SurveyForm", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional });
        }
    }
}