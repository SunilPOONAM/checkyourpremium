using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;
using Business;
using CheckYourPremiumMVC.Models;

namespace CheckYourPremiumMVC.Controllers
{
    public class AdminHealthViewController : Controller
    {
        // GET: AdminHealthView


        public adminhealthBusniess IadminhealthBusniess;
        public AdminHealthViewController()
        {
            IadminhealthBusniess = new adminhealthBusniess();

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayHealth(RegistrationDetails objuserlhealth)
        {

            return View();

        }



        [ValidateInput(false)]
        public ActionResult HealthListData(HealthPlanDetails objuserlhealth)
        {
            try
            {
                if (Session["EmailID"] == null)
                {
                    return RedirectToAction("CominSoon", "Home");
                }
                List<HealthPlanDetails> registration = IadminhealthBusniess.GetHealthList(objuserlhealth);
                var d = registration.ToList();
                return View(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        // Data Table
        [HttpPost]
        public JsonResult BindPlanList(int draw, int start, int length, string searchTxt, string searchString)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                HealthPlanDetails objSearchHealthInsurance = new HealthPlanDetails();
                //List<HealthPlanDetails> list = IadminhealthBusniess.GetDetails(objSearchHealthInsurance, searchString);
                List<HealthPlanDetails> list = IadminhealthBusniess.GetHealthList(objSearchHealthInsurance);
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

                    if (item.Self == null)
                    {
                        item.Self = "0";
                    }
                    if (item.Spouses == null)
                    {
                        item.Spouses = "0";
                    } if (item.son == null)
                    {
                        item.son = "0";
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
                    int totmem = Convert.ToInt32(item.Self) + Convert.ToInt32(item.Spouses) + Convert.ToInt32(item.son);
                    // 

                    string column1 = "<p>" + item.Full_Name + "<p>";
                    string column2 = "<p>" + item.Gender + "<p>";
                    string column3 = "<p>" + item.MobileNo + "<p>";
                    string column4 = "<p>" + item.City + "<p>";
                    string column5 = "<p>" + totmem + "<p>";
                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    common.Add(column5);
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
        [HttpPost]
        public JsonResult Searchdata(int draw, int start, int length, string searchTxt, string searchString, string FullName)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                HealthPlanDetails objSearchHealthInsurance = new HealthPlanDetails();
                objSearchHealthInsurance.Full_Name = FullName;
                List<HealthPlanDetails> list = IadminhealthBusniess.GetSearchList(objSearchHealthInsurance);
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

                    int totmem = Convert.ToInt32(item.Self) + Convert.ToInt32(item.Spouses) + Convert.ToInt32(item.son);
                    // 

                    string column1 = "<p>" + item.Full_Name + "<p>";
                    string column2 = "<p>" + item.Gender + "<p>";
                    string column3 = "<p>" + item.MobileNo + "<p>";
                    string column4 = "<p>" + item.City + "<p>";
                    string column5 = "<p>" + totmem + "<p>";
                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    common.Add(column5);
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