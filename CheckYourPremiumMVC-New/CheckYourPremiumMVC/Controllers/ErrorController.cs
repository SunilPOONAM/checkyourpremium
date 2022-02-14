using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckYourPremiumMVC.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Index()
        {
            return View();
        }
        [HandleError()]
        public ActionResult Errorhanddling()
        {
           try
                {}
          catch(Exception ex)
             {return View("Error");
          }return View();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };

        }
        }
	}
