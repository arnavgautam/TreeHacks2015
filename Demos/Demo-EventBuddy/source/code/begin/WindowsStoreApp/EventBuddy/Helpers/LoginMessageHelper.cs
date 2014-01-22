using EventBuddy.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBuddy.Helpers
{
    public static class LoginMessageHelper
    {
        public static string GetLoginMessage()
        {
            if (string.IsNullOrEmpty(User.Current.UserId))
            {
                return string.Empty;
            }
            else
            {
                var provider = User.Current.UserId.Split(':')[0];

                if (provider.Equals("Twitter", StringComparison.OrdinalIgnoreCase))
                {
                    return "Logged in with Twitter";
                }
                else if (provider.Equals("Facebook", StringComparison.OrdinalIgnoreCase))
                {
                    return "Logged in with Facebook";
                }
                else
                {
                    return "Logged as " + User.Current.UserId;
                }
            }
        }
    }
}
