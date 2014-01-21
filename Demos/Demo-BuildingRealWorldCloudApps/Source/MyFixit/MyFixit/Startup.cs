using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(MyFixIt.Startup))]

namespace MyFixIt
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}
