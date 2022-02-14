using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckYourPremiumMVC.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }
        //..................Url Encrypt.......................
        public string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }


        //..................................................
        public ActionResult DashBoard(string name)
        {
            try
            {
                //var decodename = Decode(name);
                //if (decodename != "")
                //{
                //    if (Session["EmailID"] != null)
                //    {
                //        if (Session["EmailID"] != "")
                //        {
                            return View();
                //        }
                //    }
                //}
                //else
                //{

                //    return RedirectToAction("LoginDetails", "RegistrationLogin");
                //}
                //return RedirectToAction("LoginDetails", "RegistrationLogin");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("LoginDetails", "RegistrationLogin");
        }
    }
}