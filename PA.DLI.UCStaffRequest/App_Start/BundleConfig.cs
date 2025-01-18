using System;
using System.Web;
using System.Web.Optimization;

namespace PA.DLI.UCStaffRequest
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //if (bundles == null)
            //{
            //    throw new ArgumentNullException(nameof(bundles));
            //}
            //bundles.UseCdn = true;
            //BundleTable.EnableOptimizations = true;
            //var jquery = new ScriptBundle("~/bundles/jquery", "//ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js");
            //var jqueryval = new ScriptBundle("~/bundles/jqueryval", "//ajax.aspnetcdn.com/ajax/jquery.validate/1.11.1/jquery.validate.min.js");
            //var JqueryUi = new ScriptBundle("~/bundles/JqueryUi", "//code.jquery.com/ui/1.12.1/jquery-ui.min.js");

            //var bootstrappath = new ScriptBundle("~/bundles/bootstrap", "//cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.js");
            //var popper = new ScriptBundle("~/bundles/popper", "//cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js");
            //var modernizr = new ScriptBundle("~/bundles/modernizr", "//ajax.aspnetcdn.com/ajax/modernizr/modernizr-2.6.2.js");
            //bundles.Add(jquery);
            //bundles.Add(bootstrappath);
            //bundles.Add(jqueryval);
            //bundles.Add(popper);
            //bundles.Add(modernizr);
            //bundles.Add(JqueryUi);

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                      "~/Scripts/jquery-ui-{version}.js"));
            //bundles.Add(new ScriptBundle("~/bundles/popper").Include(
            //            "//cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

        }
    }
}
