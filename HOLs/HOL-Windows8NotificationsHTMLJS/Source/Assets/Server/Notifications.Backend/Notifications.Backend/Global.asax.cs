namespace Notifications.Backend
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Notifications.Backend.Controllers;
    using Notifications.Backend.Infrastructure;
    using Notifications.Backend.Infrastructure.Helpers;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool securityInitialized = false;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = (string)null },
                new { controller = new ListConstraint(ListConstraintType.Exclude, "AuthenticationService", "WNSNotificationService") });
        }

        protected void Application_Start()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(CloudConfigurationManager.GetSetting(configName));
            });

            var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            CloudStorageInitializer.InitializeCloudStorage(account);
            this.UploadTileImagesFromResources(account.CreateCloudBlobClient());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (!securityInitialized)
            {
                CloudStorageInitializer.InitializeCloudStorage(CloudStorageAccount.FromConfigurationSetting("DataConnectionString")); // when self signed cert installed.
                InitializeSecurity();
                securityInitialized = true;
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        private static void InitializeSecurity()
        {
            var adminUser = Membership.FindUsersByName("admin").Cast<MembershipUser>().FirstOrDefault() ??
                            Membership.CreateUser("admin", "Passw0rd!", "admin@contoso.com");

            var adminUserId = adminUser.ProviderUserKey.ToString();
            IUserPrivilegesRepository userPrivilegesRepository = new PrivilegesTableServiceContext();
            userPrivilegesRepository.AddPrivilegeToUser(adminUserId, PrivilegeConstants.AdminPrivilege);
        }

        private static bool IsAllowedContent(string path)
        {
            return path.EndsWith("/Error", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/Content", StringComparison.OrdinalIgnoreCase)
                || path.StartsWith("/Scripts", StringComparison.OrdinalIgnoreCase);
        }

        private void UploadTileImagesFromResources(CloudBlobClient cloudBlobClient)
        {
            this.UploadTileImage(cloudBlobClient, "Notifications.Backend.Resources.WindowsAzureLogo.png");
            this.UploadTileImage(cloudBlobClient, "Notifications.Backend.Resources.WindowsAzureLogoWide.png");
        }

        private void UploadTileImage(CloudBlobClient cloudBlobClient, string imageName)
        {
            var tileImagesContainerName = CloudConfigurationManager.GetSetting("TileImagesContainer");

            var container = cloudBlobClient.GetContainerReference(tileImagesContainerName);
            container.CreateIfNotExist();
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

            var assembly = Assembly.GetExecutingAssembly();
            var imageStream = assembly.GetManifestResourceStream(imageName);

            var blob = container.GetBlobReference(imageName.Replace("Notifications.Backend.Resources.", string.Empty));
            blob.Properties.ContentType = "image/png";
            blob.UploadFromStream(imageStream);
        }
    }
}