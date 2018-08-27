using System.Web;
using System.Web.Optimization;

namespace ProfileSelect
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
              "~/Scripts/bootstrap-datepicker.js",
              "~/Scripts/bootstrap-datetimepicker.min.js",
              "~/Scripts/locales/bootstrap-datepicker.ru.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment-with-locales.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js",
                "~/Scripts/DataTables/dataTables.responsive.js",
                "~/Scripts/DataTables/responsive.bootstrap.js",
                "~/Scripts/DataTables/dataTables.rus.js"));

            bundles.Add(new ScriptBundle("~/bundles/selectize").Include(
                "~/Content/Selectize/js/standalone/selectize.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                "~/Scripts/toastr.js"));

            bundles.Add(new StyleBundle("~/styles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.theme.css",
                      "~/Content/toastr.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/styles/datepicker").Include(
               "~/Content/bootstrap-datetimepicker.css")
               .Include("~/Content/bootstrap-datepicker.css", new CssRewriteUrlTransform())
               .Include("~/Content/bootstrap-datepicker.standalone.css", new CssRewriteUrlTransform())
               .Include("~/Content/bootstrap-datepicker3.css", new CssRewriteUrlTransform())
               .Include("~/Content/bootstrap-datepicker3.standalone.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/styles/datatables")
                .Include("~/Content/DataTables/css/dataTables.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/responsive.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/rowGroup.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/rowReorder.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/scroller.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/DataTables/css/select.bootstrap.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/styles/selectize").Include(
                "~/Content/Selectize/css/selectize.css",
                "~/Content/Selectize/css/selectize.bootstrap3.css"));
            bundles.Add(new StyleBundle("~/styles/font-awesome").Include(
                "~/Content/font-awesome.css"));
        }
    }
}
