using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace WAFC.Reports
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<WAFC.Reports.Models.MISDbContext>(null);
            //Database.SetInitializer<WAFC.Reports.Models.ApplicationDbContext>(null);
            AreaRegistration.RegisterAllAreas();
            //UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //CultureInfo cInfo = new CultureInfo("en-IN");
            //cInfo.DateTimeFormat.ShortDatePattern = "MMMM d, yyyy";
            //cInfo.DateTimeFormat.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture = cInfo;
            //Thread.CurrentThread.CurrentUICulture = cInfo;
        }

    }
}
