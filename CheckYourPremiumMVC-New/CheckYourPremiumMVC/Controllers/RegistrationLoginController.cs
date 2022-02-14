using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Business;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;


namespace CheckYourPremiumMVC.Controllers
{
    public class RegistrationLoginController : Controller
    {


        public RegistrationBusiness IRegistrationBusiness;
        public RegistrationLoginController()
        {
            IRegistrationBusiness = new RegistrationBusiness();

        }

        // GET: RegistrationLogin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginDetails()
        {
           
            return View();
        }
        //...........................
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
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
        //................................for captch
        [AllowAnonymous]

        public ActionResult Captcha()
        {

            Bitmap objBMP = new System.Drawing.Bitmap(60, 30);

            Graphics objGraphics = System.Drawing.Graphics.FromImage(objBMP);

            objGraphics.Clear(Color.DimGray);

            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //' Configure font to use for text

            Font objFont = new Font("Calibri", 14, FontStyle.Bold);

            string randomStr = "";

            int[] myIntArray = new int[5];

            int x;

            //That is to create the random # and add it to our string

            Random autoRand = new Random();

            for (x = 0; x < 5; x++)
            {

                myIntArray[x] = System.Convert.ToInt32(autoRand.Next(0, 9));

                randomStr += (myIntArray[x].ToString());

            }

            //This is to add the string to session cookie, to be compared later

            Session.Add("randomStr", randomStr);

            //' Write out the text

            objGraphics.DrawString(randomStr, objFont, Brushes.White, 4, 4);

            //' Set the content type and return the image

            Response.ContentType = "image/GIF";

            objBMP.Save(Response.OutputStream, ImageFormat.Gif);



            objFont.Dispose();

            objGraphics.Dispose();
            objBMP.Dispose();

            return new EmptyResult();

        }


