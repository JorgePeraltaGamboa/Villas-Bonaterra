using VillasBonaterra.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AutenticacionPersonalizada.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Assets/css")
                .Include("~/Content/vendor/font-awesome/css/font-awesome.css", new RewritePathTransform(@"/Content/vendor/font-awesome"))
                .Include("~/Content/vendor/bootstrap/css/bootstrap.css",new RewritePathTransform(@"/Content/vendor/bootstrap"))
                .Include("~/Content/vendor/sb-admin/css/sb-admin.css", new CssRewriteUrlTransform())
                .Include("~/Content/vendor/datatables/dataTables.bootstrap4.css", new RewritePathTransform(@"/Content/vendor/datatables")
                ));

            bundles.Add(new StyleBundle("~/Assets/VisitorCSS")
               .Include("~/Content/vendor/datepicker/css/bootstrap-iso.css", new CssRewriteUrlTransform())
               .Include("~/Content/vendor/datepicker/css/font-awesome.min.css", new RewritePathTransform(@"/Content/vendor/datepicker"))
               .Include("~/Content/vendor/datepicker/css/bootstrap-datepicker3.css", new CssRewriteUrlTransform())
               .Include("~/Content/css/VisitorUpdate.css", new CssRewriteUrlTransform())
               );

            bundles.Add(new ScriptBundle("~/Assets/js").Include(
                "~/Content/vendor/jquery/jquery.js",
                "~/Content/vendor/jquery/jquery-ui.js",
                "~/Content/vendor/moment/moment.js",
                "~/Content/vendor/jquery/jquery.validate.js",
                "~/Content/vendor/bootstrap/js/bootstrap.bundle.js",
                "~/Content/vendor/jquery-easing/jquery.easing.js",
                "~/Content/vendor/sb-admin/js/sb-admin.js"
                ));

            bundles.Add(new StyleBundle("~/Assets/LoginCSS")
               .Include("~/Content/css/login.css", new CssRewriteUrlTransform())
               );

            bundles.Add(new ScriptBundle("~/Assets/Login").Include(
                "~/Content/js/Login.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/Home").Include(
                "~/Content/vendor/chart.js/Chart.js",
                "~/Content/vendor/sb-admin/js/sb-admin-charts.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/Access").Include(
                "~/Content/vendor/datatables/jquery.dataTables.js",
                "~/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/Visitor").Include(
                "~/Content/vendor/datatables/jquery.dataTables.js",
                "~/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js",
                "~/Content/vendor/datepicker/js/bootstrap-datepicker.min.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js",
                "~/Content/js/Visitor.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/VisitorAdd").Include(
                "~/Content/vendor/datepicker/js/bootstrap-datepicker.min.js",
                "~/Content/vendor/datepicker/js/locales/bootstrap-datepicker.es.js",
                "~/Content/js/Visitor.js"
                ));

            bundles.Add(new ScriptBundle("~/Assets/Users").Include(
                "~/Content/vendor/datatables/jquery.dataTables.js",
                "~/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js",
                "~/Content/vendor/datepicker/js/bootstrap-datepicker.min.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js",
                "~/Content/js/Users.js"
                ));
            bundles.Add(new ScriptBundle("~/Assets/UsersAdd").Include(
                "~/Content/vendor/datatables/jquery.dataTables.js",
                "~/Content/vendor/datatables/dataTables.bootstrap4.js",
                "~/Content/vendor/sb-admin/js/sb-admin-datatables.js",
                "~/Content/js/Users.js"
                ));
        }
    }
}