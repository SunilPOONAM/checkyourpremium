using CheckYourPremiumMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Domain;
using Newtonsoft.Json;
using System.Net;
using System.IO;
namespace CheckYourPremiumMVC.Controllers
{
    public class TermAndLifeController : Controller
    {
        public SoapRequestHandler soapRequestHandler;
        public ITermLifeBusiness _ITermLifeBusiness;

        public TermAndLifeController()
        {
            soapRequestHandler = new SoapRequestHandler();
            _ITermLifeBusiness = new TermLifeBusiness();
        }
        //
        // GET: /TermAndLife/
        static string constr = ConfigurationManager.ConnectionStrings["MyString"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TermLifeinsurance()
        {

            //if (Session["EmailID"] == null)
            //{
            //    return RedirectToAction("Home", "Home");
            //}
            BindState();
            return View();
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]

        public ActionResult TermLifeinsurance(GetIndiaFirstResponceDetail searchTravelInsurance)
        {

            BindState();
            //user.CustID = ViewBag.ID;
            //Session.Timeout = 36000;
            //SaveUser(user);

            //return Json(true,JsonRequestBehavior.AllowGet);
            if (!ModelState.IsValid)
            {

                return View(searchTravelInsurance);
            }
            var Tid = _ITermLifeBusiness.SaveTermLifeSearch(searchTravelInsurance);
            string name = searchTravelInsurance.fullName;
            string Dob = searchTravelInsurance.DOB;
            searchTravelInsurance.PemiumType = "Regular";
            searchTravelInsurance.Income = searchTravelInsurance.Income;
            long techId = Tid;
            var encoded = Encode(techId.ToString());
            ////...........................SMS Sender...............................
            string host = Url.Content(Request.Url.PathAndQuery);

            //.....................send Sms

            string msg = "Hi " + name + " , Start your happy days - share, compare, buy protection at Check Your Premium. Term and Condition apply. Visit  https://bit.ly/32XDHmQ";
            string phone = searchTravelInsurance.Phone;

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

                }
            }
            catch (Exception e)
            {

            }
            //.............................
            return RedirectToAction("TermLifeList/" + encoded, "TermAndLife", new { @ageSelf = searchTravelInsurance.Age, @Gender = searchTravelInsurance.Gender, @Smoke = searchTravelInsurance.Smoke, @fullName = searchTravelInsurance.fullName, @DOB = searchTravelInsurance.DOB, @Email = searchTravelInsurance.Email, @phone = searchTravelInsurance.Phone, @Suminsured = searchTravelInsurance.sumAssured, @POLICY_TERM = searchTravelInsurance.POLICY_TERM, @Frequency = searchTravelInsurance.Frequency, @City = searchTravelInsurance.city, @PemiumType = searchTravelInsurance.PemiumType, @Income = searchTravelInsurance.Income, @custid = Tid });
            //return View();
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
        public void BindMarialStatus()
        {
            var marialstatus = _ITermLifeBusiness.MarialStatus();
            SelectList selectList = new SelectList(marialstatus, "text", "value");
            ViewData["MaterialStatus"] = selectList;
        }
        public void BindState()
        {
            var state = _ITermLifeBusiness.BindState();
            SelectList selectstate = new SelectList(state, "text", "value");
            ViewData["State"] = selectstate;
        }
        public void PremiumType()
        {
            var premiumtype = _ITermLifeBusiness.PremiumType();
            SelectList selectstate = new SelectList(premiumtype, "text", "value");
            ViewData["PremiumTypenew"] = selectstate;
        }
        //Bind ICICI Data
        public void BindCountry()
        {
            var Country = _ITermLifeBusiness.GetCountry();
            SelectList selectstate = new SelectList(Country, "text", "value");
            ViewData["Country"] = selectstate;
        }
        public void BindICICIState()
        {
            var ICICIState = _ITermLifeBusiness.GetICICIState();
            SelectList selectstate = new SelectList(ICICIState, "text", "value");
            ViewData["ICICIState"] = selectstate;
        }
        public void BindNationality()
        {
            var Nationality = _ITermLifeBusiness.GetNationality();
            SelectList selectstate = new SelectList(Nationality, "text", "value");
            ViewData["Nationality"] = selectstate;
        }
        public void BindICICIMaritalStatus()
        {
            var ICICIMaritalStatus = _ITermLifeBusiness.GetICICIMaritalStatus();
            SelectList selectstate = new SelectList(ICICIMaritalStatus, "text", "value");
            ViewData["ICICIMaritalStatus"] = selectstate;
        }
        public void BindOccupation()
        {
            var Occupation = _ITermLifeBusiness.Occupation();
            SelectList selectstate = new SelectList(Occupation, "text", "value");
            ViewData["Occupation"] = selectstate;
        }
        public void BindOrganisation_Name()
        {
            var Organisation = _ITermLifeBusiness.Organaisation_Name();
            SelectList selectstate = new SelectList(Organisation, "text", "value");
            ViewData["Organisation"] = selectstate;
        }
        public void BindEducation()
        {
            var Education = _ITermLifeBusiness.GetEducation();
            SelectList selectstate = new SelectList(Education, "text", "value");
            ViewData["Education"] = selectstate;
        }
        public void BindTrusteeType()
        {
            var TrusteeType = _ITermLifeBusiness.GetTrusteeType();
            SelectList selectstate = new SelectList(TrusteeType, "text", "value");
            ViewData["TrusteeType"] = selectstate;
        }

