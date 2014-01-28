using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;

namespace CloudShop.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];

            if (!this.IsPostBack)
            {
                var list = (RadioButtonList)this.RegisterUser.WizardSteps[0].FindControl("roles");

                list.DataSource = System.Web.Security.Roles.GetAllRoles().OrderByDescending(a => a);
                list.DataBind();

                if (list.Items.Count > 0)
                {
                    list.Items[0].Selected = true;
                }
            }
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            var list = (RadioButtonList)this.RegisterUser.WizardSteps[0].FindControl("roles");

            System.Web.Security.Roles.AddUserToRole(
                                       this.RegisterUser.UserName,
                                       list.SelectedItem.Text);
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }
    }
}