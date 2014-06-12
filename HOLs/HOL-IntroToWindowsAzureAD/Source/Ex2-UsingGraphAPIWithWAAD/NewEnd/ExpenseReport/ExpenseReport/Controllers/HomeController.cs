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
            string tenantId = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string authString = "https://login.windows.net/" + tenantId;

            AuthenticationContext authenticationContext = new AuthenticationContext(authString, false);

            // retrieve the clientId and password from the Web.config file 
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string clientSecret = ConfigurationManager.AppSettings["Password"];

            // create the credentials and get a token
            ClientCredential clientCred = new ClientCredential(clientId, clientSecret);
            string resource = "https://graph.windows.net";

            string token = string.Empty;

            try
            {
                AuthenticationResult authenticationResult = authenticationContext.AcquireToken(resource, clientCred);
                token = authenticationResult.AccessToken;
            }
            catch (ActiveDirectoryAuthenticationException ex)
            {
                ViewBag.OtherMessage = string.Format("Acquiring a token failed with the following error: {0}", ex.Message);
                return View();
            }
            
            GraphConnection graphConnection = null;

            if (!string.IsNullOrEmpty(token))
            {
                Guid ClientRequestId = Guid.NewGuid();
                GraphSettings graphSettings = new GraphSettings();
                graphSettings.ApiVersion = "2013-11-08";
                graphSettings.GraphDomainName = "graph.windows.net";
                graphConnection = new GraphConnection(token, ClientRequestId, graphSettings);
            }

            //  get tenant information
            GraphObject tenant = graphConnection.Get(typeof(TenantDetail), tenantId);
            if (tenant != null)
            {
                TenantDetail tenantDetail = (TenantDetail)tenant;
                ViewBag.OtherMessage = "User List from tenant: " + tenantDetail.DisplayName;
            }

            // get the list of users
            PagedResults<User> pagedReslts = graphConnection.List<User>(null, new FilterGenerator());
            return View(pagedReslts.Results);
        }

    }
}