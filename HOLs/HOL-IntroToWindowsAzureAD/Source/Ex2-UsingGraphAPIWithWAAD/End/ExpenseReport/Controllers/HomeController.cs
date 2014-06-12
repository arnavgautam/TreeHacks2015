using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Configuration;
using System.Data.Services.Client;
using Microsoft.WindowsAzure.ActiveDirectory;
using Microsoft.WindowsAzure.ActiveDirectory.GraphHelper;

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
            string tenantName = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            // retrieve the clientId and password values from the Web.config file
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string password = ConfigurationManager.AppSettings["Password"];

            // get a token using the helper
            AADJWTToken token = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);

            // initialize a graphService instance using the token acquired from previous step
            DirectoryDataService graphService = new DirectoryDataService(tenantName, token);

            // get Users
            var users = graphService.users;
            QueryOperationResponse<User> response;
            response = users.Execute() as QueryOperationResponse<User>;
            List<User> userList = response.ToList();
            ViewBag.userList = userList;

            // For subsequent Graph Calls, the existing token should be used.
            // The following checks to see if the existing token is expired or about to expire in 2 mins
            // if true, then get a new token and refresh the graphService
            int tokenMins = 2;
            if (token.IsExpired || token.WillExpireIn(tokenMins))
            {
                AADJWTToken newToken = DirectoryDataServiceAuthorizationHelper.GetAuthorizationToken(tenantName, clientId, password);
                token = newToken;
                graphService = new DirectoryDataService(tenantName, token);
            }

            // get tenant information
            var tenant = graphService.tenantDetails;
            QueryOperationResponse<TenantDetail> responseTenantQuery;
            responseTenantQuery = tenant.Execute() as QueryOperationResponse<TenantDetail>;
            List<TenantDetail> tenantInfo = responseTenantQuery.ToList();
            ViewBag.OtherMessage = "User List from tenant: " + tenantInfo[0].displayName;

            return View(userList);
        }
    }
}