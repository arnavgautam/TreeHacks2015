using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudShop
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void OnLoggedOut(object sender, EventArgs e)
        {
            this.Session.Clear();
        }
    }
}
