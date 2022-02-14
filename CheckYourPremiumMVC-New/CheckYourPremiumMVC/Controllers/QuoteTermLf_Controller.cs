using CheckYourPremiumMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
namespace CheckYourPremiumMVC.Controllers
{
    public class QuoteTermLf_Controller : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["MyString"].ConnectionString;
        //
        // GET: /QuoteTermLf_/
        public ActionResult Index()
        {

            return RedirectToAction("QuoteTermLife");
        }
        public ActionResult QuoteTermLife(QuatoTermLifeModel DataQuate)
        {
            DataQuate.Name = Session["Name"].ToString();
            DataQuate.Gender = Session["Gender"].ToString();
            DataQuate.Age = Session["Age"].ToString();
            DataQuate.Smoke = Session["Tabocco"].ToString();
            DataQuate.CustID = Session["CustId"].ToString();
            DataQuate.email = Session["Email"].ToString();
            DataQuate.phone = Session["Phone"].ToString();
            GetCompany(DataQuate);
            ViewBag.Base64String = DataQuate.Logo;
            ViewBag.Premium = DataQuate.Premium;
            ViewBag.PlanBenifit = DataQuate.PlanBenifit;
            ViewBag.PlanPDf = DataQuate.PlanPDF;
            ViewBag.Plan = DataQuate.Plan;
            //  string serializepostdata = JsonConvert.SerializeObject(DataQuate);

            return View();
        }
        //public ActionResult QuoteCompany(TermLifeResponce User)
        //{
        //    string comp = User.Company;
        //    return View();
        //}
        public static void GetCompany(QuatoTermLifeModel DataQuate)
        {

            string constr = ConfigurationManager.ConnectionStrings["MyString"].ConnectionString;
            SqlConnection connection = new SqlConnection(constr);

            SqlCommand command = new SqlCommand("Select * from Companies where CompanyName='­­Edelweiss Tokio Life Insurance'", connection);
            command.CommandType = System.Data.CommandType.Text;
            connection.Open();
            DataTable dt = new DataTable();

            dt.Load(command.ExecuteReader());
            if (dt.Rows.Count > 0)
            {

                DataQuate.Logo = dt.Rows[0][2].ToString();
                //............................Get Premium
                SqlCommand commandd = new SqlCommand("select * from tbl_GetPremiumResponce inner join tbl_GetPremiumData on tbl_GetPremiumData.CustID= tbl_GetPremiumResponce.CustID where tbl_GetPremiumResponce.CustID='" + DataQuate.CustID + "'", connection);
                commandd.CommandType = System.Data.CommandType.Text;
                //connection.Open();
                DataTable dtt = new DataTable();

                dtt.Load(commandd.ExecuteReader());
                if (dtt.Rows.Count > 0)
                {

                    DataQuate.Premium = dtt.Rows[0][12].ToString();
                    //...................End Premium..........................
                    //............Start For Plan

                    DataQuate.Plan = dtt.Rows[0][14].ToString();
                    SqlCommand cm = new SqlCommand("select * from Plan_Benifit where PlanType='" + DataQuate.Plan + "'", connection);
                    cm.CommandType = System.Data.CommandType.Text;
                    //connection.Open();
                    DataTable dtab = new DataTable();

                    dtab.Load(cm.ExecuteReader());
                    if (dtab.Rows.Count > 0)
                    {
                        DataQuate.PlanBenifit = dtab.Rows[0][2].ToString();
                        DataQuate.PlanPDF = dtab.Rows[0][3].ToString();
                    }
                    //.............End Plan..............

                }

            }

        }

        public void GetPlanBenifit()
        {

        }
      
    }
}