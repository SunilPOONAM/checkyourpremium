using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CheckYourPremiumMVC.Controllers
{
    public class CustomerLoginController : Controller
    {
        //
        // GET: /CustomerLogin/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult customerLogin()
        {
            return View();
        }
        public ActionResult MobileVerification()
        {
            return View();
        }
        public JsonResult SendOTP(string mobile)
        {
            int otpValue = new Random().Next(100000, 999999);
            var status = "";

            //.....................send Sms
            Session["MobileNumber"] = mobile;
            string msg ="Dear Customer "+otpValue+" is your OTP to login. This OTP is valid for next 20 minutes.Best Wishes, Check Your Premium.";
            string phone = mobile;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/vb/apikey.php?apikey=71e6646b9e66e6941b61&senderid=CYPIWA&route=3&number=" + phone + "&message=" + msg);
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/http-api.php?username=nitink&password=bulksms@123&senderid=CYPIWA& route=1&number=" + phone + "&message=" + msg + "");

            String responseString = String.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream objStream = response.GetResponseStream())
                    {
                        using (StreamReader objReader = new StreamReader(objStream))
                        {
                            responseString = objReader.ReadToEnd();
                            objReader.Close();
                        }
                        objStream.Flush();
                        objStream.Close();
                    }
                    //lblErrormsg.Text = "Msg Sent";
                    response.Close();
                    Session["CurrentOTP"] = otpValue;
                }
            }
            catch (Exception e)
            {

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult OtpVerification()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OtpVerification(string otp)
        {
            bool result = false;
            //Session.Timeout = 60;  
            string sessionOTP = Session["CurrentOTP"].ToString();

            if (otp == sessionOTP)
            {
                result = true;

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}