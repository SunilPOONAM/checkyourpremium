using CheckYourPremiumMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckYourPremiumMVC.Controllers
{
    public class QuoteHealth_ControllerController : Controller
    {
        //
        // GET: /QuoteHealth_Controller/
        static string conn = ConfigurationManager.ConnectionStrings["MyString"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult quotes_Health(ShowHealthInsurenceQuate SessionData)
        {
            SessionData.Name = Session["HealthName"].ToString();
            SessionData.Gender = Session["HealthGender"].ToString();
            SessionData.AgeFrom = Session["HealthAge"].ToString();
            SessionData.Individual = Session["HealthSelf"].ToString();
            SessionData.Adults=Session["HealthAdult"].ToString();
            SessionData.Childrens=Session["HealthChildren"].ToString();

            SessionData.SelectBlance = Session["HealthBalnce"].ToString();
            ViewBag.HealthName = SessionData.Name;
            ViewBag.HealthGender=SessionData.Gender;
            ViewBag.AgeFrom = SessionData.AgeFrom;
            return View();
        }
        public JsonResult GetAllUser(int EmpId)
        {
            
            List<ShowHealthInsurenceQuate> ChartData = new List<ShowHealthInsurenceQuate>();
            string query = string.Format("Select * From tbl_HealthPremiumChartData inner join Companies on Companies.CompanyName=tbl_HealthPremiumChartData.PremiumDesc where Duration='1 Year' and SumInsured='" + Session["HealthBalnce"] + "' and Individual='" + Session["HealthSelf"] + "' and Adults='" + Session["HealthAdult"] + "' and Childrens='" + Session["HealthChildren"] + "' and AgeFrom<='" + Session["HealthAge"] + "' and AgeTo>='" + Session["HealthAge"] + "'", EmpId);
            SqlConnection connection = new SqlConnection(conn);
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        ChartData.Add(
                            new ShowHealthInsurenceQuate
                            {
                                //EmpId = int.Parse(reader["EmpId"].ToString()),
                                //PremiumDesc = reader.GetValue(0).ToString(),
                                PremiumDesc = reader["PremiumDesc"].ToString(),
                                SumAssured = reader["SumInsured"].ToString(),
                                Premium = reader["Premium"].ToString(),
                                Logo = reader["LogoImage"].ToString(),
                              
                            }
                            //.......................Company get.....
                            
                            //.........................................
                        );
                        
                    }
                }
                return Json(ChartData, JsonRequestBehavior.AllowGet);
            }
        }
        //....................Change Event For Self,Adult and child...............
        public ActionResult Checkdb(ShowHealthInsurenceQuate Data)
        {
            string value=Data.SelectValue;
            if (value == "1000")
            {
                Session["HealthSelf"] = 1;
                Session["HealthAdult"] = 0;
                Session["HealthChildren"] = 0;
            }
            else if (value == "1100")
            {
                Session["HealthSelf"] = 0;
                Session["HealthAdult"] = 2;
                Session["HealthChildren"] = 0;
            }
            else if (value == "1110")
            {
                Session["HealthSelf"] = 0;
                Session["HealthAdult"] = 2;
                Session["HealthChildren"] = 1;
            }
            else if (value == "1120")
            {
                Session["HealthSelf"] = 0;
                Session["HealthAdult"] = 2;
                Session["HealthChildren"] = 2;
            }
            else if (value == "1010")
            {
                Session["HealthSelf"] = 0;
                Session["HealthAdult"] = 1;
                Session["HealthChildren"] = 1;
            }
            else if (value == "1020")
            {
                Session["HealthSelf"] = 0;
                Session["HealthAdult"] = 1;
                Session["HealthChildren"] = 2;
            }
          //  var result = ;
           var result= GetAllUser(2);
           return Json(result, JsonRequestBehavior.AllowGet);
        }
        //...................Change Event According to Blance................
        public ActionResult CheckCover(ShowHealthInsurenceQuate Data2)
        {
            string value = Data2.SelectBlance;
            Session["HealthBalnce"] = value + ".00";
            var result = GetAllUser(2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
    }
}