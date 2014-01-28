using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdChat_WebRole
{
    public class Message : Microsoft.WindowsAzure.StorageClient.TableServiceEntity
    {
        public string Name { get; set; }

        public string Body { get; set; }

        public Message()
        {
            PartitionKey = "a";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }
    }
}