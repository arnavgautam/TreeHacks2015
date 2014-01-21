namespace AutomatedHelloWorld
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
