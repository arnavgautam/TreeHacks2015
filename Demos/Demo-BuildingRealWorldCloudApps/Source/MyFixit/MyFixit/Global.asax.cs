namespace MyFixIt
{
    using System.Data.Entity;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using MyFixIt.App_Start;
    using MyFixIt.Logging;
    using MyFixIt.Persistence;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependenciesConfig.RegisterDependencies();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            PhotoService photoService = new PhotoService(new Logger());
            photoService.CreateAndConfigure();

            var configuration = new MyFixIt.Persistence.EFConfiguration();

            DbConfiguration.SetConfiguration(configuration);

            IdentityConfig.ConfigureIdentity();
        }

        protected void Application_PostAuthenticateRequest()
        {
            ClaimsPrincipal currentPrincipal = ClaimsPrincipal.Current;

            if (currentPrincipal != null && currentPrincipal.Claims.Count(x => x.Type == IdentityConfig.GiveNameClaimType) > 0)
            {
                var claims = IdentityConfig.RemoveUserIdentityClaims(currentPrincipal.Claims);
                IdentityConfig.AddUserIdentityClaims(claims.First(x => x.Type == IdentityConfig.GiveNameClaimType).Value, claims);
                var identity = new ClaimsIdentity(claims, currentPrincipal.Identity.AuthenticationType);
                var principal = new ClaimsPrincipal(identity);

                System.Threading.Thread.CurrentPrincipal = principal;
                HttpContext.Current.User = principal;
            }
        }
    }
}
