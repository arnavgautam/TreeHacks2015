using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ChannelController : ApiController
    {
        private CustomerContext db = new CustomerContext();

        //
        // POST: /Channel/Create
        public Channel Create(Channel channel)
        {
            Channel ch = null;

            if (ModelState.IsValid)
            {
                ch = db.Channels.Find(channel.Id);

                if (ch == null)
                {
                    db.Channels.Add(channel);
                    db.SaveChanges();
                    return channel;
                }
            }

            return ch;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}