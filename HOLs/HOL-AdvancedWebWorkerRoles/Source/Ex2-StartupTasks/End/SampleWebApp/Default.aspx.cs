using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SampleWebApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void ShowGreeting_Click(object sender, EventArgs e)
        {
            LegacyCOMLib.Helper helper = new LegacyCOMLib.Helper();
            this.Message.Text = helper.Greeting(this.Username.Text);
        }
    }
}