        //Bind ICICI Data
        //poonam.................
        //poonam.................
        public void BindRiskOption()
        {
            var risk = _ITermLifeBusiness.BindRiskOptions();
            SelectList selectList = new SelectList(risk, "text", "value");
            ViewData["RiskOption"] = selectList;
        }
        public void BindProductPlan()
        {

            var CoverAgeAmount = _ITermLifeBusiness.BindcoverOptions();
            SelectList CoverAgeAmountSelectList = new SelectList(CoverAgeAmount, "text", "value");
            ViewData["ProductPlan"] = CoverAgeAmountSelectList;
        }

        public void BindSuminsured()
        {
            var suminsured = _ITermLifeBusiness.GetSuminsured();
            SelectList destinationsSelectList = new SelectList(suminsured, "text", "value");
            ViewData["suminsured"] = destinationsSelectList;
        }
        public void BindPolicyTerm()
        {
            var policy = _ITermLifeBusiness.GetPolicyTerm();
            SelectList destinationsSelectList = new SelectList(policy, "text", "value");
            ViewData["Policyterm"] = destinationsSelectList;
        }
        public void BindFrequency()
        {
            var frequency = _ITermLifeBusiness.GetFrequency();
            SelectList destinationsSelectList = new SelectList(frequency, "text", "value");
            ViewData["Frequ"] = destinationsSelectList;
        }

        //End pppppppppppppppp......
        public void RowCount(AddDataTermLife ID)
        {
            using (SqlConnection con = new SqlConnection("ConnectionString"))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM tbl_GetPremiumData";
                SqlCommand cmd = new SqlCommand(query, con);
                Int32 count = (Int32)cmd.ExecuteScalar();
                ID.CustID = count.ToString();
                ViewBag.ID = ID.CustID;
            }
        }

