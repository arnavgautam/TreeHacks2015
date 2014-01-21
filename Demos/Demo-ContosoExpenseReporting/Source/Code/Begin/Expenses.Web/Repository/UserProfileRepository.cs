namespace Expenses.Web.Repository
{
    using System.Data;
    using System.Linq;
    using Expenses.Web.Models;

    public class UserProfileRepository : IUserProfileRepository
    {
        public UserProfile GetProfile(string username)
        {
            using (var ctx = this.GetContext())
            {
                return ctx.UserProfiles.SingleOrDefault(u => u.UserName.Equals(username));
            }
        }

        public UserProfile Save(UserProfile profile)
        {
            using (var ctx = this.GetContext())
            {
                ctx.Entry(profile).State = this.GetProfile(profile.UserName) != null ? EntityState.Modified : EntityState.Added;
                ctx.SaveChanges();
                return profile;
            }            
        }

        private ReportsContext GetContext()
        {
            return new ReportsContext();
        }
    }
}