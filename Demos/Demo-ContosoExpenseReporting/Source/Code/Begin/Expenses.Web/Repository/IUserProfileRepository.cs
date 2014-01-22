namespace Expenses.Web.Repository
{
    using Expenses.Web.Models;

    public interface IUserProfileRepository
    {
        UserProfile GetProfile(string username);

        UserProfile Save(UserProfile profile);
    }
}