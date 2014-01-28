using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Services.Client;
using Microsoft.WindowsAzure;

namespace RdChat_WebRole
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Save the text message to the message data context, and bind it to the list control.
        /// </summary>
        /// <param name="sender">Submit button</param>
        /// <param name="e">Button click event</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            var statusMessage = string.Empty;

            try
            {
                var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                var context = new MessageDataServiceContext(account.TableEndpoint.ToString(), account.Credentials);

                context.AddMessage(HttpUtility.HtmlEncode(this.nameBox.Text), HttpUtility.HtmlEncode(this.messageBox.Text));

                this.messageList.DataSource = context.Messages;
                this.messageList.DataBind();
            }
            catch (DataServiceRequestException ex)
            {
                statusMessage = "Unable to connect to the table storage server. Please check that the service is running.<br>"
                                 + ex.Message;
            }

            this.status.Text = statusMessage;
        }
    }
}