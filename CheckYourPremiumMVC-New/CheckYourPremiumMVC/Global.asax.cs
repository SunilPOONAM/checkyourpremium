using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CheckYourPremiumMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
        }
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class CustomHandleErrorAttribute : HandleErrorAttribute
        {
            public override void OnException(ExceptionContext filterContext)
            {
            }
        }
      
        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            if (exception is HttpException)
            {
                var httpException = (HttpException)exception;
                Response.StatusCode = httpException.GetHttpCode();
            }
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-powered-by");
            HttpContext.Current.Response.Headers.Remove("x-powered-by");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Remove("Server");
            HttpContext.Current.Response.Headers.Remove("cookie");
            HttpContext.Current.Response.Headers.Remove("Cookie");
            HttpContext.Current.Response.Headers.Remove("cache-control");
            HttpContext.Current.Response.Headers.Remove("content-length");
            HttpContext.Current.Response.Headers.Remove("x-frame-options");
            HttpContext.Current.Response.Headers.Remove("x-powered-by-plesk");
            HttpContext.Current.Response.Headers.Remove(":authority");
            HttpContext.Current.Response.Headers.Remove(":path");
            HttpContext.Current.Response.Headers.Remove(":scheme");
            HttpContext.Current.Response.Headers.Remove("accept");
            HttpContext.Current.Response.Headers.Remove("accept-encoding");
            HttpContext.Current.Response.Headers.Remove("accept-language");
            HttpContext.Current.Response.Headers.Remove("referer");
            HttpContext.Current.Response.Headers.Remove("user-agent");
            HttpContext.Current.Response.Headers.Remove("x-requested-with");
            HttpContext.Current.Response.Headers.Remove("sec-fetch-site");
            HttpContext.Current.Response.Headers.Remove("sec-fetch-mode");
            HttpContext.Current.Response.Headers.Remove("sec-fetch-dest");
            HttpContext.Current.Response.Headers.Remove("X-CSRFToken");
            
        }
       
      
    }
}
