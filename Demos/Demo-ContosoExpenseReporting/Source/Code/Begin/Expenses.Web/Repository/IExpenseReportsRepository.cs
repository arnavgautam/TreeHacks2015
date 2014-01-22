namespace Expenses.Web.Repository
{
    using System;
    using System.Collections.Generic;
    using Expenses.Web.Models;

    public interface IExpenseReportsRepository
    {
        void Delete(int reportId);

        ExpenseReport GetReport(int reportId);

        IEnumerable<ExpenseReport> GetReports(bool includeDrafts);

        IEnumerable<ExpenseReport> GetReports(bool includeDrafts, int count);

        IEnumerable<ExpenseReport> GetReports(string status);

        IEnumerable<ExpenseReport> GetReports(string status, int count);

        IEnumerable<ExpenseReport> GetEmployeesReports(Guid managerId, string status, int count);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, int count);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status);

        IEnumerable<ExpenseReport> GetUserReports(Guid userId, string status, int count);

        ExpenseReport Save(ExpenseReport report);
    }
}
