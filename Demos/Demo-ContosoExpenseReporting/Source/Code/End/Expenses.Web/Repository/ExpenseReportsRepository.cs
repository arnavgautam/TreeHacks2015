namespace Expenses.Web.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Expenses.Web.Models;

    public class ExpenseReportsRepository : IExpenseReportsRepository
    {
        public void Delete(int reportId)
        {
            using (var ctx = this.GetContext())
            {
                var report = ctx.ExpenseReports.Find(reportId);
                if (report != null)
                {
                    ctx.ExpenseReports.Remove(report);
                    ctx.SaveChanges();
                }
            }
        }

        public ExpenseReport GetReport(int reportId)
        {
            using (var ctx = this.GetContext())
            {
                return this.LoadReferences(ctx, ctx.ExpenseReports.Find(reportId));
            }
        }

        public IEnumerable<ExpenseReport> GetReports(bool includeDrafts)
        {
            return this.GetReports(includeDrafts, 0);
        }

        public IEnumerable<ExpenseReport> GetReports(bool includeDrafts, int count)
        {
            using (var ctx = this.GetContext())
            {
                IEnumerable<ExpenseReport> reports = includeDrafts ?
                    ctx.ExpenseReports.OrderByDescending(r => r.LastModified) :
                    ctx.ExpenseReports.Where(r => !r.StatusId.Equals(1)).OrderByDescending(r => r.LastModified);

                if (count > 0)
                {
                    reports = reports.Take(count);
                }

                return this.LoadReferences(ctx, reports.ToList());
            }
        }

        public IEnumerable<ExpenseReport> GetReports(string status)
        {
            return this.GetReports(status, 0);
        }

        public IEnumerable<ExpenseReport> GetReports(string status, int count)
        {
            using (var ctx = this.GetContext())
            {
                IEnumerable<ExpenseReport> reports = ctx.ExpenseReports.Where(r => r.Status.Name.Equals(status)).OrderByDescending(r => r.LastModified);

                if (count > 0)
                {
                    reports = reports.Take(count);
                }

                return this.LoadReferences(ctx, reports.ToList());
            }
        }

        public IEnumerable<ExpenseReport> GetEmployeesReports(Guid managerId, string status, int count)
        {
            using (var ctx = this.GetContext())
            {
                IEnumerable<ExpenseReport> reports = from report in ctx.ExpenseReports
                                                     join user in ctx.UserProfiles on report.User.ManagerId equals user.UserId
                                                     where string.IsNullOrEmpty(status) ?
                                                     (!report.Status.Name.Equals("draft") && user.UserId.Equals(managerId)) :
                                                     (report.Status.Name.Equals(status) && user.UserId.Equals(managerId))
                                                     orderby report.LastModified descending
                                                     select report;

                if (count > 0)
                {
                    reports = reports.Take(count);
                }

                return this.LoadReferences(ctx, reports.ToList());
            }
        }

        public IEnumerable<ExpenseReport> GetUserReports(Guid userId)
        {
            return this.GetUserReports(userId, 0);
        }

        public IEnumerable<ExpenseReport> GetUserReports(Guid userId, int count)
        {
            using (var ctx = this.GetContext())
            {
                IEnumerable<ExpenseReport> reports = ctx.ExpenseReports.Where(r => r.UserId.Equals(userId)).OrderByDescending(r => r.LastModified);

                if (count > 0)
                {
                    reports = reports.Take(count);
                }

                return this.LoadReferences(ctx, reports.ToList());
            }
        }

        public IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status)
        {
            return this.GetUserReports(userId, status, 0);
        }

        public IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status, int count)
        {
            using (var ctx = this.GetContext())
            {
                IEnumerable<ExpenseReport> reports = ctx.ExpenseReports.Where(r => r.UserId.Equals(userId) && r.Status.Name.Equals(status)).OrderByDescending(r => r.LastModified);

                if (count > 0)
                {
                    reports = reports.Take(count);
                }

                return this.LoadReferences(ctx, reports.ToList());
            }
        }

        public ExpenseReport Save(ExpenseReport report)
        {
            using (var ctx = this.GetContext())
            {
                var targetReport = this.LoadReferences(ctx, ctx.ExpenseReports.Find(report.Id));

                if (targetReport == null)
                {
                    if (report.Details != null)
                    {
                        report.Details.ToList().ForEach(d =>
                        {
                            d.Report = report;
                            ctx.ExpenseReportDetails.Add(d);
                        });
                    }
                    else
                    {
                        report.Details = new List<ExpenseReportDetail>();
                    }

                    ctx.ExpenseReports.Add(report);
                }
                else
                {
                    targetReport.CopyFrom(report);

                    if (report.Details != null)
                    {
                        targetReport.Details.Where(d => !report.Details.Contains(d))
                                    .ToList()
                                    .ForEach(d =>
                                    {
                                        ctx.ExpenseReportDetails.Remove(d);
                                    });

                        report.Details.Where(d => targetReport.Details.Contains(d))
                                        .ToList()
                                        .ForEach(d =>
                                        {
                                            var targetExpense = targetReport.Details.Where(e => e.Id.Equals(d.Id)).FirstOrDefault();
                                            targetExpense.CopyFrom(d);
                                        });

                        report.Details.Where(d => !targetReport.Details.Contains(d))
                                      .ToList()
                                      .ForEach(d =>
                                        {
                                            d.Report = targetReport;
                                            ctx.ExpenseReportDetails.Add(d);
                                        });
                    }
                    else
                    {
                        targetReport.Details.ToList().ForEach(d => ctx.ExpenseReportDetails.Remove(d));                        
                    }
                }

                ctx.SaveChanges();

                return report;
            }
        }

        private ReportsContext GetContext()
        {
            return new ReportsContext();
        }

        private IEnumerable<ExpenseReport> LoadReferences(DbContext ctx, IEnumerable<ExpenseReport> reports)
        {
            foreach (var report in reports)
            {
                this.LoadReferences(ctx, report);
            }

            return reports;
        }

        private ExpenseReport LoadReferences(DbContext ctx, ExpenseReport report)
        {
            if (report != null)
            {
                ctx.Entry(report).Collection(r => r.Details).Load();
                ctx.Entry(report).Reference(r => r.User).Load();
                ctx.Entry(report).Reference(r => r.Status).Load();
            }

            return report;
        }
    }
}