using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Domain;
using CheckYourPremiumMVC.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
namespace CheckYourPremiumMVC.Controllers
{
    public class TravelInsuranceController : Controller
    {
        // GET: TravelInsurance
        public ITravelBusiness _ITravelBusiness;
        public ICommonBusiness _IProposerBusiness;
        public ICommonBusiness _ITravellerBusiness;
        public IBhartiAxa _IBhartiAxaTravel;
        public List<string> triptype;
        public List<string> relations;
        public List<string> isPassport;
        string secretKey = string.Empty;
        string apiKey = string.Empty;
        public TravelInsuranceController()
        {
            _ITravelBusiness = new TravelBusiness();
            _IProposerBusiness = new ProposerBusiness();
            _ITravellerBusiness = new TravellerBusiness();
            _IBhartiAxaTravel = new BhartiAxa();
            triptype = new List<string>();
            relations = new List<string>();
            isPassport = new List<string>();
            triptype.Add("Single");
            triptype.Add("Multi");
            relations.Add("Sibling");
            relations.Add("Child");
            relations.Add("Niece");
            relations.Add("GrandParent");
            relations.Add("GrandChild");
            relations.Add("Brother-in-Law");
            relations.Add("Sister-in-Law");
            relations.Add("Nephew");
            relations.Add("Parent");
            relations.Add("Spouse");
            isPassport.Add("Yes");
            isPassport.Add("No");
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetQuote()
        {
            BindDropdown();
            return View();
        }
        public void BindDropdown()
        {
            var destinations = _ITravelBusiness.GetAllDestination();
            SelectList destinationsSelectList = new SelectList(destinations, "text", "value");
            ViewData["Destinations"] = destinationsSelectList;
            SelectList listTripType = new SelectList(triptype);
            ViewData["listTripType"] = listTripType;
        }
        public void BindCoverageAmount()
        {
            var CoverAgeAmount = _ITravelBusiness.GetAllCoverAgeAmount();
            SelectList CoverAgeAmountSelectList = new SelectList(CoverAgeAmount, "text", "value");
            ViewData["CoverageAmount"] = CoverAgeAmountSelectList;

        }
        public void BindAssigneeRelation(string type, Int32 planId)
        {
            var CoverAgeAmount = _ITravelBusiness.GetAssigneeNominee(type, planId);
            SelectList CoverAgeAmountSelectList = new SelectList(CoverAgeAmount, "text", "value");
            ViewData["assigneerelations"] = CoverAgeAmountSelectList;

        }

        public void BindAssigneeRelationBharti(string type, Int32 planId, string compid)
        {
            var CoverAgeAmount = _ITravelBusiness.GetAssigneeNominee(type, planId, compid);
            SelectList CoverAgeAmountSelectList = new SelectList(CoverAgeAmount, "text", "value");
            ViewData["assigneerelations"] = CoverAgeAmountSelectList;

        }
        public void BindRelations()
        {
            //  var relations = _ITravelBusiness.GetRelations();
            //   SelectList selectList = new SelectList(relations, "text", "value");
            SelectList selectList = new SelectList(relations);
            ViewData["relations"] = selectList;

        }

        public void BindRelationStar()
        {
            var CoverAgeAmount = _ITravelBusiness.GetReltionStar();

            SelectList selectlist = new SelectList(CoverAgeAmount, "value", "text");
            ViewData["relationsstar"] = selectlist;
        }
        public void BindRelationGdigit()
        {
            var CoverAgeAmount = _ITravelBusiness.GetReltionGodigit();

            SelectList selectlist = new SelectList(CoverAgeAmount, "value", "text");
            ViewData["relationsGodigit"] = selectlist;
        }
        public void BindRelationGodit()
        {
            var CoverAgeAmount = _ITravelBusiness.GetReltionStar();

            SelectList selectlist = new SelectList(CoverAgeAmount, "value", "text");
            ViewData["relationsGodit"] = selectlist;
        }
        public void BindPurpose()
        {
            var purpose = _ITravelBusiness.GetPurpose();
            SelectList destinationsSelectList = new SelectList(purpose, "text", "value");
            ViewData["TravelPurpose"] = destinationsSelectList;
        }
        public void BindGender()
        {
            var purpose = _ITravelBusiness.GetGender();
            SelectList destinationsSelectList = new SelectList(purpose, "text", "value");
            ViewData["TravelGender"] = destinationsSelectList;
        }
        public void BindDesease()
        {
            var desease = _ITravelBusiness.GetIllness();
            SelectList selectList = new SelectList(desease, "text", "value");
            ViewData["desease"] = selectList;
        }
        //.............ponam
        /// <summary>
        public void BindIProfession()
        {
            var ilness = _ITravelBusiness.GetProfession();
            SelectList selectList = new SelectList(ilness, "text", "value");
            ViewData["illness"] = selectList;
        }
        public void BindState()
        {
            var state = _ITravelBusiness.GetGodigitState();
            SelectList selectList = new SelectList(state, "text", "value");
            ViewData["GoDigitstate"] = selectList;
        }
        public void BindCountry(string pakagecode)
        {
            var country = _ITravelBusiness.GetGodigitCountry(pakagecode);
            SelectList selectList = new SelectList(country, "text", "value");
            ViewData["GoDigitCountry"] = selectList;
        }
        public void BindMarialStatus()
        {
            var marialstatus = _ITravelBusiness.GetGodigitMarialStatus();
            SelectList selectList = new SelectList(marialstatus, "text", "value");
            ViewData["GoDigitMaterial"] = selectList;
        }
        /// ////////////////////////////////
        /// </summary>
        public void BindVisaType()
        {
            var lstVisaType = _ITravelBusiness.GetVisaType();
            SelectList selectList = new SelectList(lstVisaType, "text", "value");
            ViewData["visaType"] = selectList;
        }
        public void BindPlaceOfVisit(string compid)
        {
            var lstPlaceOfVisit = _ITravelBusiness.GetPlaceOfVisit(compid);
            SelectList selectList = new SelectList(lstPlaceOfVisit, "text", "value");
            ViewData["PlaceOfVisit"] = selectList;
        }
        [HttpPost]
        public ActionResult GetQuote(SearchTravelInsurance searchTravelInsurance)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    BindDropdown();
                    return View(searchTravelInsurance);
                }
                int adults = 0; int child = 0;
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageSelf) && Convert.ToInt32(searchTravelInsurance.ageSelf) > 17)
                {
                    adults = 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageSpouse))
                {
                    adults += 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageChild1))
                {
                    child += 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageChild2))
                {
                    child += 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageFather))
                {
                    adults += 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageMother))
                {
                    adults += 1;
                }
                if (!string.IsNullOrEmpty(searchTravelInsurance.ageBrother) && Convert.ToInt32(searchTravelInsurance.ageSelf) > 17)
                {
                    adults += 1;
                }
                else
                {
                    child += 1;
                }

                string msg = "Hi%20" + searchTravelInsurance.travellerName + "%20, Start your happy days - share, compare, buy protection at Check Your Premium. Term and Condition apply. Visit  https://bit.ly/32XDHmQ";
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
                //................................
                var Tid = _ITravelBusiness.SaveTravelSearch(searchTravelInsurance);
                string tidd = Encode(Tid.ToString());

                return RedirectToAction("QResult/" + tidd, "TravelInsurance", new { @ageSelf = searchTravelInsurance.ageSelf, @destination = searchTravelInsurance.destination, @stayDays = searchTravelInsurance.stayDays, @adults = adults.ToString(), @children = child.ToString(), @travellerName = searchTravelInsurance.travellerName, @tripStartDate = searchTravelInsurance.tripStartDate, @tripEndDate = searchTravelInsurance.tripEndDate, @City = searchTravelInsurance.City, @phone = searchTravelInsurance.Phone, @Email = searchTravelInsurance.Email, @AgeSpouse = searchTravelInsurance.ageSpouse, @agechild1 = searchTravelInsurance.ageChild1, @agechild2 = searchTravelInsurance.ageChild2, @ageFather = searchTravelInsurance.ageFather, @ageMother = searchTravelInsurance.ageMother });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
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
        public ActionResult QResult(string Id, string ageSelf, string destination, string stayDays, string travellerName, string tripStartDate, string tripEndDate, string City, string Phone, string Email, string AgeSpouse, string agechild1, string agechild2, string ageFather, string ageMother, string adults, string children)
        {
            try
            {
                BindDropdown();
                BindCoverageAmount();
                ViewData["ageSelf"] = ageSelf ?? "0";
                ViewData["ageSpouse"] = AgeSpouse ?? "0";
                ViewData["ageChild1"] = agechild1 ?? "0";
                ViewData["ageChild2"] = agechild2 ?? "0";
                ViewData["ageFather"] = ageFather ?? "0";
                ViewData["ageMother"] = ageMother ?? "0";
                ViewData["destination"] = destination;
                ViewData["stayDays"] = stayDays;
                ViewData["TId"] = Id;
                ViewData["travellerName"] = travellerName;
                ViewData["tripStartDate"] = tripStartDate;
                ViewData["tripEndDate"] = tripEndDate;
                ViewData["city"] = City;
                ViewData["phone"] = Phone;
                ViewData["email"] = Email;
                // Login Session Code........
                //Session["id2"] = ViewData["TId"].ToString();
                //Session["agesel2"] = ViewData["ageSelf"].ToString();
                //Session["destination2"] = ViewData["destination"].ToString();
                //Session["stayDays2"] = ViewData["stayDays"].ToString();
                //Session["travellerName2"] = ViewData["travellerName"].ToString();
                //Session["tripStartDate2"] = ViewData["tripStartDate"].ToString();
                //Session["tripEndDate2"] = ViewData["tripEndDate"].ToString();
                //Session["city2"] = ViewData["city"].ToString();
                //Session["phone2"] = ViewData["phone"].ToString();
                //Session["email2"] = ViewData["email"].ToString();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public JsonResult BindPremiumList(int draw, int start, int length, string searchTxt, string ageSelf, string destination, string stayDays, string txtTriptype, string Suminsured, string travellerName, string tripStartDate, string tripEndDate, string City, string Phone, string Email, string agespouse, string agechild1, string agechild2, string agefather, string agemother, string ID)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];
                searchTxt = searchKey.Trim().ToLower();
                SearchTravelInsurance objSearchTravelInsurance = new SearchTravelInsurance();
                objSearchTravelInsurance.ageSelf = ageSelf;
                objSearchTravelInsurance.ageSpouse = agespouse;
                objSearchTravelInsurance.ageChild1 = agechild1;
                objSearchTravelInsurance.ageChild2 = agechild2;
                objSearchTravelInsurance.ageFather = agefather;
                objSearchTravelInsurance.ageMother = agemother;
                objSearchTravelInsurance.stayDays = stayDays;
                objSearchTravelInsurance.destination = destination;
                objSearchTravelInsurance.tripType = txtTriptype;
                objSearchTravelInsurance.SumInsured = Suminsured;
                View_TravelInsuranceModel objser1 = new View_TravelInsuranceModel();
                objser1.ageSelf = ageSelf;
                objser1.ageSpouse = agespouse;
                objser1.ageChild1 = agechild1;
                objser1.ageChild2 = agechild2;
                objser1.ageFather = agefather;
                objser1.ageMother = agemother;
                objser1.stayDays = stayDays;
                objser1.destination = destination;
                objser1.tripType = txtTriptype;
                objser1.travellerName = travellerName;
                DateTime Startdob = DateTime.ParseExact(tripStartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objser1.tripStartDate = Startdob.ToString("yyyy-MM-dd");
                DateTime Enddob = DateTime.ParseExact(tripEndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                objser1.tripEndDate = Enddob.ToString("yyyy-MM-dd");
                objser1.City = City;
                objser1.Phone = Phone;
                objser1.Email = Email;
                objser1.SumInsured = Convert.ToDecimal(Suminsured);
                List<View_TravelInsuranceModel> l = _ITravelBusiness.GetPakageList(objser1);
                objser1.pakegecode = objser1.pakegecode;
                var clist = l;
                foreach (var item in clist)
                {
                    objser1.pakegecode = item.pakegecode;
                }
                objSearchTravelInsurance.Loading = "Loading...........";
                View_TravelInsuranceModel bhartObj = new View_TravelInsuranceModel();
                bhartObj.ageSelf = ageSelf;
                bhartObj.ageSpouse = agespouse;
                bhartObj.ageChild1 = agechild1;
                bhartObj.ageChild2 = agechild2;
                bhartObj.ageFather = agefather;
                bhartObj.ageMother = agemother;
                bhartObj.stayDays = stayDays;
                bhartObj.destination = destination;
                bhartObj.tripType = txtTriptype;
                bhartObj.travellerName = travellerName;
                //  DateTime Startdob = DateTime.ParseExact(tripStartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                bhartObj.tripStartDate = Startdob.ToString("yyyy-MM-dd");
                // DateTime Enddob = DateTime.ParseExact(tripEndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                bhartObj.tripEndDate = Enddob.ToString("yyyy-MM-dd");
                bhartObj.City = City;
                bhartObj.Phone = Phone;
                bhartObj.Email = Email;
                bhartObj.SumInsured = Convert.ToDecimal(Suminsured);
                AllCompanyDetails srchproduct = new AllCompanyDetails();
                CompareDetails com = new CompareDetails();
                List<View_TravelInsuranceModel> list = _ITravelBusiness.GetPremiumList(objSearchTravelInsurance);
                List<View_TravelInsuranceModel> lstedlewise = _ITravelBusiness.GetPremiumGodigitResponce(objser1);
                List<View_TravelInsuranceModel> lstquato = _ITravelBusiness.SubmitQuato(bhartObj);
                List<AllCompanyDetails> list1 = _ITravelBusiness.GetAllCompanyDetails(srchproduct);
                List<CompareDetails> listCompare = _ITravelBusiness.GetCompareDetails(com);
                int bookingid = Convert.ToInt32(objser1.SearchId);
                ViewData["BookingId"] = bookingid.ToString();
                lstedlewise.AddRange(lstquato);
                list.AddRange(lstedlewise);
                var commonList = list;
                commonList = list;
              
                var commonList2 = list1;
                var commonList3 = listCompare;
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
                        commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.SumInsured).ToList() : commonList.OrderByDescending(x => x.SumInsured).ToList();
                        break;
                }


                jsonData.recordsTotal = commonList.Count;
                jsonData.recordsFiltered = commonList.Count;
                jsonData.draw = draw;
                commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
                List<List<string>> DetailList = new List<List<string>>();
                string action = string.Empty;


              //  var i = 1;
                var i = 0;
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
                   // decimal Premium1 = P + Premium;
                    //string pre = Premium1.ToString();
                    //............For Compare................
                    string Benefit = "";
                    string Co_Pay1 = "";
                    string Room_Rent1 = "";
                    string OPD = "";
                    string Day_Care_Treatment1 = "";
                    string Medical_Checkup = "";
                    string Pre_Existing_Disease_Covered_After = "";
                    string Domicilliary_Expenses = "";
                    string Organ_Donar_Expenses = "";
                    string Hospital_Cash_Daily_Limit = "";
                    string Maternity_Benefit = "";
                    string New_Born_Baby = "";
                    string Pre_Hospitalization1 = "";
                    string Post_Hospitalization1 = "";
                    string Ambulance_Charges1 = "";
                    string Health_Check_Up1 = "";
                    string Restoration_Benefit1 = "";
                    string Free_Look_Period1 = "";
                    int day = Convert.ToInt32(objSearchTravelInsurance.stayDays);
                    if (day >= 91 && item.LogoImage == "/Logo/bhartiaxa-logo.png")
                    { }
                    else
                    {
                        //.............................................
                        foreach (var item2 in commonList3)
                        {
                            if (item.CompanyId == Convert.ToInt32(item2.Company_Id))
                            {
                                if (item.Plans == item2.Company_Plan)
                                {


                                    Benefit = item2.Benefit;
                                    Co_Pay1 = item2.Co_Pay;
                                    Room_Rent1 = item2.Room_Rent;
                                    OPD = item2.OPD;
                                    Day_Care_Treatment1 = item2.Day_Care_Treatment;
                                    Medical_Checkup = item2.Medical_Checkup;
                                    Pre_Existing_Disease_Covered_After = item2.Pre_Existing_Disease_Covered_After;
                                    Domicilliary_Expenses = item2.Domicilliary_Expenses;
                                    Organ_Donar_Expenses = item2.Organ_Donar_Expenses;
                                    Hospital_Cash_Daily_Limit = item2.Hospital_Cash_Daily_Limit;
                                    Maternity_Benefit = item2.Maternity_Benefit;
                                    New_Born_Baby = item2.New_Born_Baby;
                                    Pre_Hospitalization1 = item2.Pre_Hospitalization;
                                    Post_Hospitalization1 = item2.Post_Hospitalization;
                                    Ambulance_Charges1 = item2.Ambulance_Charges;
                                    Health_Check_Up1 = item2.Health_Check_Up;
                                    Restoration_Benefit1 = item2.Restoration_Benefit;
                                    Free_Look_Period1 = item2.Free_Look_Period;


                                }
                            }
                        }
                        //...............
                        foreach (var item1 in commonList2)
                        {
                            if (item.CompanyId == Convert.ToInt32(item1.Company_Id))
                            {
                                if (item.Plans == item1.Company_Plan)
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
                        column1 = "<div class='selectProduct w3-padding' data-title=" + item.Plans + "  data-id=" + tbl + " > <input type='checkbox' class='addToCompare' value=\"" + i++ + "\"><span class='adCmps'>addToCompare</span><div style='display:none'><input type='text' class='compare1' value='" + Benefit + "'><input type='text' class='compare2' value='" + Co_Pay1 + "'><input type='text' class='compare3' value='" + Room_Rent1 + "'><input type='text' class='compare4' value='" + OPD + "'><input type='text' class='compare5' value='" + Day_Care_Treatment1 + "'><input type='text' class='compare6' value='" + Medical_Checkup + "'><input type='text' class='compare7' value='" + Pre_Existing_Disease_Covered_After + "'><input type='text' class='compare8' value='" + Domicilliary_Expenses + "'><input type='text' class='compare9' value='" + Organ_Donar_Expenses + "'><input type='text' class='compare10' value='" + Hospital_Cash_Daily_Limit + "'><input type='text' class='compare11' value='" + Maternity_Benefit + "'><input type='text' class='compare12' value='" + New_Born_Baby + "'><input type='text' class='compare13' value='" + Pre_Hospitalization1 + "'><input type='text' class='compare14' value='" + Post_Hospitalization1 + "'><input type='text' class='compare15' value='" + Ambulance_Charges1 + "'><input type='text' class='compare16' value='" + Health_Check_Up1 + "'><input type='text' class='compare17' value='" + Restoration_Benefit1 + "'><input type='text' class='compare18' value='" + Free_Look_Period1 + "'><a class='AddD' href='#' onclick='NavigateDetail(" + item.SearchId + ")'>Buy This Plan</a><input type='text' class='PremiumChart' value='" + item.SearchId + "'><input type='text' class='Premiumtotal' value='" + (item.Plans == "Star Health Insurance") + "'></div><img src=" + item.LogoImage + " class='imgFill productImg'/></div><p><button type='button' id='" + btn + "' onclick=HideTable('" + tbl + "');>Details</button></p>";
                         //column1 = "<img src=" + item.LogoImage + " />";
                        string column2 = "<p>" + item.Plans.ToUpper() + "<p>";
                        string column3 = "<p>" + item.SumInsured + "<p>";
                        string column4Premium = "<span><i class='fa fa-inr' aria-hidden='true'></i></span> " + item.Premium + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.Travel_id + "," + item.CompanyId + ")'>Buy This Plan</a>";
                        //string col4 = "<button  style='border:none;' onclick='navigateEdit(" + item.EDesigId + ")'  type='submit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button><button onclick='navigatedel(" + item.EDesigId + ")' style='border:none;' type='submit'><i class='fa fa-trash-o' aria-hidden='true'></i></button>";

                        List<string> common = new List<string>();
                        common.Add(column1);
                        common.Add(column2);
                        common.Add(column3);
                        common.Add(column4Premium);

                        string data = "<table cellpadding='5' id='" + tbl + "' style='width:100%;display:none' cellspacing='0' border='0' style='padding-left:50px;'> <tr><td>Company Plan:</td><td>" + itemplan + "</td></tr><tr><td>Benefit:</td><td>" + itemBenefit + "</td></tr><tr><td>Co Pay:</td><td>" + Co_Pay + "</td></tr><tr><td>Room Rent:</td><td>" + Room_Rent + "</td></tr><tr><td>Day Care Treatment:</td><td>" + Day_Care_Treatment + "</td></tr><tr><td>Restoration Benefit:</td><td>" + Restoration_Benefit + "</td></tr><tr><td>Pre Hospitalization:</td><td>" + Pre_Hospitalization + "</td></tr><tr><td>Post Hospitalization:</td><td>" + Post_Hospitalization + "</td></tr><tr><td>Ambulance_Charges:</td><td>" + Ambulance_Charges + "</td></tr><tr><td>Company_Plan:</td><td>" + Company_Plan + "</td></tr></table>";

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

                        //DetailList.Add(common);
                    }


                }

                jsonData.data = DetailList;
                objSearchTravelInsurance.Loading = "";
                return new JsonResult()
                {
                    Data = jsonData,
                    MaxJsonLength = Int32.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            catch (Exception)
            {
                // (new CreateLogFile()).ErrorLog(ex.ToString());
                return Json(null);
            }
        }

        public ActionResult GetDetail(string Id, string tid, string compid)
        {
            compid = "10005";
            string tidd = Decode(tid);
            ////Login Session Code 
            //if (Session["UserName"] == null)
            //{
            //    Session["IdNum2"] = Id;

            //}

            //if (Session["UserName"] != null)
            //{


            //    SearchTravelInsurance searchTravelInsurance = new SearchTravelInsurance();
            //    if (Id == null)
            //    {
            //        string Tid = Session["id2"].ToString();
            //        tid = Tid;
            //        Id = Session["IdNum22"].ToString();
            //        Response.Cookies["page"].Value = "";
            //    }
            //}
            //else
            //{
            //    Response.Cookies["page"].Value = "GetDetail";
            //    Response.Cookies["page"].Expires = DateTime.Now.AddHours(1);
            //    return RedirectToAction("LoginDetails", "RegistrationLogin");
            //}

            //// End..............

            GetQuotationDetail quotationDetail = new GetQuotationDetail(); ;
            try
            {
                BindPurpose();
                BindDesease();
                BindVisaType();
                BindRelationStar();
                BindRelations();
                BindPlaceOfVisit(compid);
                BindAssigneeRelation("travel", Convert.ToInt32(Id));
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
                {
                    return RedirectToAction("GetQuote");
                }

                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                quotationDetail = _ITravelBusiness.GetQuotationDetail(Id, tidd, compid);

            }
            catch (Exception ex)
            {

                (new ErrorLog()).Error("getdetail-controller=", ex.Message.ToString());
            }

            return View(quotationDetail);

        }

        [HttpPost]
        //public ActionResult GetDetail(string Id, string tid,GetQuotationDetail quotationDetail )
        public ActionResult GetDetail()
        {
            try
            {
                var form = Request.Form;
                var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);

                if (Request.Form["travelDeclaration"] != null)
                {
                    formData["travelDeclaration"] = "true";
                }

                var formDataSerialize = JsonConvert.SerializeObject(formData);
                var quotationDetail = JsonConvert.DeserializeObject<GetQuotationDetail>(formDataSerialize);
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;

                if (!ModelState.IsValid || quotationDetail.travelDeclaration == false)
                {
                    BindPurpose();
                    BindDesease();
                    BindVisaType();
                    BindRelations();
                    BindRelationStar();
                    BindPlaceOfVisit(quotationDetail.CompanyID.ToString());
                    BindAssigneeRelation("travel", Convert.ToInt32(quotationDetail.PlanId));
                    return View(quotationDetail);
                }
                _IProposerBusiness.SaveInfo(quotationDetail);
                _ITravellerBusiness.SaveInfo(quotationDetail);
                int companyid = quotationDetail.CompanyID;
                switch (companyid)
                {
                    case 10005:
                        var res = _ITravelBusiness.SubmitProposal(quotationDetail);
                        if (!string.IsNullOrEmpty(res) && !res.Contains("error"))
                        {
                            ViewBag.SecretKey = secretKey;
                            ViewBag.APIKey = apiKey;
                            ViewBag.referenceId = res;
                        }
                        else
                        {
                            ViewData["Error"] = res.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        }
                        break;
                    case 10004:
                        var res1 = _IBhartiAxaTravel.SubmitProposal(quotationDetail);

                        if (res1 != null && res1.OrderNo != null)
                        {
                            ViewBag.OrderNo = res1.OrderNo;
                            ViewBag.QuatoNo = res1.QuoteNo;
                            ViewBag.Channel = res1.Channel;
                            ViewBag.Product = res1.Product;
                            //ViewBag.Amount = res1.Amount;
                            ViewBag.Amount = res1.Amount;
                            ViewBag.referenceId = "0";
                            // return RedirectToAction("ProposalStatus", "TravelInsurance", new { @refid = res });
                        }
                        else
                        {
                            // ViewData["Error"] = res1.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        }
                        break;
                    //case 10008:
                    //    var res1 = _IBhartiAxaTravel.SubmitProposal(quotationDetail);

                    //    if (res1 != null && res1.OrderNo != null)
                    //    {
                    //        ViewBag.OrderNo = res1.OrderNo;
                    //        ViewBag.QuatoNo = res1.QuoteNo;
                    //        ViewBag.Channel = res1.Channel;
                    //        ViewBag.Product = res1.Product;
                    //        ViewBag.Amount = res1.Amount;
                    //        ViewBag.referenceId = "0";
                    //        // return RedirectToAction("ProposalStatus", "TravelInsurance", new { @refid = res });
                    //    }
                    //    else
                    //    {
                    //        // ViewData["Error"] = res1.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                    //    }
                    //    break;
                    default:
                        break;
                }

                BindPurpose();
                BindDesease();
                BindVisaType();
                BindRelations();
                BindRelationStar();
                BindPlaceOfVisit(quotationDetail.CompanyID.ToString());

                BindAssigneeRelation("travel", Convert.ToInt32(quotationDetail.PlanId));


                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }


        public ActionResult GetDetailGodigit(string Id, string tid, string SumInsuried)
        {
            GetQuotationDetail quotationDetail = new GetQuotationDetail(); ;
            string tidd = Decode(tid);
            try
            {
                BindGender();
                BindState();
                BindIProfession();
                BindVisaType();
                BindRelations();
                BindRelationStar();
                BindPlaceOfVisit(quotationDetail.CompanyID.ToString());
                BindPurpose();
                BindDesease();
                BindMarialStatus();
                BindRelationGodit();
                BindRelationGdigit();
                //  bookingid = ViewData["BookingId"].ToString();
                //BindAssigneeRelation("travel", Convert.ToInt32(Id));
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
                {
                    return RedirectToAction("GetQuote");
                }

                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                quotationDetail = _ITravelBusiness.GetQuotationDetailGodigit(Id, tidd, SumInsuried);
                BindCountry(quotationDetail.Plans);

            }
            catch (Exception ex)
            {

                (new ErrorLog()).Error("getdetail-controller=", ex.Message.ToString());
            }

            return View(quotationDetail);
        }

        [HttpPost]
        public ActionResult GetDetailGodigit()
        {
            var form = Request.Form;
            var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);

            if (Request.Form["travelDeclaration"] != null)
            {
                formData["travelDeclaration"] = "true";
            }

            var formDataSerialize = JsonConvert.SerializeObject(formData);
            var quotationDetail = JsonConvert.DeserializeObject<GetQuotationDetail>(formDataSerialize);
            SelectList lstPassport = new SelectList(isPassport);
            ViewData["lstPassport"] = lstPassport;
            _IProposerBusiness.SaveInfo(quotationDetail);
            _ITravellerBusiness.SaveInfo(quotationDetail);
            int companyid = quotationDetail.CompanyID;
            BindGender();
            BindDesease();
            BindState();
            BindIProfession();
            BindVisaType();
            BindRelations();
            BindRelationStar();
            BindPlaceOfVisit(quotationDetail.CompanyID.ToString());
            BindPurpose();
            BindCountry(quotationDetail.Plans);
            BindMarialStatus();
            BindRelationGodit();
            BindRelationGdigit();
            var res = _ITravelBusiness.SubmitProposalGodigit(quotationDetail);
            //////...........................SMS Sender...............................
            string host = Url.Content(Request.Url.PathAndQuery);
            //  string url = "http://checkyourpremium.com/TermAndLife/TermLifeList/" + TID + "?ageSelf=" + ageSelf + "&Gender=" + Gender + "&Smoke=" + Smoke + "&fullName=" + fullName + "&DOB=" + DOB + "&Email=" + Email + "&phone=" + Phone + "&Suminsured=" + Suminsured + "&POLICY_TERM=" + POLICY_TERM + "&Frequency=" + Frequency + "&City=" + State + "";

            //.....................send Sms

            string msg = "Dear%20" + quotationDetail.proposerName + "%20age%20" + quotationDetail.age + "%20your Term Life Insurance is ready to URl %20 and Your Policy Number is" + quotationDetail.applicationId;
            string phone = quotationDetail.mobile;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://app.smsinsta.in/vendorsms/pushsms.aspx?clientid=2ca8257e-3fe8-4e6d-88d1-95bfb22933b6&apikey=b3a54956-7755-4b4e-8796-9f23c6a8ad3d&msisdn=" + phone + "&sid=CYP123&msg=" + msg + "&fl=0&gwid=2 ");

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
            //.................Send Email
            //string to = "mishradivyanshi88@gmail.com"; //To address    
            //string from = quotationDetail.email; //From address    
            //MailMessage message = new MailMessage(from, to);

            //string mailbody = "Dear%20" + quotationDetail.proposerName + "%20age%20" + quotationDetail.age + "%20your Term Life Insurance is ready to URl %20 and Your Policy Number is" + quotationDetail.applicationId;
            //message.Subject = "Travel Insurance";
            //message.Body = mailbody;
            //message.BodyEncoding = Encoding.UTF8;
            //message.IsBodyHtml = true;
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            //System.Net.NetworkCredential basicCredential1 = new
            //System.Net.NetworkCredential("mishradivyanshi88@gmail.com", "");
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = false;
            //client.Credentials = basicCredential1;
            //try
            //{
            //    client.Send(message);
            //}

            //catch (Exception ex)
            //{
            //    throw ex;
            //}  
            //.....................
            //.............................

            return View(quotationDetail);
        }

        public ActionResult GetCity(string pinCode)
        {

            var response = _ITravelBusiness.GetCityList(pinCode);
            var result = response != null && !response[0].Contains("error") ? new { data = response, status = "success" } : new { data = response, status = "error" };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetArea(string pinCode, string cityId)
        {

            var response = _ITravelBusiness.GetArea(pinCode, cityId);
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ProposalStatus(string refid)
        {
            try
            {
                ViewBag.SecretKey = secretKey;
                ViewBag.APIKey = apiKey;
                ViewBag.referenceId = refid;
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
        [HttpPost]
        public ActionResult GenerateToken(string refId)
        {
            var redirectToken = _ITravelBusiness.GenerateToken(refId);
            // var redirectToken = "60b393827113566ce99e5160373c5331";
            ViewBag.RedirectToken = redirectToken;
            return Json(new { token = redirectToken }, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //public ActionResult getPayment(string refId)
        //{
        //    var redirectToken = _ITravelBusiness.GenerateToken(refId);
        //    // var redirectToken = "60b393827113566ce99e5160373c5331";
        //    ViewBag.RedirectToken = redirectToken;
        //    return Json(new { token = redirectToken }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetPaymentStatus(string purchaseToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(purchaseToken))
                {
                    var policyStatus = _ITravelBusiness.PolicyStatus(purchaseToken);
                    if (policyStatus.Count == 4)
                    {
                        ViewBag.ReferenceNumber = (policyStatus["RefNumber"] ?? "").ToString();
                        ViewBag.PolicyNumber = policyStatus["PolicyNumber"].ToString();
                        ViewBag.RefId = policyStatus["RefId"].ToString();
                        ViewBag.PolicyStatus = policyStatus["PolicyStatus"].ToString();
                    }
                    else
                    {
                        ViewBag.RefId = policyStatus["RefId"].ToString();
                        ViewBag.PolicyStatus = policyStatus["PolicyStatus"].ToString();
                        ViewBag.Note = policyStatus["Note"].ToString().Replace("{", "").Replace("}", "").Replace(@"""", "");

                    }


                }

                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpGet]
        public ActionResult Download(string refid)
        {

            var response = _ITravelBusiness.GetDocument(refid);
            return File(response, "application/pdf");

        }

        public ActionResult GetPaymentStatusBhartiAxa(string productID, string orderNo, string amount, string status, string transactionRef, string policyNo, string link, string emailId)
        {
            try
            {
                GetPaymentStatusBhartiAxa payment = new GetPaymentStatusBhartiAxa();
                payment.productID = productID;
                payment.orderNo = orderNo;
                payment.amount = amount;
                payment.status = status;
                payment.transactionRef = transactionRef;
                payment.policyNo = policyNo;
                payment.link = link;
                payment.emailId = emailId;
                var policyStatus = _ITravelBusiness.PolicyBhartiaxaStatus(payment);
                ViewBag.PolicyStatus = status;
                ViewData["Status"] = status;
                if (ViewBag.PolicyStatus == "success")
                {
                    var link1 = link.Split('/').Last();
                    ViewData["transactionRef"] = transactionRef;
                    ViewData["policyNo"] = policyNo;
                    ViewBag.PolicyNumber = ViewData["policyNo"].ToString();
                    ViewBag.RefId = ViewData["transactionRef"].ToString();
                    ViewData["link"] = link1;
                    ViewData["Note"] = "";
                }
                else
                {
                    ViewData["transactionRef"] = transactionRef;
                    ViewData["policyNo"] = policyNo;
                    ViewBag.RefId = ViewData["transactionRef"].ToString();
                    ViewBag.PolicyStatus = status;
                    ViewData["Note"] = "Payment Fail";
                    return RedirectToAction("Fail", "TravelInsurance");

                }


                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }
        public ActionResult GetGodigitPaymentStatus(string transactionNumber)
        {
            try
            {
                //pppppppppppppppppppppppppppppppppp
                if (!string.IsNullOrEmpty(transactionNumber))
                {
                    GodigitDetails obj = new GodigitDetails();
                    var policyId = _ITravelBusiness.PolicyPay(transactionNumber);
                    ViewData["policyNumber"] = policyId;
                    obj.policyID = policyId;
                    var res = _ITravelBusiness.GetPolicyPDf(obj);
                    ViewBag.Shedulepath = obj.schedulePath;
                    ViewBag.success = obj.message;
                }
                //pppppppppppppppppppppppppppppppppp
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        //...........................For Bharti Axa
        public ActionResult GetDetailBhartiAxa(string Id, string tid, string compid)
        {
            compid = "10004";
            string tidd = Decode(tid);
            ////Login Session Code 
            //if (Session["UserName"] == null)
            //{
            //    Session["IdNum2"] = Id;

            //}

            //if (Session["UserName"] != null)
            //{


            //    SearchTravelInsurance searchTravelInsurance = new SearchTravelInsurance();
            //    if (Id == null)
            //    {
            //        string Tid = Session["id2"].ToString();
            //        tid = Tid;
            //        Id = Session["IdNum22"].ToString();
            //        Response.Cookies["page"].Value = "";
            //    }
            //}
            //else
            //{
            //    Response.Cookies["page"].Value = "GetDetail";
            //    Response.Cookies["page"].Expires = DateTime.Now.AddHours(1);
            //    return RedirectToAction("LoginDetails", "RegistrationLogin");
            //}

            //// End..............

            GetQuotationDetail quotationDetail = new GetQuotationDetail(); ;
            try
            {
                BindPurpose();
                BindDesease();
                BindVisaType();
                BindRelationStar();
                BindRelations();
                BindPlaceOfVisit(compid);
                BindAssigneeRelationBharti("travel", Convert.ToInt32(Id), compid);
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
                {
                    return RedirectToAction("GetQuote");
                }

                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                quotationDetail = _ITravelBusiness.GetQuotationDetail(Id, tidd, compid);

            }
            catch (Exception ex)
            {

                (new ErrorLog()).Error("getdetail-controller=", ex.Message.ToString());
            }

            return View(quotationDetail);

        }

        [HttpPost]
        //public ActionResult GetDetail(string Id, string tid,GetQuotationDetail quotationDetail )
        public ActionResult GetDetailBhartiAxa()
        {
            try
            {
                var form = Request.Form;
                var formData = form.AllKeys.Where(p => form[p] != "null").ToDictionary(p => p, p => form[p]);

                if (Request.Form["travelDeclaration"] != null)
                {
                    formData["travelDeclaration"] = "true";
                }

                var formDataSerialize = JsonConvert.SerializeObject(formData);
                var quotationDetail = JsonConvert.DeserializeObject<GetQuotationDetail>(formDataSerialize);
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;

                if (!ModelState.IsValid || quotationDetail.travelDeclaration == false)
                {
                    BindPurpose();
                    BindDesease();
                    BindVisaType();
                    BindRelations();
                    BindRelationStar();
                    BindPlaceOfVisit(quotationDetail.CompanyID.ToString());
                    BindAssigneeRelationBharti("travel", Convert.ToInt32(quotationDetail.PlanId), "10004");
                    return View(quotationDetail);
                }
                _IProposerBusiness.SaveInfo(quotationDetail);
                _ITravellerBusiness.SaveInfo(quotationDetail);
                int companyid = quotationDetail.CompanyID;
                switch (companyid)
                {
                    case 10005:
                        var res = _ITravelBusiness.SubmitProposal(quotationDetail);
                        if (!string.IsNullOrEmpty(res) && !res.Contains("error"))
                        {
                            ViewBag.SecretKey = secretKey;
                            ViewBag.APIKey = apiKey;
                            ViewBag.referenceId = res;
                        }
                        else
                        {
                            ViewData["Error"] = res.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        }
                        break;
                    case 10004:
                        var res1 = _IBhartiAxaTravel.SubmitProposal(quotationDetail);

                        if (res1 != null && res1.OrderNo != null)
                        {
                            ViewBag.OrderNo = res1.OrderNo;
                            ViewBag.QuatoNo = res1.QuoteNo;
                            ViewBag.Channel = res1.Channel;
                            ViewBag.Product = res1.Product;
                            //ViewBag.Amount = res1.Amount;
                            ViewBag.Amount = res1.Amount;
                            ViewBag.referenceId = "0";
                            // return RedirectToAction("ProposalStatus", "TravelInsurance", new { @refid = res });
                        }
                        else
                        {
                            // ViewData["Error"] = res1.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                        }
                        break;
                    //case 10008:
                    //    var res1 = _IBhartiAxaTravel.SubmitProposal(quotationDetail);

                    //    if (res1 != null && res1.OrderNo != null)
                    //    {
                    //        ViewBag.OrderNo = res1.OrderNo;
                    //        ViewBag.QuatoNo = res1.QuoteNo;
                    //        ViewBag.Channel = res1.Channel;
                    //        ViewBag.Product = res1.Product;
                    //        ViewBag.Amount = res1.Amount;
                    //        ViewBag.referenceId = "0";
                    //        // return RedirectToAction("ProposalStatus", "TravelInsurance", new { @refid = res });
                    //    }
                    //    else
                    //    {
                    //        // ViewData["Error"] = res1.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");
                    //    }
                    //    break;
                    default:
                        break;
                }

                BindPurpose();
                BindDesease();
                BindVisaType();
                BindRelations();
                BindRelationStar();
                BindPlaceOfVisit(quotationDetail.CompanyID.ToString());
                BindAssigneeRelationBharti("travel", Convert.ToInt32(quotationDetail.PlanId), "10004");

                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
        //.......................................
    }
}