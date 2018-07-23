using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutenticacionPersonalizada.App_Start;
using VillasBonaterra.App_Start;
using VillasBonaterra.Seguridad;

namespace VillasBonaterra
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register WEB API
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e) {
            if (Request.IsAuthenticated) {
                var identity = new IdentityPersonalizado(HttpContext.Current.User.Identity);
                var principal = new PrincipalPersonalizado(identity);
                HttpContext.Current.User = principal;
            }
        }
    }
}
