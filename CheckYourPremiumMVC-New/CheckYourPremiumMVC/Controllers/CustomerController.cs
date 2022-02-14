using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Domain;
using CheckYourPremiumMVC.Models;

namespace CheckYourPremiumMVC.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        //
        public CustomerBusiness ICustomerBusiness;
        public CustomerController()
        {
            ICustomerBusiness = new CustomerBusiness();

        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult HealthDetails()
        {
            if (Session["MobileNumber"] != null)
            {
                ViewBag.transId = Session["MobileNumber"];
            }
            else
            {

                return RedirectToAction("MobileVerification", "CustomerLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult SearchdataHealth(int draw, int start, int length, string searchTxt, string searchString, string FullName)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                HealthPlanDetails objSearchHealthInsurance = new HealthPlanDetails();
                objSearchHealthInsurance.MobileNo = FullName;
                List<HealthPlanDetails> list = ICustomerBusiness.GetSearchListHealth(objSearchHealthInsurance);
                //List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth, string searchString);

                var commonList = list;
                commonList = list;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                }

                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                List<List<string>> newDetailList = new List<List<string>>();
                string action = string.Empty;


                var i = 1;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {

                    if (item.Self == null)
                    {
                        item.Self = "";
                    }
                    if (item.Spouses == null)
                    {
                        item.Spouses = "";
                    } if (item.son == null)
                    {
                        item.son = "";
                    }
                    if (item.MobileNo == null)
                    {
                        item.MobileNo = "";
                    }
                    if (item.Gender == null)
                    {
                        item.Gender = "";
                    }

                    if (item.City == null)
                    {
                        item.City = "";
                    } if (item.Full_Name == null)
                    {
                        item.Full_Name = "";
                    }
                    btn += inum;
                    tbl += inum; inum++;

                 //   int totmem = Convert.ToInt32(item.Self) + Convert.ToInt32(item.Spouses) + Convert.ToInt32(item.son);
                    var bill = "";
                    if (item.PremiuminServiceTax == "True")
                   {
                       bill = "Paid";
                   }
                   else
                   {
                       bill = "UnPaid";
                   }
                    var valuedownlode = "https://checkyourpremium.com/HealthPlan/Download?refid=" + item.income;
                    string column1 = "<p>" + item.Full_Name + "<p>";
                    string column2 = "<p>" + item.Gender + "<p>";
                    string column3 = "<p>" + item.MobileNo + "<p>";
                    string column4 = "<p>" + item.City + "<p>";
                    string column5 = "<p><a target='_blank' href=" + valuedownlode + ">Downlode Policy<p>";
                    string column6 = "<p>" + bill + "<p>";
                    string column7 = "<p>" + item.CoverForYear + "<p>";
                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    common.Add(column5);
                    common.Add(column6);
                    common.Add(column7);
                    DetailList.Add(common);

                }
                jsonData.data = DetailList;
                return new JsonResult()
                {
                    Data = jsonData,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }


            catch (Exception ex)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("ErrorHandling", "ErrorHandler"),
                    isRedirect = true
                });
                // return RedirectToAction("ErrorHandling/ErrorHandler");
                (new ErrorLog()).Error(ex.ToString(), "");
                return Json(null);
            }



        }

        public ActionResult TermLifeDtls()
        {


            if (Session["MobileNumber"] != null)
            {
                ViewBag.transId = Session["MobileNumber"];
            }
            else
            {

                return RedirectToAction("MobileVerification", "CustomerLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult Searchdata(int draw, int start, int length, string searchTxt, string searchString, string FullName)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                ZPModel objSearchHealthInsurance = new ZPModel();
                objSearchHealthInsurance.Phone = FullName;
                List<ZPModel> list = ICustomerBusiness.GetSearchList(objSearchHealthInsurance);
                //List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth, string searchString);

                var commonList = list;
                commonList = list;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                }

                //switch (order)
                //{

                //    default:
                //        commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.Full_Name).ToList() : commonList.OrderByDescending(x => x.Full_Name).ToList();
                //        break;
                //}
                //new
                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                List<List<string>> newDetailList = new List<List<string>>();
                string action = string.Empty;


                var i = 1;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {

                    if (item.FullName == null)
                    {
                        item.FullName = "";
                    }
                    if (item.DOB == null)
                    {
                        item.DOB = "";
                    } if (item.Phone == null)
                    {
                        item.Phone = "";
                    }
                    if (item.Income == null)
                    {
                        item.Income = "";
                    }
                    if (item.Gender == null)
                    {
                        item.Gender = "";
                    }

                    if (item.POLICY_TERM == null)
                    {
                        item.POLICY_TERM = "";
                    }
                    btn += inum;
                    tbl += inum; inum++;
                    //int totmem=Convert.ToInt32(item.Self)+Convert.ToInt32(item.Spouses)+Convert.ToInt32(item.son);
                    // 

                    string column1 = "<p>" + item.FullName + "<p>";
                    string column2 = "<p>" + item.Gender + "<p>";
                    string column3 = "<p>" + item.Phone + "<p>";
                    string column4 = "<p>" + item.POLICY_TERM + "<p>";
                    string column5 = "<p>" + item.totalPremium + "<p>";
                    string column6 = "<p>" + item.DSA_ind + "<p>";
                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    common.Add(column5);
                    common.Add(column6);
                    DetailList.Add(common);

                }
                jsonData.data = DetailList;
                return new JsonResult()
                {
                    Data = jsonData,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }


            catch (Exception ex)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("ErrorHandling", "ErrorHandler"),
                    isRedirect = true
                });
                // return RedirectToAction("ErrorHandling/ErrorHandler");
                (new ErrorLog()).Error(ex.ToString(), "");
                return Json(null);
            }



        }
        public ActionResult TravelDetails()
        {

            if (Session["MobileNumber"] != null)
            {
                ViewBag.transId = Session["MobileNumber"];
            }
            else
            {

                return RedirectToAction("MobileVerification", "CustomerLogin");
            }
            return View();
        }
        [HttpPost]
        public JsonResult SearchdataTravel(int draw, int start, int length, string searchTxt, string searchString, string FullName)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                SearchTravelInsurance objSearchHealthInsurance = new SearchTravelInsurance();
                objSearchHealthInsurance.travellerName = FullName;
                List<SearchTravelInsurance> list = ICustomerBusiness.GetTravelList(objSearchHealthInsurance);
                //List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth, string searchString);

                var commonList = list;
                commonList = list;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                }

                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                List<List<string>> newDetailList = new List<List<string>>();
                string action = string.Empty;


                var i = 1;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {


                    btn += inum;
                    tbl += inum; inum++;

                    //int totmem = Convert.ToInt32(item.Self) + Convert.ToInt32(item.Spouses) + Convert.ToInt32(item.son);
                    // 

                    string column1 = "<p>" + item.travellerName + "<p>";
                    string column2 = "<p>" + item.ageSelf + "<p>";
                    string column3 = "<p>" + item.City + "<p>";
                    string column4 = "<p>" + item.Phone + "<p>";
                    string column5 = "<p>" + item.Email + "<p>";
                    string column6 = "<p>" + item.destination + "<p>";
                    string column7 = "<p>" + item.tripStartDate + "<p>";
                    string column8 = "<p>" + item.tripEndDate + "<p>";
                    string column9 = "<p>" + item.ageSpouse + "<p>";
                    string column10 = "<p>" + item.ageChild1 + "<p>";
                    string column11 = "<p>" + item.ageChild2 + "<p>";
                    string column12 = "<p>" + item.ageBrother + "<p>";
                    string column13 = "<p>" + item.ageMother + "<p>";
                    string column14 = "<p>" + item.ageFather + "<p>";

                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    common.Add(column5);
                    common.Add(column6);
                    common.Add(column7);
                    common.Add(column8);
                    common.Add(column9);
                    common.Add(column10);
                    common.Add(column11);
                    common.Add(column12);
                    common.Add(column13);
                    common.Add(column14);
                    DetailList.Add(common);

                }
                jsonData.data = DetailList;
                return new JsonResult()
                {
                    Data = jsonData,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }


            catch (Exception ex)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("ErrorHandling", "ErrorHandler"),
                    isRedirect = true
                });
                // return RedirectToAction("ErrorHandling/ErrorHandler");
                (new ErrorLog()).Error(ex.ToString(), "");
                return Json(null);
            }



        }

        public ActionResult MotorDetails()
        {

            if (Session["MobileNumber"] != null)
            {
                ViewBag.transId = Session["MobileNumber"];
            }
            else
            {

                return RedirectToAction("MobileVerification", "CustomerLogin");
            }
            return View();
        }

        [HttpPost]
        public JsonResult SearchdataMotor(int draw, int start, int length, string searchTxt, string searchString, string FullName)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                BuyInfoDetails objSearchHealthInsurance = new BuyInfoDetails();
                objSearchHealthInsurance.Phone = FullName;
                List<BuyInfoDetails> list = ICustomerBusiness.GetMotorList(objSearchHealthInsurance);
                //List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth, string searchString);

                var commonList = list;
                commonList = list;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                }

                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                List<List<string>> newDetailList = new List<List<string>>();
                string action = string.Empty;


                var i = 1;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {


                    btn += inum;
                    tbl += inum; inum++;
                    if(item.Status=="True")
                    {
                        item.Status = "Purchage";
                    }
                    else
                    {
                        item.Status = "";
                    }
                    //int totmem = Convert.ToInt32(item.Self) + Convert.ToInt32(item.Spouses) + Convert.ToInt32(item.son);
                    // 

                    string column1 = "<p>" + item.Name + "<p>";
                    string column2 = "<p>" + item.Email + "<p>";
                    string column3 = "<p>" + item.Phone + "<p>";
                   
                    string column4 = "<p>" + item.Status + "<p>";
                    

                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                   
                   
                    DetailList.Add(common);

                }
                jsonData.data = DetailList;
                return new JsonResult()
                {
                    Data = jsonData,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }


            catch (Exception ex)
            {
                return Json(new
                {
                    redirectUrl = Url.Action("ErrorHandling", "ErrorHandler"),
                    isRedirect = true
                });
                // return RedirectToAction("ErrorHandling/ErrorHandler");
                (new ErrorLog()).Error(ex.ToString(), "");
                return Json(null);
            }



        }

    }
}