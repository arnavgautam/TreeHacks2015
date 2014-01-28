using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudShop.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CreateUserWizard1.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];

            if (!this.IsPostBack)
            {
                var list = (RadioButtonList)this.CreateUserWizard1.WizardSteps[0].FindControl("roles");

                list.DataSource = System.Web.Security.Roles.GetAllRoles().OrderByDescending(a => a);
                list.DataBind();

                if (list.Items.Count > 0)
                {
                    list.Items[0].Selected = true;
                }
            }
        }

        protected void OnCreatedUser(object sender, EventArgs e)
        {
            var list = (RadioButtonList)this.CreateUserWizard1.WizardSteps[0].FindControl("roles");

            System.Web.Security.Roles.AddUserToRole(
                                       this.CreateUserWizard1.UserName,
                                       list.SelectedItem.Text);
        }
    }
}
