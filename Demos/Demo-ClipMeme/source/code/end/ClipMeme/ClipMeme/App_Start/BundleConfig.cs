namespace ClipMeme
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/vendors/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/vendors/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/vendors/bootstrap.js",
                      "~/Scripts/vendors/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/vendors/jquery.signalR-2.0.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/vendors/angular.min.js",
                      "~/Scripts/vendors/ui-bootstrap-tpls-0.10.0.js",
                      "~/Scripts/vendors/angular-file-upload-shim.min.js",
                      "~/Scripts/vendors/angular-resource.js",
                      "~/Scripts/vendors/angular-file-upload.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-app-controllers").IncludeDirectory(
                      "~/Scripts/app/controllers", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-app-directives").IncludeDirectory(
                      "~/Scripts/app/directives", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/angular-app-services").IncludeDirectory(
                      "~/Scripts/app/services", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/bootstrap.min.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/fonts/fonts.css",
                      "~/Content/Site.css"));
        }
    }
}
