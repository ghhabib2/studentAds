using System.Web;
using System.Web.Optimization;

namespace Classified.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Scripts/bootbox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Admin JavaScript Labs


            //Chart Script - Should be rendered separately 
            bundles.Add(new ScriptBundle("~/bundles/AdminJavaChartScript").Include(
                "~/Scripts/AdminScripts/chart-master/Chart.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/AdminJavaScriptLabs").Include(
                "~/Scripts/AdminScripts/jquery.js",
                "~/Scripts/AdminScripts/jquery-1.8.3.min.js",
                "~/Scripts/AdminScripts/bootstrap.min.js",
                "~/Scripts/AdminScripts/jquery.dcjqaccordion.2.7.js",
                "~/Scripts/AdminScripts/jquery.dcjqaccordion.2.7.js",
                "~/Scripts/AdminScripts/jquery.scrollTo.min.js",
                "~/Scripts/AdminScripts/jquery.sparkline.js",
                "~/Scripts/AdminScripts/common-scripts.js",
                "~/Scripts/AdminScripts/gritter/js/jquery.gritter.js",
                "~/Scripts/AdminScripts/gritter-conf.js",
                "~/Scripts/AdminScripts/sparkline-chart.js",
                "~/Scripts/AdminScripts/zabuto_calendar.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Scripts/bootbox.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/DataTable").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"
            ));


            //Admin Style Css Contnet
            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                "~/Content/AdminStyle/assets/css/bootstrap.css",
                "~/Content/AdminStyle/assets/font-awesome/css/font-awesome.css",
                "~/Content/AdminStyle/assets/css/zabuto_calendar.css",
                "~/Content/AdminStyle/assets/lineicons/style.css",
                "~/Content/AdminStyle/assets/css/style.css",
                "~/Content/AdminStyle/assets/css/style-responsive.css",
                "~/Content/DataTables/css/dataTables.bootstrap.css",
                "~/Content/themes/base/all.css"
            ));


            #region Client 1 Styles and Scripts

            //Admin Style Css Contnet
            bundles.Add(new StyleBundle("~/Content/client1Style").Include(
                "~/Content/Client1/plugins/bootstrap/dist/css/bootstrap.min.css",
                "~/Content/Client1/plugins/font-awesome/css/font-awesome.min.css",
                "~/Content/Client1/plugins/slick-carousel/slick/slick.css",
                "~/Content/Client1/plugins/slick-carousel/slick/slick-theme.css",
                "~/Content/Client1/plugins/fancybox/jquery.fancybox.pack.css",
                "~/Content/Client1/plugins/jquery-nice-select/css/nice-select.css",
                "~/Content/Client1/plugins/seiyria-bootstrap-slider/dist/css/bootstrap-slider.min.css",
                "~/Content/Client1/plugins/hierarchy-select-2/dist/hierarchy-select.min.css",
                "~/Content/themes/base/all.css",
                "~/Content/Client1/css/style.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/client1Scripts").Include(
                "~/Scripts/Client1Scripts/plugins/jquery/jquery.min.js",
                "~/Scripts/Client1Scripts/plugins/jquery-ui/jquery-ui.min.js",
                "~/Scripts/Client1Scripts/plugins/tether/js/tether.min.js",
                "~/Scripts/Client1Scripts/plugins/raty/jquery.raty-fa.js",
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/Client1Scripts/plugins/seiyria-bootstrap-slider/dist/bootstrap-slider.min.js",
                "~/Scripts/Client1Scripts/plugins/slick-carousel/slick/slick.min.js",
                "~/Scripts/Client1Scripts/plugins/jquery-nice-select/js/jquery.nice-select.min.js",
                "~/Scripts/Client1Scripts/plugins/fancybox/jquery.fancybox.pack.js",
                "~/Scripts/Client1Scripts/plugins/smoothscroll/SmoothScroll.min.js",
                "~/Scripts/Client1Scripts/plugins/hierarchy-select-2/dist/hierarchy-select.min.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/Client1Scripts/js/scripts.js",
                "~/Scripts/jquery-ui-1.12.1.min.js"
                
            ));

            

            bundles.Add(new StyleBundle("~/bundles/summernotecss").Include(
                "~/Content/summernote/summernote-lite.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/summernote").Include(
                "~/Scripts/summernote/summernote-lite.js"
            ));

            #endregion


        }
    }
}
