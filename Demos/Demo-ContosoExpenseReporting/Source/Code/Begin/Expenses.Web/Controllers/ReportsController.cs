namespace Expenses.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Expenses.Web.Models;
    using Expenses.Web.Repository;

    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IExpenseReportsRepository expenseReportRepository;
        private readonly IUserProfileRepository userProfileRepository;

        public ReportsController()
            : this(new ExpenseReportsRepository(), new UserProfileRepository())
        {
        }

        public ReportsController(IExpenseReportsRepository expenseReportRepository, IUserProfileRepository userProfileRepository)
        {
            this.expenseReportRepository = expenseReportRepository;
            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public ActionResult Index(string status)
        {
            IEnumerable<ExpenseReport> reports;

            ViewBag.Status = status ?? "all reports";
            var userId = this.GetUserId();

            if (User.IsInRole("manager"))
            {
                reports = this.expenseReportRepository.GetEmployeesReports(userId, status, 0);
                ViewBag.Header = string.Format("{0} ({1})", string.IsNullOrEmpty(status) ? "All Reports" : status + " Reports", reports.Count());
            }
            else
            {
                reports = string.IsNullOrEmpty(status) ? this.expenseReportRepository.GetUserReports(userId) : this.expenseReportRepository.GetUserReports(userId, status);
                ViewBag.Header = string.Format("{0} ({1})", string.IsNullOrEmpty(status) ? "All Reports" : "Filtered by " + status + " status", reports.Count());
            }

            return View(reports);
        }

        [HttpPost]
        public JsonResult Index(ExpenseReport report)
        {
            return this.CreateOrUpdate(report);
        }

        [HttpGet]
        public ActionResult GetOrUpdate(int id)
        {
            var report = this.expenseReportRepository.GetReport(id);
            return View("Detail", report);
        }

        [HttpPut]
        public JsonResult GetOrUpdate(ExpenseReport report)
        {
            return this.CreateOrUpdate(report);
        }

        public ActionResult Edit(int id)
        {
            var report = this.expenseReportRepository.GetReport(id);
            return View(report);
        }

        [HttpPost]
        public ActionResult ApproveReject(int id, string comments, string action)
        {
            var report = this.expenseReportRepository.GetReport(id);
            if (report != null)
            {
                report.StatusId = ReportsContextInitializer.RejectedStatus;
                report.StatusId = "Approve".Equals(action) ? ReportsContextInitializer.ApprovedStatus : ReportsContextInitializer.RejectedStatus;
                report.Comments = comments;
                report.ApproverName = User.Identity.Name;
                this.expenseReportRepository.Save(report);
            }

            return RedirectToAction("GetOrUpdate", id);
        }

        public ActionResult New()
        {
            return View("Edit", new ExpenseReport());
        }

        public ActionResult Delete(int id)
        {
            this.expenseReportRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Summary()
        {
            var result = this.expenseReportRepository.GetEmployeesReports(this.GetUserId(), "pending", 0);
            return Json(new { pending = result.Count() }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult CreateOrUpdate(ExpenseReport report)
        {
            try
            {
                report.UserId = this.GetUserId();
                if (report.StatusId == ReportsContextInitializer.PendingStatus)
                {
                    report.Submitted = DateTime.Now;
                }

                this.expenseReportRepository.Save(report);
                return Json(new { success = true, reportId = report.Id });
            }
            catch (Exception)
            {
                return Json(new { error = "error" });
            }
        }

        private Guid GetUserId()
        {
            return this.userProfileRepository.GetProfile(User.Identity.Name).UserId;
        }
    }
}
