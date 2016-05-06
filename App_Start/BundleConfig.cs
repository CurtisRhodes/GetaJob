using System.Web;
using System.Web.Optimization;

namespace GetaJob
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery/jquery-{version}.js",
                "~/Scripts/jquery/jquery-ui.js",
                "~/Scripts/jquery/jquery.datetimepicker.js",
                "~/Scripts/jquery/jquery-print.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
            //    "~/Scripts/knockout/knockout-{version}.js"));



            bundles.Add(new StyleBundle("~/Styles/css").Include(
                      "~/Styles/ezList.css",
                      "~/Styles/bootstrap.css",
                      "~/Styles/layout.css",
                      "~/Styles/site.css"));

            bundles.Add(new StyleBundle("~/Styles/jquery").Include(
                      "~/Styles/jquery/jquery*"));
        }
    }
}
