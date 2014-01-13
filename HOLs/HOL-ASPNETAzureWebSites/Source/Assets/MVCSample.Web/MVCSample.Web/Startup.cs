using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCSample.Web.Startup))]
namespace MVCSample.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
