using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoUploader_WebRole.Startup))]
namespace PhotoUploader_WebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
