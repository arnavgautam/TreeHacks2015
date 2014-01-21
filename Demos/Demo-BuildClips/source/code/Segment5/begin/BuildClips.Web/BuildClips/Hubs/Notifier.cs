using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using BuildClips.Services.Models;

namespace BuildClips.Hubs
{
    public class Notifier : Hub
    {
        public void VideoUpdated(Video video)
        {
            Clients.All.onVideoUpdate(video);
        }
    }
}