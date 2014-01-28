using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using PhotoAlbum;
using System.Web.Configuration;

namespace PhotoAlbum
{
    public class Global : HttpApplication
    {
        private static string imageStorePath;

        public static string ImageStorePath
        {
            get
            {
                return imageStorePath;
            }

            set
            {
                imageStorePath = value;
            }
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();

            if (imageStorePath == null)
            {
                ImageStorePath = WebConfigurationManager.AppSettings["ImageStorePath"];
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