        public ActionResult TermLifeList(string ID, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string Suminsured, string POLICY_TERM, string Frequency, string city, string PemiumType, string Income, string custid)
        {
            try
            {

                BindSuminsured();//ppppppppppppppp....................................................
                BindPolicyTerm();
                BindFrequency();
                PremiumType();
                BindProductPlan();
                ViewData["TId"] = ID;
                ViewData["SelfAge"] = ageSelf;
                ViewData["Gender"] = Gender;
                if (Smoke == "true")
                {
                    Smoke = "Yes";
                }
                else if (Smoke == "false")
                {
                    Smoke = "No";
                }
                ViewData["Smoke"] = Smoke;

                ViewData["fullName"] = fullName;
                ViewData["DOB"] = DOB;
                ViewData["Email"] = Email;
                ViewData["Phone"] = Phone;
                ViewData["Sumins"] = Suminsured;
                ViewData["plicyterm"] = POLICY_TERM;
                ViewData["Frequency"] = Frequency;
                ViewData["city"] = city;
                ViewData["Premiumtype"] = PemiumType;
                ViewData["Income"] = Income;
                ViewData["Custid"] = custid;
                //////// For Login Session.............
                Session["id1"] = ViewData["TId"].ToString();
                Session["agesel1"] = ViewData["SelfAge"].ToString();
                Session["Gender1"] = ViewData["Gender"].ToString();
                Session["Smoke1"] = ViewData["Smoke"].ToString();
                Session["fullName1"] = ViewData["fullName"].ToString();
                Session["DOB1"] = ViewData["DOB"].ToString();
                Session["Email1"] = ViewData["Email"].ToString();
                Session["Phone1"] = ViewData["Phone"].ToString();
                Session["Suminsured1"] = ViewData["Sumins"].ToString();
                Session["POLICY1"] = ViewData["plicyterm"].ToString();
                Session["Frequency1"] = ViewData["Frequency"].ToString();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpPost]
        public JsonResult TermList(int draw, int start, int length, string searchTxt, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string Suminsured, string POLICY_TERM, string Frequency, string State, string TID, string PemiumType, string Income, string CustId)
        {

            JsonTableData jsonData = new JsonTableData();
            try
            {
                BindState();
                PremiumType();
                BindSuminsured();//ppppppppppppppp....................................................
                BindPolicyTerm();
                BindFrequency();
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];
                searchTxt = searchKey.Trim().ToLower();
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                int age = 60 - Convert.ToInt32(ageSelf);
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = Suminsured;
                objSearchTermLifeInsurance.POLICY_TERM = POLICY_TERM;
                objSearchTermLifeInsurance.POLICY_TERM = POLICY_TERM;
                objSearchTermLifeInsurance.CustID = CustId;
                objSearchTermLifeInsurance.Frequency = Frequency;
                objSearchTermLifeInsurance.city = State;
                objSearchTermLifeInsurance.PemiumType = PemiumType;
                string result1 = objSearchTermLifeInsurance.ANNUAL_GROSS_PREMIUM;
                string result2 = objSearchTermLifeInsurance.INSTALL_PREMIUM_BEFORETAX;
                string result3 = objSearchTermLifeInsurance.INSTALL_PREMIUM_TAXINCLUSIVE;
                string result4 = objSearchTermLifeInsurance.SUM_ASSURED;
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance1 = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance1.Age = ageSelf;
                objSearchTermLifeInsurance1.Gender = Gender;
                objSearchTermLifeInsurance1.Smoke = Smoke;
                objSearchTermLifeInsurance1.fullName = fullName;
                objSearchTermLifeInsurance1.DOB = DOB;
                objSearchTermLifeInsurance1.Phone = Phone;
                objSearchTermLifeInsurance1.Email = Email;
                objSearchTermLifeInsurance1.sumAssured = Suminsured;
                objSearchTermLifeInsurance1.POLICY_TERM = POLICY_TERM;
                objSearchTermLifeInsurance1.Frequency = Frequency;
                objSearchTermLifeInsurance1.city = State;
                objSearchTermLifeInsurance1.PemiumType = PemiumType;
                objSearchTermLifeInsurance1.CustID = CustId;
                result1 = objSearchTermLifeInsurance1.ANNUAL_GROSS_PREMIUM;
                result2 = objSearchTermLifeInsurance1.INSTALL_PREMIUM_BEFORETAX;
                result3 = objSearchTermLifeInsurance1.INSTALL_PREMIUM_TAXINCLUSIVE;
                result4 = objSearchTermLifeInsurance1.SUM_ASSURED;
                objSearchTermLifeInsurance.Income = Income;
                List<GetIndiaFirstResponceDetail> list = _ITermLifeBusiness.GetPremiumList(objSearchTermLifeInsurance);
                List<GetIndiaFirstResponceDetail> listHDFC = _ITermLifeBusiness.GetPremiumListHDFC(objSearchTermLifeInsurance);
                List<GetIndiaFirstResponceDetail> lstedlewise = _ITermLifeBusiness.GetPremiumEdelWises(objSearchTermLifeInsurance);
                List<GetIndiaFirstResponceDetail> listICICI = _ITermLifeBusiness.GetPremiumListICICI(objSearchTermLifeInsurance1);//ppppppp
                 List<GetIndiaFirstResponceDetail> listKotak = _ITermLifeBusiness.GetPremiumListKotak(objSearchTermLifeInsurance1);//
                //For Compare...........................ppppppppppppppppp
                 Compare_Term com = new Compare_Term();
                AllCompanyDetails srchproduct = new AllCompanyDetails();
                List<Compare_Term> listCompare = _ITermLifeBusiness.GetCompareDetails(com);
                List<AllCompanyDetails> list1 = _ITermLifeBusiness.GetAllCompanyDetails(srchproduct);
                //.....................................................

                
                var commonList2 = list1;//.................
                var commonList3 = listCompare;//.................................
               

                listKotak.AddRange(list);
                var commonList = listKotak;
                if (!string.IsNullOrEmpty(searchTxt))
                {
                   
                }

                switch (order)
                {

                    default:
                        
                        break;
                }
                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                string action = string.Empty;
                var i = 1;
                //  For Compare........................................................................
                // var i = 0;
                int inum = 0;
                var btn = "btn";
                var tbl = "tbl";
                foreach (var item in commonList)
                {

                    btn += inum;
                    tbl += inum; inum++;

                    string column4 = "";
                    string column1 = "";
                    string itemplan = "";
                    string itemBenefit = "";
                    string Co_Pay = "";
                    string Room_Rent = "";
                    string Day_Care_Treatment = "";
                    string Restoration_Benefit = "";
                    string Pre_Hospitalization = "";
                    string Post_Hospitalization = "";
                    string Ambulance_Charges = "";
                    string Company_Id = "";
                    string Company_Plan = "";

                    //decimal P = Convert.ToDecimal(item.BeforeServiceTax ?? "0");
                    //decimal Premium = P * Convert.ToDecimal((item.Gsttax)) / 100;
                    //decimal Premium1 = P + Premium;
                    //string pre = Premium1.ToString();
                    //............For Compare................
                    string Company = "";
                    string Plan_Name = "";
                    string Minimum_Entry_Age = "";
                    string Maximum_Entry_Age = "";
                    string Cover_Upto = "";
                    string Premium_Payment_Option = "";
                    string Payment_Payment_Mode = "";
                    string Minimum_Sum_Assured = "";
                    string Medical_Test_Required = "";
                    foreach (var item2 in commonList3)
                    {
                        if (item.Company == (item2.Company_Id))
                        {
                            if (item.PlanName == item2.Company)
                            {
 Company_Id = item2.Company_Id;
 Company = item2.Company;
 Plan_Name = item2.Plan_Name;
 Minimum_Entry_Age = item2.Minimum_Entry_Age;
 Maximum_Entry_Age = item2.Maximum_Entry_Age;
 Cover_Upto = item2.Cover_Upto;
 Premium_Payment_Option = item2.Premium_Payment_Option;
 Payment_Payment_Mode = item2.Payment_Payment_Mode;
 Minimum_Sum_Assured = item2.Minimum_Sum_Assured;
 Medical_Test_Required = item2.Medical_Test_Required;


                            }
                        }
                    }
                    //...............
                    foreach (var item1 in commonList2)
                    {
                        if (item.Company == (item1.Company_Id))
                        {
                            if (item.PlanName == item1.Company_Plan)
                            {

                                itemplan = item1.Company_Plan;
                                itemBenefit = item1.Benefit;
                                Co_Pay = item1.Co_Pay;
                                Room_Rent = item1.Room_Rent;
                                Day_Care_Treatment = item1.Day_Care_Treatment;
                                Restoration_Benefit = item1.Restoration_Benefit;
                                Pre_Hospitalization = item1.Pre_Hospitalization;
                                Post_Hospitalization = item1.Post_Hospitalization;
                                Ambulance_Charges = item1.Ambulance_Charges;
                                Company_Plan = item1.Company_Plan;
                                Company_Id = item1.Company_Id;

                            }
                        }
                    }
                    //.............compare........................


                    column1 = "<div class='selectProduct w3-padding' data-title=" + item.PlanName + "  data-id=" + tbl + " > <input type='checkbox' class='addToCompare' value=\"" + i++ + "\"><span class='adCmps'>addToCompare</span><div style='display:none'><input type='text' class='compare1' value='" + Company + "'><input type='text' class='compare2' value='" + Plan_Name + "'><input type='text' class='compare3' value='" + Minimum_Entry_Age + "'><input type='text' class='compare4' value='" + Maximum_Entry_Age + "'><input type='text' class='compare5' value='" + Cover_Upto + "'><input type='text' class='compare6' value='" + Premium_Payment_Option + "'><input type='text' class='compare7' value='" + Payment_Payment_Mode + "'><input type='text' class='compare8' value='" + Minimum_Sum_Assured + "'><input type='text' class='compare9' value='" + Medical_Test_Required + "'><a class='AddD' href='#' onclick='NavigateDetail(" + item.SearchId + ")'>Buy This Plan</a><input type='text' class='PremiumChart' value='" + item.SearchId + "'><input type='text' class='Premiumtotal' value='" + (item.PlanName == "Star Health Insurance") + "'></div><img src=" + item.Logo + " class='imgFill productImg'/></div><p><button type='button' id='" + btn + "' onclick=HideTable('" + tbl + "');>Details</button></p>";
                    //.........end........................

                    string column2 = "<p>" + item.PlanName + "<p>";
                    string column3 = "<p>" + item.SUM_ASSURED + "<p>";
                    string column4Premium = "<i class='fa fa-inr' aria-hidden='true'></i>" + item.INSTALL_PREMIUM_TAXINCLUSIVE + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.SearchId + "," + item.Company + ")'>Buy This Plan</a>";
                    //string col4 = "<button  style='border:none;' onclick='navigateEdit(" + item.EDesigId + ")'  type='submit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button><button onclick='navigatedel(" + item.EDesigId + ")' style='border:none;' type='submit'><i class='fa fa-trash-o' aria-hidden='true'></i></button>";
                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    // common.Add(column4);
                    common.Add(column4Premium);

                    //string data = "<table cellpadding='5' id='" + tbl + "' style='width:100%;display:none' cellspacing='0' border='0' style='padding-left:50px;'> <tr><td>Company Plan:</td><td>" + itemplan + "</td></tr><tr><td>Benefit:</td><td>" + itemBenefit + "</td></tr><tr><td>Co Pay:</td><td>" + Co_Pay + "</td></tr><tr><td>Room Rent:</td><td>" + Room_Rent + "</td></tr><tr><td>Day Care Treatment:</td><td>" + Day_Care_Treatment + "</td></tr><tr><td>Restoration Benefit:</td><td>" + Restoration_Benefit + "</td></tr><tr><td>Pre Hospitalization:</td><td>" + Pre_Hospitalization + "</td></tr><tr><td>Post Hospitalization:</td><td>" + Post_Hospitalization + "</td></tr><tr><td>Ambulance_Charges:</td><td>" + Ambulance_Charges + "</td></tr><tr><td>Company_Plan:</td><td>" + Company_Plan + "</td></tr></table>";
                    string data = "<table cellpadding='5' id='" + tbl + "' style='width:100%;display:none' cellspacing='0' border='0' style='padding-left:50px;'> <tr><td>Company Plan:</td><td>" + itemBenefit + "</td></tr><tr><td>Claim Settled:</td><td>" + Co_Pay + "</td></tr><tr><td>Min Entry Age:</td><td>" + Room_Rent + "</td></tr><tr><td>Max Entry Age:</td><td>" + Day_Care_Treatment + "</td></tr><tr><td>Cover Upto :</td><td>" + Restoration_Benefit + "</td></tr><tr><td>Payment Frequemcy:</td><td>" + Pre_Hospitalization + "</td></tr></table>";

                    DetailList.Add(common);
                    List<string> common1 = new List<string>();
                    column1 = "";
                    column2 = "";
                    column3 = "";
                    column4Premium = "";
                    common1.Add(data);
                    common1.Add(column2);
                    common1.Add(column3);
                    // common.Add(column4);
                    common1.Add(column4Premium);
                    DetailList.Add(common1);
                    ;
                    //  End Compare........................................................................



                    //string column1 = "<img src=" + item.Logo + " />";
                    //string column2 = "<p>" + item.PlanName + "<p>";
                    //string column3 = "<p>" + item.SUM_ASSURED + "<p>";
                    //string column4Premium = "<i class='fa fa-inr' aria-hidden='true'></i>" + item.INSTALL_PREMIUM_TAXINCLUSIVE + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.SearchId + "," + item.Company + ")'>Buy This Plan</a>";
                    ////string col4 = "<button  style='border:none;' onclick='navigateEdit(" + item.EDesigId + ")'  type='submit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button><button onclick='navigatedel(" + item.EDesigId + ")' style='border:none;' type='submit'><i class='fa fa-trash-o' aria-hidden='true'></i></button>";
                    //List<string> common = new List<string>();
                    //common.Add(column1);
                    //common.Add(column2);
                    //common.Add(column3);
                    //common.Add(column4Premium);

                    //DetailList.Add(common);



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
                return Json(null);
            }
            // return JsonResult()
        }
        public void BindGender()
        {
            var purpose = _ITermLifeBusiness.GetGender();
            SelectList destinationsSelectList = new SelectList(purpose, "text", "value");
            ViewData["SpouseGender"] = destinationsSelectList;
        }
        [ValidateInput(false)]
        public ActionResult GetDetails(string Id, string tid, string POLICY_TERM, string Frequency, string State)
        {
            try
            {
                var encoded = Decode(tid.ToString());
                // var iddecode = Decode(Id.ToString());
                ViewData["ID"] = Id;
                ViewData["Tid"] = encoded;
                ViewData["policyterm"] = POLICY_TERM;
                ViewData["Frequency"] = Frequency;
                ViewData["State"] = State;
                //if (Session["UserName"] == null)
                //{
                //    Session["IdNum"] = Id;

                //}

                //if (Session["UserName"] != null)
                //{


                //    GetIndiaFirstResponceDetail searchTravelInsurance = new GetIndiaFirstResponceDetail();
                //    if (Id == null)
                //    {
                //        string Tid = Session["id1"].ToString();
                //        tid = Tid;
                //        Id = Session["IdNum2"].ToString();
                //        searchTravelInsurance.POLICY_TERM = Session["POLICY1"].ToString();
                //        POLICY_TERM = searchTravelInsurance.POLICY_TERM;
                //        searchTravelInsurance.Frequency = Session["Frequency1"].ToString();
                //        Frequency = searchTravelInsurance.Frequency;
                //        Response.Cookies["page"].Value = "";
                //    }
                //}
                //else
                //{


                //    Response.Cookies["page"].Value = "GetDetails";
                //    // Response.Cookies["page"].Expires = DateTime.Now.AddHours(1);
                //    return RedirectToAction("LoginDetails", "RegistrationLogin");
                //}
                BindProductPlan();
                BindGender();
                // BindDropdown();
                BindState();
                BindMarialStatus();
                BindRiskOption();//ppppppppppppppppppppp.........
                var quotationDetail = _ITermLifeBusiness.GetQuotationDetail(Id, encoded, POLICY_TERM, Frequency, State);
                //  switch (quotationDetail.Company)
                //  {
                //    case 1:
                //        break;
                //    case 10007:
                //        break;
                //    default:
                //        break;
                //}

                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetDetails()
        {
            try
            {
                BindProductPlan();
                BindMarialStatus();
                BindGender();
                BindState();
                PremiumType();
                BindRiskOption();//ppppppppppppppppppppp.........
                var form = Request.Form;
                var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);
                var formDataSerialize = JsonConvert.SerializeObject(formData);
                var zpmodel = JsonConvert.DeserializeObject<ZPModel>(formDataSerialize);
                var quotationDetail = _ITermLifeBusiness.GetQuotationDetail(zpmodel);
                if (quotationDetail.Company != "10007")
                {
                    Session["zpmodel"] = quotationDetail;
                    return RedirectToAction("Redirect", "TermAndLife");
                }
                else
                {
                    return RedirectToAction("RedirectIndiaFirst", "TermAndLife");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
        [HttpGet]
        public ActionResult Redirect()
        {
            ZPModel zpmodel = (ZPModel)Session["zpmodel"];
            //  var quotationDetail = _ITermLifeBusiness.GetQuotationDetail(zpmodel);
            return View(zpmodel);
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GetPremium(string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string sumInsured, string term, string Frequency)
        {
            try
            {
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = sumInsured;
                objSearchTermLifeInsurance.POLICY_TERM = term;
                objSearchTermLifeInsurance.Frequency = Frequency;

                List<GetIndiaFirstResponceDetail> lstedlewise = _ITermLifeBusiness.GetPremiumEdelWises(objSearchTermLifeInsurance);

                return Json(new { premium = lstedlewise[0].totalPremium }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return Json(new { premium = "0" }, JsonRequestBehavior.AllowGet);
        }
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult GetIndiaFirst(string searchTxt, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string sumInsured, string term, string Frequency, string PemiumType, string State, string Income)///////////
        {
            try
            {
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = sumInsured;
                objSearchTermLifeInsurance.POLICY_TERM = term;
                objSearchTermLifeInsurance.Frequency = Frequency;
                // Now
                objSearchTermLifeInsurance.PemiumType = PemiumType;
                objSearchTermLifeInsurance.city = State;
                objSearchTermLifeInsurance.Income = Income;
                //Now
                List<GetIndiaFirstResponceDetail> list = _ITermLifeBusiness.GetPremiumList(objSearchTermLifeInsurance);

                return Json(new { premium = list[0].INSTALL_PREMIUM_TAXINCLUSIVE }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return Json(new { premium = "0" }, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetPremiumHDFC(string searchTxt, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string sumInsured, string term, string Frequency)
        {
            try
            {
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = sumInsured;
                objSearchTermLifeInsurance.POLICY_TERM = term;
                objSearchTermLifeInsurance.Frequency = Frequency;
                objSearchTermLifeInsurance.premiumWaiver = searchTxt;
                List<GetIndiaFirstResponceDetail> listHDFC = _ITermLifeBusiness.GetPremiumListHDFC(objSearchTermLifeInsurance);
                if (objSearchTermLifeInsurance.error != "")
                {
                    var error = objSearchTermLifeInsurance.error;
                    return Json(new { token = "0", msg = objSearchTermLifeInsurance.error }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { token = listHDFC[0].totalPremium, msg = "" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { token = "0" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return Json(new { premium = "0" }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult RedirectIndiaFirst()
        {
            return View();
        }
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetIndiaFirstData(string searchTxt, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string Suminsured, string POLICY_TERM, string Frequency, string State, string PemiumType, string Income, string Custid)
        {
            try
            {
                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = Suminsured;
                objSearchTermLifeInsurance.POLICY_TERM = POLICY_TERM;
                objSearchTermLifeInsurance.Frequency = Frequency;
                objSearchTermLifeInsurance.city = State;
                objSearchTermLifeInsurance.CustID = searchTxt;
                objSearchTermLifeInsurance.PemiumType = PemiumType;
                objSearchTermLifeInsurance.Income = Income;
                objSearchTermLifeInsurance.SearchId = Convert.ToInt32(Custid);
                string result1 = objSearchTermLifeInsurance.ANNUAL_GROSS_PREMIUM;
                string result2 = objSearchTermLifeInsurance.INSTALL_PREMIUM_BEFORETAX;
                string result3 = objSearchTermLifeInsurance.INSTALL_PREMIUM_TAXINCLUSIVE;
                string result4 = objSearchTermLifeInsurance.SUM_ASSURED;
                var quotationDetail = _ITermLifeBusiness.GetQuotationDetailIndiaFirst(objSearchTermLifeInsurance);
                // List<GetIndiaFirstResponceDetail> list = _ITermLifeBusiness.GetPremiumList(objSearchTermLifeInsurance);
                objSearchTermLifeInsurance.totalPremium = objSearchTermLifeInsurance.totalPremium;
                if (objSearchTermLifeInsurance.totalPremium != null)
                {

                    return Json(new { token = objSearchTermLifeInsurance.totalPremium }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Response.Redirect("<script>alert('This age is not valid');<script>");
                    return Json(new { token = "This age is not valid" }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { token = "0" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("");
        }

        public ActionResult GetCity(string state)
        {

            var destinations = _ITermLifeBusiness.GetAllCity(state);
            SelectList destinationsSelectList = new SelectList(destinations, "text", "value");
            // ViewData["Cities"] = destinationsSelectList;
            return Json(destinationsSelectList, JsonRequestBehavior.AllowGet);
            //return View();
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GetHdfcDetails(string Id, string tid, string POLICY_TERM, string Frequency, string State)
        {
            try
            {
                ViewData["ID"] = Id;
                ViewData["Tid"] = tid;
                ViewData["policyterm"] = POLICY_TERM;
                ViewData["Frequency"] = Frequency;
                BindGender();
                BindProductPlan();

                // BindDropdown();
                BindState();
                PremiumType();
                BindMarialStatus();
                BindRiskOption();//ppppppppppppppppppppp.........
                var quotationDetail = _ITermLifeBusiness.GetQuotationDetail(Id, tid, POLICY_TERM, Frequency, State);
                quotationDetail.PayoutOption = quotationDetail.PayoutOption;
                ViewData["PayoutOption"] = quotationDetail.PayoutOption;
                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        ///ICICI pppppppppppppppppppppppppppppppppppppp
        ///
        public ActionResult GetICICIDetails(string Id, string tid, string POLICY_TERM, string Frequency, string State)
        {
            try
            {
                var encoded = Decode(tid.ToString());
                // var iddecode = Decode(Id.ToString());
                ViewData["ID"] = Id;
                ViewData["Tid"] = encoded;
                ViewData["policyterm"] = POLICY_TERM;
                ViewData["Frequency"] = Frequency;
                ViewData["State"] = State;
                BindOrganisation_Name();
                BindOccupation();
                BindEducation();
                BindProductPlan();
                BindICICIMaritalStatus();
                BindGender();
                BindState();
                BindMarialStatus();
                BindRiskOption();//ppppppppppppppppppppp.........
                var quotationDetail = _ITermLifeBusiness.GetQuotationICICIDetail(Id, encoded, POLICY_TERM, Frequency, State);
                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetICICIDetails()
        {
            try
            {
                BindOrganisation_Name();
                BindOccupation();
                BindEducation();
                BindICICIMaritalStatus();
                BindProductPlan();
                BindMarialStatus();
                BindGender();
                BindState();
                PremiumType();
                BindRiskOption();//ppppppppppppppppppppp.........
                var form = Request.Form;
                var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);
                var formDataSerialize = JsonConvert.SerializeObject(formData);
                var zpmodel = JsonConvert.DeserializeObject<GetICICIResponseDetails>(formDataSerialize);
                var quotationDetail = _ITermLifeBusiness.SubmitProposalICICI(zpmodel);
                ViewBag.id = quotationDetail.responseRemarks;
                ViewBag.Url = quotationDetail.Url;
                ViewBag.transId = quotationDetail.TransactionId;
                quotationDetail.totalPremium = zpmodel.totalPremium;

                return View(quotationDetail);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
        [HttpPost]
        public ActionResult UpdateData(GetIndiaFirstResponceDetail std)
        {
            //...context.Tasks
            //.Where(t => t.StatusId == 1)
            //.Update(t => new Task { StatusId = 2 });
            var Tid = _ITermLifeBusiness.UpdateTermLifeSearch(std);
            string message = "SUCCESS";
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
        }

        //For Kotak................
        public ActionResult GetKotakDetails(string Id, string tid, string POLICY_TERM, string Frequency, string State)
        {
            var encoded = Decode(tid.ToString());
            ViewData["ID"] = Id;
            ViewData["Tid"] = tid;
            ViewData["policyterm"] = POLICY_TERM;
            ViewData["Frequency"] = Frequency;
            ViewData["State"] = State;
            //if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
            //{
            //    return RedirectToAction("GetQuote");
            //}
            BindProductPlan();
            BindGender();
            // BindDropdown();
            BindState();
            PremiumType();
            BindMarialStatus();
            BindRiskOption();//ppppppppppppppppppppp.........
            var quotationDetail = _ITermLifeBusiness.GetQuotationDetail(Id, encoded, POLICY_TERM, Frequency, State);
            //  switch (quotationDetail.Company)
            //  {
            //    case 1:
            //        break;
            //    case 10007:
            //        break;
            //    default:
            //        break;
            //}

            return View(quotationDetail);
        }
        [HttpPost]
        public ActionResult GetKotakDetails()
        {
            BindProductPlan();
            BindMarialStatus();
            BindGender();
            BindState();
            PremiumType();
            BindRiskOption();//ppppppppppppppppppppp.........
            var form = Request.Form;
            var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);
            var formDataSerialize = JsonConvert.SerializeObject(formData);
            var zpmodel = JsonConvert.DeserializeObject<Kotak>(formDataSerialize);
            //Kotak myDeserializedClass = JsonConvert.DeserializeObject<Kotak>(formDataSerialize);
            var quotationDetail = _ITermLifeBusiness.GetQuotationDetailKotak(zpmodel);

            if (quotationDetail.Company == "10014")
            {
                //Session["name"]= zpmodel.FullName;
                //Session["email"] = quotationDetail.Email;
                // return RedirectToAction("RedirectKotak", "TermAndLife");
                return RedirectToAction("RedirectKotak/TermAndLife", new { @Name = zpmodel.FullName, @Email = zpmodel.Email, @Smoke = zpmodel.Smoke, @Gender = zpmodel.Gender, Phone = zpmodel.Phone, @Dob = quotationDetail.DOB });
                // return RedirectToAction("uateinsurance.mykotaklife.com/einsurance/get-quote?", new { @Name = quotationDetail.FullName, @Email = quotationDetail.Email });
            }
            else
            {
                return RedirectToAction("RedirectKotak/TermAndLife", new { @Name = zpmodel.FullName, @Email = zpmodel.Email, @Smoke = zpmodel.Smoke, @Gender = zpmodel.Gender, Phone = zpmodel.Phone, @Dob = quotationDetail.DOB });
            }
            return View();
        }

        public ActionResult RedirectKotak(string Name, string Smoke, string Gender, string Phone, string Dob, string Email)
        {

            ViewData["Name"] = Name;
            ViewData["Smoke"] = Smoke;
            ViewData["Gender"] = Gender;
            ViewData["Phone"] = Phone;
            ViewData["Dob"] = Dob;
            ViewData["Email"] = Email;
            //ViewBag.Url = Session["email"].ToString();
            return View();
        }

        public ActionResult GetKotak(string searchTxt, string ageSelf, string Gender, string Smoke, string fullName, string DOB, string Email, string Phone, string sumInsured, string term, string Frequency, string PemiumType, string State, string Income, string plan, string PayoutOption, string PPT)
        {
            try
            {
                //if (PemiumType == null) { }else
                //{
                //    Session["Premiumtype"] = PemiumType;
                //}

                GetIndiaFirstResponceDetail objSearchTermLifeInsurance = new GetIndiaFirstResponceDetail();
                objSearchTermLifeInsurance.Age = ageSelf;
                objSearchTermLifeInsurance.Gender = Gender;
                objSearchTermLifeInsurance.Smoke = Smoke;
                objSearchTermLifeInsurance.fullName = fullName;
                objSearchTermLifeInsurance.DOB = DOB;
                objSearchTermLifeInsurance.Phone = Phone;
                objSearchTermLifeInsurance.Email = Email;
                objSearchTermLifeInsurance.sumAssured = sumInsured;
                objSearchTermLifeInsurance.POLICY_TERM = term;
                objSearchTermLifeInsurance.Frequency = Frequency;
                objSearchTermLifeInsurance.PemiumType = PemiumType;

                //if (objSearchTermLifeInsurance.PemiumType!=null)
                //{
                //    objSearchTermLifeInsurance.PemiumType = Session["Premiumtype"].ToString();
                //}
                objSearchTermLifeInsurance.city = State;
                objSearchTermLifeInsurance.CustID = searchTxt;
                objSearchTermLifeInsurance.Income = Income;
                objSearchTermLifeInsurance.PlanName = plan;
                objSearchTermLifeInsurance.Payout_option = PayoutOption;
                objSearchTermLifeInsurance.pdfName = PPT;
                List<GetIndiaFirstResponceDetail> list = _ITermLifeBusiness.GetPremiumListKotak(objSearchTermLifeInsurance);

                return Json(new { premium = list[0].INSTALL_PREMIUM_TAXINCLUSIVE }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {


            }
            return Json(new { premium = "0" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetKotakPlan(string ptype)
        {

            var destinations = _ITermLifeBusiness.GetKotakplan(ptype);
            SelectList destinationsSelectList = new SelectList(destinations, "text", "value");
            // ViewData["Cities"] = destinationsSelectList;
            return Json(destinationsSelectList, JsonRequestBehavior.AllowGet);
            //return View();
        }
    }
}