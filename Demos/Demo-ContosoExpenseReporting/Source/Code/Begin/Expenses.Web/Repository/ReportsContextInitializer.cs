namespace Expenses.Web.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Security;
    using Expenses.Web.Models;

    public class ReportsContextInitializer : CreateDatabaseIfNotExists<ReportsContext>
    {
        public const byte DraftStatus = 1;
        public const byte PendingStatus = 2;
        public const byte ApprovedStatus = 3;
        public const byte RejectedStatus = 4;

        private static Random random = new Random();
        
        protected override void Seed(ReportsContext context)
        {
            base.Seed(context);
            this.CreateDatabaseConstraints();
            this.AddUsers();

            this.AddProfiles();
            this.AddReportStatuses();
            this.AddExpenseReports();
        }

        private void AddUsers()
        {
            Roles.CreateRole("manager");
            Roles.CreateRole("user");

            Membership.CreateUser("manager", "Passw0rd!");
            Roles.AddUsersToRole(new[] { "manager" }, "manager");

            Membership.CreateUser("manager2", "Passw0rd!");
            Roles.AddUsersToRole(new[] { "manager2" }, "manager");

            Membership.CreateUser("manager3", "Passw0rd!");
            Roles.AddUsersToRole(new[] { "manager3" }, "manager");

            Membership.CreateUser("user", "Passw0rd!");
            Roles.AddUsersToRole(new[] { "user" }, "user");

            Membership.CreateUser("user2", "Passw0rd!");
            Roles.AddUsersToRole(new[] { "user2" }, "user");
        }

        private void CreateDatabaseConstraints() 
        {
            using (var ctx = new ReportsContext())
            {
                ctx.Database.ExecuteSqlCommand("ALTER TABLE UserProfile ADD CONSTRAINT uc_UserName UNIQUE(UserName)");
            }
        }

        private void AddProfiles()
        {
            using (var ctx = new ReportsContext())
            {
                var manager1 = new UserProfile() { UserId = new Guid("99038106-F2AC-41B0-9C5F-CE9D1233E0A4"), UserName = "manager", FirstName = "Sample", LastName = "Manager One", IsAdmin = true };
                ctx.UserProfiles.Add(manager1);
                var manager2 = new UserProfile() { UserId = new Guid("4D64501B-A08A-4EE1-88A9-914DB8BDFC25"), UserName = "manager2", FirstName = "Sample", LastName = "Manager Two", IsAdmin = true };
                ctx.UserProfiles.Add(manager2);
                var manager3 = new UserProfile() { UserId = new Guid("33436645-A051-402C-814F-4618F91E75E2"), UserName = "manager3", FirstName = "Sample", LastName = "Manager Three", IsAdmin = true };
                ctx.UserProfiles.Add(manager3);

                ctx.UserProfiles.Add(new UserProfile() { UserId = new Guid("BF8F48E1-714A-4501-8D96-C3C677156BF7"),  UserName = "user", FirstName = "Sample", LastName = "User One", Manager = manager1, IsAdmin = false });
                ctx.UserProfiles.Add(new UserProfile() { UserId = new Guid("7B211E9F-BE02-4A00-A41D-83761C24F66E"), UserName = "user2", FirstName = "Sample", LastName = "User Two", Manager = manager2, IsAdmin = false });

                ctx.SaveChanges();
            }
        }

        private void AddReportStatuses()
        {
            using (var ctx = new ReportsContext())
            {
                ctx.ExpenseReportStatuses.Add(new ExpenseReportStatus() { Name = "Draft", Description = "Draft report pending to be submitted" });
                ctx.ExpenseReportStatuses.Add(new ExpenseReportStatus() { Name = "Pending", Description = "Submitted report pending to be approved/rejected" });
                ctx.ExpenseReportStatuses.Add(new ExpenseReportStatus() { Name = "Approved", Description = "Report approved by a manager" });
                ctx.ExpenseReportStatuses.Add(new ExpenseReportStatus() { Name = "Rejected", Description = "Report rejected by a manager" });
                ctx.SaveChanges();
            }
        }

        private void AddExpenseReports()
        {
            var repository = new ExpenseReportsRepository();
            repository.Save(this.CreateNewExpenseReport("Trip to Denver", PendingStatus, new Guid("BF8F48E1-714A-4501-8D96-C3C677156BF7"), 3));
            repository.Save(this.CreateNewExpenseReport("Dinner with Customers", PendingStatus, new Guid("7B211E9F-BE02-4A00-A41D-83761C24F66E"), 2));
            repository.Save(this.CreateNewExpenseReport("Trip to Colorado", DraftStatus, new Guid("7B211E9F-BE02-4A00-A41D-83761C24F66E"), 4));
            repository.Save(this.CreateNewExpenseReport("Trip to Arizona", ApprovedStatus, new Guid("7B211E9F-BE02-4A00-A41D-83761C24F66E"), 3));
            repository.Save(this.CreateNewExpenseReport("Lunch at TreyResearch", RejectedStatus, new Guid("BF8F48E1-714A-4501-8D96-C3C677156BF7"), 2));
        }

        private ExpenseReport CreateNewExpenseReport(string title, byte status, Guid userId, int expensesCount)
        {
            var details = new List<ExpenseReportDetail>();

            for (int i = 0; i < expensesCount; i++)
            {
                var amount = (random.NextDouble() + 0.2) * (random.Next(1000) + 1);
                details.Add(new ExpenseReportDetail()
                {
                    Description = "Sample Expense " + i,
                    TransactionAmount = (decimal)Math.Ceiling(amount),
                    BilledAmount = (decimal)amount,
                    AccountNumber = (random.Next(100000000) + 1000000000).ToString(),
                    Date = DateTime.Now.Subtract(new TimeSpan((long)(random.Next(100) * 100000)))
                });
            }

            var report = new ExpenseReport() { Name = title, Purpose = "This should be the purpose of " + title, Created = DateTime.Now, StatusId = status, UserId = userId, Details = details };
            if (status != 1)
            {
                report.Submitted = DateTime.Now;
            }

            return report;
        }

 
    }
}