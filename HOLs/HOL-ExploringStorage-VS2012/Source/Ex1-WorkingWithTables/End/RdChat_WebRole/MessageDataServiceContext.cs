using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace RdChat_WebRole
{
    public class MessageDataServiceContext
        : TableServiceContext
    {
        public MessageDataServiceContext(string baseAddress, StorageCredentials credentials)
            : base(baseAddress, credentials)
        {
        }

        public IQueryable<Message> Messages
        {
            get
            {
                return this.CreateQuery<Message>("Messages");
            }
        }

        public void AddMessage(string name, string body)
        {
            this.AddObject("Messages", new Message { Name = name, Body = body });
            this.SaveChanges();
        }
    }
}
