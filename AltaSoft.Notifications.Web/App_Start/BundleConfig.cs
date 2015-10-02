using System.Web;
using System.Web.Optimization;

namespace AltaSoft.Notifications.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/select2.js",
                      "~/Scripts/respond.js",
                      //"~/Scripts/tinymce/tinymce.js",
                      //"~/Scripts/ckeditor/ckeditor.js",
                      "~/Scripts/altasoft.common.js"
                      ));

            bundles.Add(new StyleBundle("~/styles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/select2.css",
                      "~/Content/select2-bootstrap.css",
                      "~/Content/site.css"
                      ));
        }
    }
}
