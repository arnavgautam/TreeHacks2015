using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClipMeme.Startup))]

namespace ClipMeme
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
