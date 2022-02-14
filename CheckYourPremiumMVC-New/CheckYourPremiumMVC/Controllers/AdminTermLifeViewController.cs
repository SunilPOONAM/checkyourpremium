using Business;
using CheckYourPremiumMVC.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckYourPremiumMVC.Controllers
{
    public class AdminTermLifeViewController : Controller
    {
        // GET: AdminTermLifeView
        public IAdminTermLifeBussiness IAdminTermLifeBussiness;
        public AdminTermLifeViewController()
        {
            IAdminTermLifeBussiness = new AdminTermLifeBussiness();
        }
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult QHealthListData(ZPModel srchHealthIns)
        {
            try
            {
                if (Session["EmailID"] != null)
                {

                }
                else
                {

                    return RedirectToAction("LoginDetails", "RegistrationLogin");
                }
                List<ZPModel> list = IAdminTermLifeBussiness.GetHealthPremiumList(srchHealthIns);
                var data = list.ToList();
                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        public ActionResult DisplayTerm()
        {
            return View();
        }
        [HttpPost]
        public JsonResult BindPlanList(int draw, int start, int length, string searchTxt, string searchString)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];

                ZPModel srchHealthIns = new ZPModel();
                List<ZPModel> list = IAdminTermLifeBussiness.GetHealthPremiumList(srchHealthIns);
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
                    string column5 = "<p>" + item.Income + "<p>";
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

                ZPModel objSearchHealthInsurance = new ZPModel();
                objSearchHealthInsurance.FullName = FullName;
                List<ZPModel> list = IAdminTermLifeBussiness.GetSearchList(objSearchHealthInsurance);
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
                    string column5 = "<p>" + item.Income + "<p>";
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