using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Configuration;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace ExpenseReport.Controllers
{
    public class HomeController : Controller
    {
        private const string Resource = "https://graph.windows.net";
        private const string AuthString = "https://login.windows.net/{0}";
        private const string ClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";

        public ActionResult Index()
        {
            ClaimsPrincipal cp = ClaimsPrincipal.Current;
            ViewBag.Message = string.Format("Dear \"{0}, {1}\", welcome to the Expense Note App", cp.FindFirst(ClaimTypes.Surname).Value, cp.FindFirst(ClaimTypes.GivenName).Value);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Users()
        {
            // get the tenantName
            var tenantId = ClaimsPrincipal.Current.FindFirst(ClaimType).Value;

            var authenticationContext = new AuthenticationContext(
                string.Format(AuthString, tenantId),
                false);
            authenticationContext.TokenCacheStore.Clear();

            // retrieve the clientId and password from the Web.config file 
            var clientId = ConfigurationManager.AppSettings["ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["Password"];

            // create the credentials and get a token
            var clientCred = new ClientCredential(clientId, clientSecret);

            try
            {
                var token = authenticationContext.AcquireToken(Resource, clientCred).AccessToken;
                
                if (!string.IsNullOrEmpty(token))
                {
                    var graphSettings = new GraphSettings
                    {
                        ApiVersion = "2013-11-08",
                        GraphDomainName = "graph.windows.net"
                    };

                    //  get tenant information
                    var graphConnection = new GraphConnection(token, graphSettings);
                    var tenant = graphConnection.Get(typeof(TenantDetail), tenantId);
                    if (tenant != null)
                    {
                        var tenantDetail = (TenantDetail)tenant;
                        ViewBag.OtherMessage = "User List from tenant: " + tenantDetail.DisplayName;
                    }

                    // get the list of users
                    var pagedReslts = graphConnection.List<User>(null, new FilterGenerator());
                    return View(pagedReslts.Results);
                }
            }
            catch (ActiveDirectoryAuthenticationException ex)
            {
                ViewBag.OtherMessage = string.Format("Acquiring a token failed with the following error: {0}", ex.Message);
               }
            catch (AuthorizationException e)
            {
                ViewBag.OtherMessage = string.Format("Failed to return the list of Users with Exception: {0}", e.ErrorMessage);                
            }

            return View();
        }
    }
}