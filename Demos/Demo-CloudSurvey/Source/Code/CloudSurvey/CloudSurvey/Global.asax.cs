namespace CloudSurvey
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using CloudSurvey.Repositories;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SurveyContext, Migrations.Configuration>());
            //// Database.SetInitializer<SurveyContext>(new DropCreateDatabaseIfModelChanges<SurveyContext>()); 

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleDependencyResolver();

            using (var context = new SurveyContext())
            {
                var dummy = context.Surveys.FirstOrDefault();
            }

            InitializeSecurity();
        }

        private static void InitializeSecurity()
        {
            if (Membership.FindUsersByName("admin").Cast<MembershipUser>().FirstOrDefault() == null)
            {
                Membership.CreateUser("admin", "password", "admin@contoso.com");
            }
        }
    }
}