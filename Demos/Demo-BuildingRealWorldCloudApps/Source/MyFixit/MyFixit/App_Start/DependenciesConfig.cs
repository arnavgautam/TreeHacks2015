namespace MyFixIt.App_Start
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using MyFixIt.Logging;
    using MyFixIt.Persistence;

    public class DependenciesConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterType<FixItTaskRepository>().As<IFixItTaskRepository>();
            builder.RegisterType<PhotoService>().As<IPhotoService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}