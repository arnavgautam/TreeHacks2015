using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace RdStorage_WebRole
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // initialize the account information
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            
            // retrieve a reference to the messages queue
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("messagequeue");
            queue.CreateIfNotExist();

            // add the message to the queue
            var msg = new CloudQueueMessage(this.txtMessage.Text);
            queue.AddMessage(msg);
            this.txtMessage.Text = string.Empty;
        }
    }
}