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
    public class HomeController : Controller
    {
        public IHomeBusiness _IHomeBusiness;
        //
        // GET: /Home/
        public HomeController()
        {
            _IHomeBusiness = new HomeBusiness();
         }
        public ActionResult Home()
        {
            try
            {

                //if (Session["EmailID"] == null)
                //{
                //    return RedirectToAction("ComingSoon", "Home");
                //}
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        public ActionResult ComingSoon()
        {
            return View();
        }


        public ActionResult ContatctUs()
        {
            return View();
        }

        public ActionResult Complaint()
        {
            return View();
        }
        public ActionResult PlanBuyInfoDetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PlanBuyInfoDetails(BuyInfoDetails data)
        {

            var details = _IHomeBusiness.SaveBuyerDetails(data);
            Response.Redirect(data.dataforjoin);
            // Response.Write("<script>window.open ('" + data.dataforjoin + "','_blank');</script>");
            return View();
        }
          [HttpPost]
        
        public JsonResult BindPlanList(int draw, int start, int length, string searchTxt)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];
                CompanyPlanDetails objSearchHealthInsurance = new CompanyPlanDetails();
                CompareDetails com = new CompareDetails();
                List<CompanyPlanDetails> list = _IHomeBusiness.GetPlanDetails(objSearchHealthInsurance);


                var commonList = list;
                commonList = list;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                    //commonList = commonList.Where(x => x.EDesigName.ToLower().Trim().Contains(searchTxt.ToLower().Trim())).ToList();
                }

                switch (order)
                {
                    //case "1":
                    //    commonList = orderby.Equals("asc") ? commonList.OrderBy(x => x.EDesigName).ToList() : commonList.OrderByDescending(x => x.EDesigName).ToList();
                    //    break;
                    default:
                        commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.CompanyName).ToList() : commonList.OrderByDescending(x => x.CompanyName).ToList();
                        break;
                }
                //new
                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                List<List<string>> newDetailList = new List<List<string>>();
                string action = string.Empty;


                var i = 0;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {

                    btn += inum;
                    tbl += inum; inum++;


                    string column1 = "<img src=" + item.CompanyLogo + " class='imgFill productImg'/>";
                    string column2 = "<p>" + item.CompanyName.ToUpper() + "<p>";
                    string column3 = "<p>" + item.PlanInfo + "<p>";
                    string column4 = "<button id='" + tbl + "' onclick=HideTable('" + tbl + "','" + item.Status + "''" + item.BuyNow + "') value='" + item.BuyNow + "'>Buy Now</button>";
                    string column5 = "<a target='_blank' href='" + item.PolicyXURL + "'>Buy Now</a>";

                    //string column4Premium = "<span><i class='fa fa-inr' aria-hidden='true'></i></span> " + (item.CompanyName == "Star Health Insurance" ? item.BuyNow.ToString() :"0") + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item. + ")'>Buy This Plan</a>";

                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    common.Add(column4);
                    // common.Add(column4);
                    common.Add(column5);

                    DetailList.Add(common);
                    List<string> common1 = new List<string>();
                    column1 = "";
                    column2 = "";
                    column3 = "";
                    column4 = "";
                    column5 = "";
                    common1.Add(column1);
                    common1.Add(column2);
                    common1.Add(column3);
                    common1.Add(column4);
                    // common.Add(column4);
                    common1.Add(column5);
                    DetailList.Add(common1);
                    ;
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
          public JsonResult BindPlanBuyList(int draw, int start, int length, string searchTxt, string BuyInfoValue)
          {
              JsonTableData jsonData = new JsonTableData();

              try
              {
                  string searchKey = Request["search[value]"];
                  string order = Request["order[0][column]"];
                  string orderby = Request["order[0][dir]"];
                  CompanyPlanDetails objSearchHealthInsurance = new CompanyPlanDetails();
                  objSearchHealthInsurance.Status = BuyInfoValue;
                  List<CompanyPlanDetails> list = _IHomeBusiness.GetPlanBuyDetails(objSearchHealthInsurance);


                  var commonList = list;
                  commonList = list;
                  if (!string.IsNullOrEmpty(searchTxt))
                  {
                      //commonList = commonList.Where(x => x.EDesigName.ToLower().Trim().Contains(searchTxt.ToLower().Trim())).ToList();
                  }

                  switch (order)
                  {
                      //case "1":
                      //    commonList = orderby.Equals("asc") ? commonList.OrderBy(x => x.EDesigName).ToList() : commonList.OrderByDescending(x => x.EDesigName).ToList();
                      //    break;
                      default:
                          commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.CompanyName).ToList() : commonList.OrderByDescending(x => x.CompanyName).ToList();
                          break;
                  }
                  //new
                  jsonData.recordsTotal = commonList.Count;
                  jsonData.recordsFiltered = commonList.Count;
                  jsonData.draw = draw;
                  commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                  List<List<string>> DetailList = new List<List<string>>();
                  List<List<string>> newDetailList = new List<List<string>>();
                  string action = string.Empty;


                  var i = 0;
                  int inum = 0;
                  var btn = "btn";
                  var tbl = "tbl";
                  foreach (var item in commonList)
                  {

                      btn += inum;
                      tbl += inum; inum++;


                      string column1 = "<img src=" + item.CompanyLogo + " class='imgFill productImg'/><br/><p style='text-align: left; margin-top: 10px; margin-left: 3px;'>" + item.CompanyName.ToUpper() + "<p>";
                      string column2 = "<p>" + item.PlanInfo + "</p></br><button style='background-color: transparent; color: blueviolet; border: none;' id='" + btn + "'  onclick=policyBau('" + btn + "') value='" + item.PolicyBoucher + "' >Product Brochure</button></br><button style='background-color: transparent; color: blueviolet; border: none;' id='" + tbl + "'  onclick=policyWor('" + tbl + "') value='" + item.PolicyWording + "' >Policy Wordings</button>";
                      string column3 = "<button id='" + tbl + "' class='AddD' onclick=HideTable('" + tbl + "','" + item.Status + "','" + item.BuyNow + "') value='" + item.BuyNow + "'>Proceed</button></br><span style='font-size:10px;'> *By clicking you agree to the<a> <span style='font-size:10px;color:blue;' onclick=policyTerm('" + btn + "')> Terms &amp; Conditions </span></a>and<a> <span style='font-size:10px;color:blue;' onclick=policyPrivacy('" + btn + "')>Privacy Policy</span></a>";
                      
                      List<string> common = new List<string>();
                      common.Add(column1);
                      common.Add(column2);
                      common.Add(column3);
                      //common.Add(column4);
                      //common.Add(column4);
                      //common.Add(column5);

                      DetailList.Add(common);
                      List<string> common1 = new List<string>();
                      column1 = "";
                      column2 = "";
                      column3 = "";
                      //column4 = "";
                      //column5 = "";
                      common1.Add(column1);
                      common1.Add(column2);
                      common1.Add(column3);
                      
                      DetailList.Add(common1);
                      ;
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
          public JsonResult AddBuyerData(string Name, string Phone, string Email)
          {
              try
              {
                  BuyInfoDetails Responcedetail = new BuyInfoDetails();
                  Responcedetail.Name = Name;
                  Responcedetail.Phone = Phone;
                  Responcedetail.Email = Email;
                  var details = _IHomeBusiness.SaveBuyerDetails(Responcedetail);
                  string message = "SUCCESS";
                  return Json(new { Message = message, JsonRequestBehavior.AllowGet });

              }
              catch (Exception ex)
              {

                  (new ErrorLog()).Error(ex.ToString(), "");
                  return Json(null);
              }
              return new JsonResult();
          }
    }
}
   