namespace Expenses.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Expenses.Web.Repository;
    using Expenses.Web.ViewModels;

    public class HomeController : Controller
    {
        private readonly IExpenseReportsRepository expenseReportRepository;
        private readonly IUserProfileRepository userProfileRepository;

        public HomeController()
            : this(new ExpenseReportsRepository(), new UserProfileRepository())
        {
        }

        public HomeController(IExpenseReportsRepository expenseReportRepository, IUserProfileRepository userProfileRepository)
        {
            this.expenseReportRepository = expenseReportRepository;
            this.userProfileRepository = userProfileRepository;            
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Dashboard");
            }
            
            return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {            
            var userId = this.GetUserId();

            int pending;
            int approved;
            int rejected;

            // TODO: Create repository method to retrieve the count
            if (User.IsInRole("manager"))
            {
                pending = this.expenseReportRepository.GetEmployeesReports(userId, "pending", 0).ToList().Count();
                approved = this.expenseReportRepository.GetEmployeesReports(userId, "approved", 0).ToList().Count();
                rejected = this.expenseReportRepository.GetEmployeesReports(userId, "rejected", 0).ToList().Count();
            }
            else
            {
                pending = this.expenseReportRepository.GetUserReports(userId, "pending").ToList().Count();
                approved = this.expenseReportRepository.GetUserReports(userId, "approved").ToList().Count();
                rejected = this.expenseReportRepository.GetUserReports(userId, "rejected").ToList().Count();
            }

            return View(new DashboardItems { Pending = pending, Approved = approved, Rejected = rejected });
        }
        
        public ActionResult Support()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Policies()
        {
            return View();
        }

        private Guid GetUserId()
        {
            return this.userProfileRepository.GetProfile(User.Identity.Name).UserId;
        }
    }
}
