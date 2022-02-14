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
    public class HealthInsuranceController : Controller

    {
        //
        // GET: /HealthInsurance/
        static string conn =ConfigurationManager.ConnectionStrings["MyString"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }
        //...................Bind City...........................
        public JsonResult City()
        {
            BindCityData CityDb = new BindCityData();
            DataSet ds = CityDb.Get_City();
            List<City_Cls> searchlist = new List<City_Cls>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                searchlist.Add(new City_Cls
                {
                    State_ID = Convert.ToInt32(dr["State_ID"].ToString()),
                    City_Name = dr["City_Name"].ToString()
                });
            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);
        }
       
        //.....................End City....................
      
        public ActionResult HealthInsurance(GetDataValue Data)
        {
           
        return View();
        }


        //.................................Insert data.............................

         [HttpPost]
         public ActionResult InsertDataIp(GetDataValue Data)
         {
              var result = Saved_Record(Data);
             Session.Timeout = 36000;
             Session["HealthName"] = Data.Full_Name;
             Session["HealthGender"] = Data.Gender;
             Session["HealthAge"] = Data.SAge;
             Session["HealthSelf"] = Data.Self;
             Session["HealthAdult"] = Data.Spouses;
             Session["HealthChildren"] = Data.Sona;
             Session["HealthBalnce"] = "100000.00";
             return Json(result, JsonRequestBehavior.AllowGet);
         }
         public static bool Saved_Record(GetDataValue Data)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyString"].ConnectionString);
            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand("Health_Insurence_Store", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Action", Data.Action);
                cmd.Parameters.AddWithValue("@Gender ", Data.Gender);
                cmd.Parameters.AddWithValue("@Name", Data.Full_Name);
                cmd.Parameters.AddWithValue("@MobileNo", Data.MobileNo);
                cmd.Parameters.AddWithValue("@City ", Data.City);
                cmd.Parameters.AddWithValue("@Income", Data.income);
                cmd.Parameters.AddWithValue("@Self", Data.Self);
                cmd.Parameters.AddWithValue("@Child", Data.Sona);
                cmd.Parameters.AddWithValue("@Adult", Data.Spouses);
                cmd.Parameters.AddWithValue("@Self_Age  ", Data.SAge);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString());
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
               

            }

            catch (Exception ex)
            {
                return true;
               
            }


        }

	}
}