        //..................................................
         [AllowAnonymous]
         [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginDetails(RegistrationDetails objuserlogin)
        {
            //Poonam ......................................................
            try
            {
                 List<RegistrationDetails> registration = IRegistrationBusiness.GetLoginData(objuserlogin);
                List<RegistrationDetails> registcount = IRegistrationBusiness.GetregData(objuserlogin);
              //  RegistrationDetails regCapcha = new RegistrationDetails();
                string c = Session["randomStr"].ToString();
                if (objuserlogin.SecurityCode == Session["randomStr"].ToString())
                {
                    if (registcount[0].attempttime >= 5)
                    {
                        LoginDetails data = new LoginDetails();
                        data.EmailId = objuserlogin.EmailId;
                        var tid = IRegistrationBusiness.attamttimeupdate(data);
                        return RedirectToAction("redirect", "RegistrationLogin");

                    }
                    else
                    {
                        if (registration.Count == 1)
                        {
                            LoginDetails data = new LoginDetails();
                            data.EmailId = objuserlogin.EmailId;
                            var tid = IRegistrationBusiness.attamttimeupdate(data);
                            Session["UserName"] = registration[0].FirstName;
                            Session["EmailID"] = objuserlogin.EmailId;
                            Session["AdminPassword"] = objuserlogin.Password;
                            if (Session["EmailID"].ToString() == "Admin001@gmail.com" && Session["AdminPassword"].ToString() != null)
                            {
                                if (Session["EmailID"].ToString() == "Admin001@gmail.com" && Session["AdminPassword"].ToString() != "")
                                {
                                    Session["EmailID"] = objuserlogin.EmailId;
                                    Session["HomePassword"] = objuserlogin.Password;
                                    Session.Timeout = 36000;
                                    string name = Session["UserName"].ToString();
                                    Session["UserName"] = Encode(name);
                                    return RedirectToAction("DashBoard", "AdminHome", new { @Name = Session["UserName"].ToString() });
                                }
                            }
                            else if (Request.Cookies["page"] != null)
                            {
                                string page = Request.Cookies["page"].Value.ToString();

                                if (page == null)
                                {
                                    return RedirectToAction("Home", "Home");
                                }

                                else if (page == "GetDetail")
                                {
                                    Session["IdNum22"] = Session["IdNum2"].ToString();
                                    return RedirectToAction("GetDetail", "TravelInsurance");
                                }

                                else if (page == "GetHealthQDetais")
                                {
                                    Session["IdNum23"] = Session["IdNum3"].ToString();
                                    return RedirectToAction("GetHealthQDetais", "HealthPlan");
                                }

                                else if (page == "GetDetails")
                                {
                                    Session["IdNum2"] = Session["IdNum"].ToString();
                                    return RedirectToAction("GetDetails", "TermAndLife");
                                }
                                else if (page == "GetDetailGodigit")
                                    return RedirectToAction("GetDetailGodigit", "TravelInsurance");
                                else
                                    return RedirectToAction("Home", "Home");
                            }

                            return RedirectToAction("Home", "Home");
                        }

                        else
                        {
                            LoginDetails data = new LoginDetails();
                            data.EmailId = objuserlogin.EmailId;
                            var tid = IRegistrationBusiness.attamttime(data);
                            //objuserlogin.attempttime
                            if (data.attempttime <= 5)
                            {
                                //Response.Write("<script>window.setTimeout(function () { window.location.href ='Your_Desire_Link_To_Redirect'; }, 5000); </script>");
                               ViewBag.Accepted=" Invalid UserId And Password";
                            }
                            else if (data.attempttime == 0)
                            {
                                 ViewBag.Accepted="Too many Failed login attempts. Please try again 30 seconds";
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.Accepted = "Invalid Captcha";
                   // Response.Write("<script>alert('Invalid Captcha')</script>");
                }

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        //public ActionResult  LoginDetails(RegistrationDetails objuserlogin)

        // {
        //     //Poonam ......................................................

        //  List<RegistrationDetails> registration = IRegistrationBusiness.GetLoginData(objuserlogin);
        //   if(objuserlogin.EmailId=="Admin001@gmail.com"&&objuserlogin.Password=="AdminUser")
        //   {
        //       Session["EmailID"] = objuserlogin.EmailId;
        //       Session["HomePassword"] = objuserlogin.Password;
        //       Session.Timeout = 36000;
        //       return RedirectToAction("DashBoard", "AdminHome");
        //   }
        //   else if (registration.Count == 1)
        //    {
        //        Session["UserName"] = objuserlogin.EmailId;
        //        Session["EmailID"] = objuserlogin.EmailId;
        //        if (Request.Cookies["page"] != null)
        //        {
        //            string page = Request.Cookies["page"].Value.ToString();

        //            if (page==null)
        //            {
        //                return RedirectToAction("Home", "Home");
        //            }

        //            else if (page == "GetDetail")
        //            {
        //                Session["IdNum22"] = Session["IdNum2"].ToString();
        //                return RedirectToAction("GetDetail", "TravelInsurance");
        //            }

        //            else if (page == "GetHealthQDetais")
        //            {
        //                Session["IdNum23"] = Session["IdNum3"].ToString();
        //                return RedirectToAction("GetHealthQDetais", "HealthPlan");
        //            }

        //            else if (page == "GetDetails")
        //            {
        //                Session["IdNum2"] = Session["IdNum"].ToString();
        //                return RedirectToAction("GetDetails", "TermAndLife");
        //            }
        //            else if (page == "GetDetailGodigit")
        //                return RedirectToAction("GetDetailGodigit", "TravelInsurance");
        //            else
        //                return RedirectToAction("Home", "Home");
        //        }
        //        return RedirectToAction("Home", "Home");
        //    }
        //    else
        //    {
        //        Response.Write("<script>alert(' Invalid UserId And Password')</script>");
        //    }

        //    return View();
        //} 
        //14 march
        public ActionResult ChangePassword()
        {
            return View();
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ChangePassword(LoginDetails details)
        {
            try
            {
                if (Session["EmailID"] != null)
                {
                    details.EmailId = Session["EmailID"].ToString();
                    var tid = IRegistrationBusiness.savePassword(details);
                    if (tid != "")
                    {
                    switch (tid)
                    {
                        case "Very Week":
                         //   tid = "Very Week";
                            ViewBag.Accepted = "Password is Very Week";
                            break;
                        case  "Week":
                           // tid = "Week";
                            ViewBag.Accepted = "Password is  Week";

                            break;
                        case "Medium":
                           // tid = "Medium";
                            ViewBag.Accepted = "Password is Medium";

                            break;
                        case  "Strong":
                           // tid = "Strong";
                            ViewBag.Accepted = "Password Update Successfull";
                            break;
                        case "Very Strong":
                          //  tid = "Very Strong";
                            ViewBag.Accepted = "Password Update Successfull";
                            break;
                        default:
                            ViewBag.Accepted = "Password Update Successfull";

                            break;
                    }
                }
                    else
                    {
                        ViewBag.Accepted = "Your old password appears to be incorrect";
                    }
                }
                else
                {
                    ViewBag.Accepted = "Old Password Invalid.." ;
                }
                return View(details);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                //Response.Cookies["page"].Value = "";
                //Session["EmailID"] = null; Session["AdminPassword"] = null;
                //Session["UserName"] = null; Session["UserName"] = "";
                //Session["EmailID"] = ""; Session["AdminPassword"] = "";
                //Response.ClearHeaders();
                //Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
                //Response.AddHeader("Pragma", "no-cache");
                Session.RemoveAll();
                if (Session["EmailID"] != null)
                {
                    Session["UserName"] = null;
                    Session["EmailID"] = null;
                    Response.Cookies["page"].Value = "";
                    Response.Write("<script>alert('Ok')</script>");

                }
                else
                {
                    return RedirectToAction("Home", "Home");
                }


                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
        //14 march


        //  }







        public ActionResult Registration()
        {

            BindGender();
            BindCountry();
            return View();
        }
        public void BindGender()
        {
            var registrations = IRegistrationBusiness.GetGender();
            SelectList SelectList = new SelectList(registrations, "text", "value");
            ViewData["Gender"] = SelectList;
        }
        public void BindCountry()
        {
            var registrations = IRegistrationBusiness.GetCountry();
            SelectList SelectList = new SelectList(registrations, "text", "value");
            ViewData["country"] = SelectList;
        }
        [HttpPost]
        public ActionResult Registration(RegistrationDetails Details)
        {
            try
            {
                BindGender();
                BindCountry();
                try
                {
                    var tid = IRegistrationBusiness.saveRegistration(Details);
                    var Tid = IRegistrationBusiness.saveLogin(Details);
                    //string msg = "Hi%20" + Details.FirstName + " " + Details.LastName + "%20your Registration is Succesfully .Your id %20" + Details.EmailId + "and Your Password%20" + Details.Password;
                    //string phone = Details.MobileNo;

                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/vb/apikey.php?apikey=71e6646b9e66e6941b61&senderid=CYPIWA&route=3&number=" + phone + "&message=" + msg);
                    //// HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/http-api.php?username=nitink&password=bulksms@123&senderid=CYPIWA& route=1&number=" + phone + "&message=" + msg + "");

                    //String responseString = String.Empty;
                    //try
                    //{
                    //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    //    {
                    //        using (Stream objStream = response.GetResponseStream())
                    //        {
                    //            using (StreamReader objReader = new StreamReader(objStream))
                    //            {
                    //                responseString = objReader.ReadToEnd();
                    //                objReader.Close();
                    //            }
                    //            objStream.Flush();
                    //            objStream.Close();
                    //        }
                    //        //lblErrormsg.Text = "Msg Sent";
                    //        response.Close();

                    //    }
                    //}
                    //catch (Exception e)
                    //{

                    //}
                    Response.Write("<script>alert('Registration Successfully..')</script>");
                    return RedirectToAction("LoginDetails", "RegistrationLogin");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Registration Not Completed')</script>");
                }
                //return View(Details);
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        public ActionResult redirect()
        {


            return View();
        }
    }
   
}