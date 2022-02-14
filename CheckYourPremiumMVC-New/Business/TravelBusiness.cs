using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;
using Domain;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
namespace Business
{

    public interface ITravelBusiness
    {
        List<customSelectList> GetAllDestination();
        List<customSelectList> GetAllCoverAgeAmount();
        List<customSelectList> GetReltionStar();
        List<customSelectList> GetReltionGodigit();
        List<View_TravelInsuranceModel> GetPremiumList(SearchTravelInsurance srchTrvlIns);
        List<AllCompanyDetails> GetAllCompanyDetails(AllCompanyDetails objuserlhealth);
        List<CompareDetails> GetCompareDetails(CompareDetails objuserlhealth);
        List<View_TravelInsuranceModel> SubmitQuato(View_TravelInsuranceModel srchTrvlIns);
        List<View_TravelInsuranceModel> GetPremiumGodigitResponce(View_TravelInsuranceModel srchTremIns);
        long SaveTravelSearch(SearchTravelInsurance srchTrvlIns);
        long SaveResponceGodigit(SearchTravelInsurance Responcedetail);
        string GetGodigitDetails(GetQuotationDetail srchTremIns);

        List<customSelectList> GetIllness();
        List<customSelectList> GetProfession();
        List<customSelectList> GetVisaType();
        List<customSelectList> GetPlaceOfVisit(string compid);
        List<customSelectList> GetGender();
        List<customSelectList> GetPurpose();
        List<customSelectList> GetGodigitState();
        List<customSelectList> GetGodigitCountry(string pakagecode);
        List<customSelectList> GetGodigitMarialStatus();
        List<Area> GetArea(string pinCode, string cityId);
        //List<string> GetCityList(string pinCode);
        List<string> GetCityList(string pinCode);

        GetQuotationDetail GetQuotationDetail(string planId, string TId, string compid);
        GetQuotationDetail GetQuotationDetailGodigit(string planId, string TId, string SumInsuried);
        string SubmitProposal(GetQuotationDetail getQuotationDetail);

        string SubmitProposalGodigit(GetQuotationDetail getQuotationDetail);
        string GenerateToken(string refId);
        Dictionary<string, string> PolicyStatus(string purchaseToken);
        byte[] GetDocument(string refId);
        List<customSelectList> GetAssigneeNominee(string type, Int32 planid);
        List<customSelectList> GetAssigneeNominee(string type, Int32 planid, string compid);
        string SaveGodigitResponceNew(GetQuotationDetail srchTrvlIns);
        string PolicyBhartiaxaStatus(GetPaymentStatusBhartiAxa payment);
        List<View_TravelInsuranceModel> GetPakageList(View_TravelInsuranceModel srchTrvlIns);
        //...........pppppppppppppp
        string GetPolicyPDf(GodigitDetails policynumber);
        String PolicyPay(string transactionNumber);//ppppppppppppppppppppppp
        //..........pppppppppppppp
    }
    public class TravelBusiness : ITravelBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private string baseUrl; string secretKey = string.Empty;
        private string GodigitbaseUrl;
        string apiKey = string.Empty;
        string apiKeyGodigit = string.Empty;
        private RequestHandler requestHandler = new RequestHandler();
        public TravelBusiness()
        {
            db = new CheckyourpremiumliveEntities();
            baseUrl = System.Configuration.ConfigurationSettings.AppSettings["StarBaseURL"].ToString();
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
            GodigitbaseUrl = System.Configuration.ConfigurationSettings.AppSettings["GodigitBaseURL"].ToString();
            // apiKeyGodigit = System.Configuration.ConfigurationSettings.AppSettings["GodigitApiKey"].ToString();
        }
        public List<customSelectList> GetPurpose()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var purposeList = db.TravelPurposeMasters.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Purpose, text = x.PurposeId.ToString() }).Distinct().ToList();
                return purposeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetProfession()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var illness = db.Tbl_Godigit_Profession.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.profession.ToString(), text = x.professionId }).Distinct().ToList();
                return illness;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetGodigitState()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var state = db.tbl_GodigitState.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.State.ToString(), text = x.StateID }).Distinct().ToList();
                return state;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetGodigitCountry(string pakagecode)
        {
            try
            {
                string geography = "";
                if (pakagecode == "Asia excluding Japan")
                {
                    geography = "Asia";
                }
                else if (pakagecode == "World-wide excluding USA and Canada")
                {
                    geography = "ROW";
                }
                else if (pakagecode == "World-wide including USA and Canada")
                {
                    geography = " UCI";
                }
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                //var state = db.tbl_GodigitCounty.AsQueryable().AsEnumerable().Select(x => new customSelectList { text = x.CountryID, value = x.Country.ToString() }).Distinct().ToList();

                var state = db.tbl_GodigitCounty.AsQueryable().AsEnumerable().Where(x => x.CountryCover == geography).Select(x => new customSelectList { text = x.CountryID, value = x.Country.ToString() }).Distinct().ToList();


                return state;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetGodigitMarialStatus()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var state = db.tbl_MaterialStatus.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Material.ToString(), text = x.Status }).Distinct().ToList();
                return state;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetGender()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var genderlist = db.tb_GodigitGender.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Gender.ToString(), text = x.GenderValue.ToString() }).Distinct().ToList();
                return genderlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetIllness()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var purposeList = db.Deseases.AsQueryable().Select(x => new customSelectList { text = x.DiseaseDescription, value = x.DiseaseDescription }).Distinct().ToList();
                return purposeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetVisaType()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var VisaList = db.VisaTypes.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.VType, text = x.ID.ToString() }).Distinct().ToList();
                return VisaList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetAllDestination()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var destinations = db.TravelDestinations.AsQueryable().Select(x => new customSelectList { text = x.Destination, value = x.Destination }).Distinct().ToList();
                return destinations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();

        }
        public List<customSelectList> GetReltionGodigit()
        {
            try
            {

                var destinations = db.tbl_Godigit_Relation.AsQueryable().AsEnumerable().Select(x => new customSelectList { text = x.Relation, value = x.RelationID.ToString() }).Distinct().ToList();
                return destinations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetReltionStar()
        {
            try
            {

                var destinations = db.InsuredRelations.AsQueryable().AsEnumerable().Select(x => new customSelectList { text = x.InsuredRelationshipName, value = x.RelationshipId.ToString() }).Distinct().ToList();
                return destinations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetAllCoverAgeAmount()
        {
            try
            {

                var Coverage = db.View_TravelInsurance.AsQueryable().AsEnumerable().Select(x => x.SumInsured.ToString()).Distinct().ToList().Select(x => new customSelectList { text = x, value = x }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetAssigneeNominee(string type, Int32 planid)
        {
            try
            {
                //  .Select(x => x.AssigneeRelationshipName.ToString(), x => x.AssigneeRelationshipId).Distinct().ToList()

                var CompanyId = db.View_TravelInsurance.AsEnumerable().Where(x =>
                    x.Travel_id == planid).FirstOrDefault().CompanyId;
                //if (CompanyId == 10004)
                //{
                //    var Coverage = db.AssigneeRelations.AsQueryable().AsEnumerable().Where(x => x.CompanyId == CompanyId && x.InsuranceType == type).Select(x => new customSelectList { text = x.AssigneeRelationshipName.ToString(), value = x.AssigneeRelationshipName.ToString() }).Distinct().ToList();
                //    return Coverage;
                //}
                //else
                //{
                var Coverage = db.AssigneeRelations.AsQueryable().AsEnumerable().Where(x => x.CompanyId == CompanyId && x.InsuranceType == type).Select(x => new customSelectList { text = x.AssigneeRelationshipId.ToString(), value = x.AssigneeRelationshipName }).Distinct().ToList();
                return Coverage;
                //  }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetAssigneeNominee(string type, Int32 planid, string compid)
        {
            try
            {
                //  .Select(x => x.AssigneeRelationshipName.ToString(), x => x.AssigneeRelationshipId).Distinct().ToList()

                var CompanyId = db.Companies.AsEnumerable().Where(x =>
                    x.CompanyID == Convert.ToInt32(compid)).FirstOrDefault().CompanyID;
                //if (CompanyId == 10004)
                //{
                //    var Coverage = db.AssigneeRelations.AsQueryable().AsEnumerable().Where(x => x.CompanyId == CompanyId && x.InsuranceType == type).Select(x => new customSelectList { text = x.AssigneeRelationshipName.ToString(), value = x.AssigneeRelationshipName.ToString() }).Distinct().ToList();
                //    return Coverage;
                //}
                //else
                //{
                var Coverage = db.AssigneeRelations.AsQueryable().AsEnumerable().Where(x => x.CompanyId == CompanyId && x.InsuranceType == type).Select(x => new customSelectList { text = x.AssigneeRelationshipId.ToString(), value = x.AssigneeRelationshipName }).Distinct().ToList();
                return Coverage;
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<string> GetCityList(string pinCode)
        {
            var cityList = new List<string>();
            //cityList.Add(new City { city_id = 0, city_name = "Select" });
            try
            {
                if (!string.IsNullOrEmpty(pinCode))
                {
                    var responseCityList = requestHandler.GetCityId(pinCode);

                    if (responseCityList["Status"] == "200")
                    {
                        var responseString = responseCityList["response"].ToString();
                        //if (responseString.Contains("Note"))
                        //{
                        //    var result = JsonConvert.DeserializeObject(responseString);
                        //    dicObj.Add("Note", result.ToString());

                        //}
                        //else
                        //{
                        var result = JsonConvert.DeserializeObject<CityList>(responseString);
                        cityList.Add(result.city[0].city_id.ToString());
                        cityList.Add(result.city[0].city_name.ToString());
                        return cityList;
                        //}
                    }
                    else
                    {
                        var errorVal = responseCityList["response"].ToString();
                        cityList.Add(errorVal.Replace("{", "").Replace("}", "").Replace("\"", ""));
                        return cityList;

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return cityList;
        }
        public long SaveTravelSearch(SearchTravelInsurance srchTrvlIns)
        {
            try
            {
                var tDetails = new TravelInsuranceSearchBy
                {
                    age = Convert.ToInt32(srchTrvlIns.ageSelf),
                    city = srchTrvlIns.City,
                    contactno = srchTrvlIns.Phone,
                    createdDate = DateTime.UtcNow,
                    destination = srchTrvlIns.destination,
                    emailid = srchTrvlIns.Email,
                    endDate = DateTime.ParseExact(srchTrvlIns.tripEndDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),

                    startDate = DateTime.ParseExact(srchTrvlIns.tripStartDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),

                    TravellerName = srchTrvlIns.travellerName,
                    ageSpouse = Convert.ToInt32(srchTrvlIns.ageSpouse),
                    ageChild1 = Convert.ToInt32(srchTrvlIns.ageChild1),
                    ageChild2 = Convert.ToInt32(srchTrvlIns.ageChild2),
                    ageBrother = Convert.ToInt32(srchTrvlIns.ageBrother),
                    ageFather = Convert.ToInt32(srchTrvlIns.ageFather),
                    ageMother = Convert.ToInt32(srchTrvlIns.ageMother)

                };
                db.TravelInsuranceSearchBies.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.SearchId;

                return tId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }
        public List<View_TravelInsuranceModel> SubmitQuato(View_TravelInsuranceModel srchTrv)
        {
            try
            {
                int adult = 0; int child = 0;
                if (srchTrv.ageSelf != "0")
                { srchTrv.ageSelf = "1"; }
                if (srchTrv.ageSpouse != "0")
                { srchTrv.ageSpouse = "1"; } if (srchTrv.ageChild1 != "0")
                { srchTrv.ageChild1 = "1"; }
                if (srchTrv.ageChild2 != "0")
                { srchTrv.ageChild2 = "1"; }
                var result = BhartiAxa.GetQuatorequset(srchTrv);
                XDocument docc = XDocument.Parse(result);
                List<View_TravelInsuranceModel> names = new List<View_TravelInsuranceModel>() { };
                List<View_TravelInsuranceModel> lst = new List<View_TravelInsuranceModel>();
                var value = "";
                XNamespace ns = "http://schemas.cordys.com/bagi/tparty/core/bpm/1.0";

                IEnumerable<XElement> res1 = docc.Descendants(ns + "PremiumSet");
                IEnumerable<XElement> res = docc.Descendants(ns + "PlanSet");

                XDocument doc1 = XDocument.Parse(result);
                List<XElement> result1 = doc1.Descendants(ns + "PlanSet").ToList();

                for (int intc = 0; intc <= result1.Count - 1; intc++)
                {

                    adult = Convert.ToInt32(srchTrv.ageSelf) + Convert.ToInt32(srchTrv.ageSpouse);
                    child = Convert.ToInt32(srchTrv.ageChild1) + Convert.ToInt32(srchTrv.ageChild2);
                    View_TravelInsuranceModel srchTrvlIns = new View_TravelInsuranceModel();
                    IEnumerable<XElement> responses1 = docc.Descendants(ns + "OrderNo");
                    foreach (XElement responsee in responses1)
                    {
                        result = (string)responsee.Value;
                        if (result != "NA")
                        {
                            srchTrvlIns.policyID = result;
                        }
                        IEnumerable<XElement> responses2 = docc.Descendants(ns + "QuoteNo");
                        foreach (XElement respons in responses2)
                        {
                            result = (string)respons.Value;
                            if (result != "NA")
                            {
                                srchTrvlIns.applicationId = result;
                            }
                        }

                    }
                    if (srchTrv.SumInsured == Convert.ToInt32(result1[intc].Element(ns + "SumInsured").Value.ToString()))
                    {

                        srchTrvlIns.Plans = result1[intc].Element(ns + "PlanName").Value.ToString();
                        srchTrvlIns.SumInsured = Convert.ToInt32(result1[intc].Element(ns + "SumInsured").Value.ToString());
                        srchTrvlIns.invoiceNumber = result1[intc].Element(ns + "SumInsured").Value.ToString();
                        decimal pre = Convert.ToDecimal(result1[intc].Element(ns + "Premium").Value.ToString());
                        int Premium = Convert.ToInt32(pre);
                        srchTrvlIns.Premium = Premium;
                        decimal premium = Convert.ToDecimal(result1[intc].Element(ns + "Premium").Value.ToString());
                        int premium2 = Convert.ToInt32(premium);
                        srchTrvlIns.policyPremiumd = premium2.ToString();//ServiceTax
                        decimal servicetax = Convert.ToDecimal(result1[intc].Element(ns + "ServiceTax").Value.ToString());
                        int servicetax2 = Convert.ToInt32(servicetax);
                        srchTrvlIns.sgst = servicetax2.ToString();
                        srchTrvlIns.Plan_ID = Convert.ToInt32(result1[intc].Element(ns + "PlanId").Value.ToString());
                        srchTrvlIns.SearchId = Convert.ToInt32(result1[intc].Element(ns + "PlanId").Value.ToString());
                        srchTrvlIns.Premium = Convert.ToInt32(result1[intc].Element(ns + "PremiumPayable").Value.ToString());
                        srchTrvlIns.LogoImage = "/Logo/bhartiaxa-logo.png";
                        srchTrvlIns.CompanyId = 10004;
                        int d = Convert.ToInt32(result1[intc].Element(ns + "PlanId").Value);
                        srchTrvlIns.Travel_id = d;
                        srchTrvlIns.No_Of_Memebers = adult;
                        srchTrvlIns.No_Of_Child = child;
                        lst.Add(srchTrvlIns);
                        srchTrvlIns.SearchId = SaveGodigitResponce(srchTrvlIns);
                        names.Add(srchTrvlIns);
                    }
                }

                return names;
            }
            catch (Exception ex)
            {
                return new List<View_TravelInsuranceModel>();
            }
            return new List<View_TravelInsuranceModel>();
        }
        public List<View_TravelInsuranceModel> GetPremiumList(SearchTravelInsurance srchTrvlIns)
        {
            try
            {
                int ageSelf = Convert.ToInt32(Convert.ToDecimal(srchTrvlIns.ageSelf) * 12);
                int stayDays = Convert.ToInt32(srchTrvlIns.stayDays);
                var destinations = db.View_TravelInsurance.AsEnumerable().Where(x =>
                    (Convert.ToInt32(x.Age_From) <= ageSelf
                    && Convert.ToInt32(x.Age_To) >= ageSelf)
                    && x.Destination == srchTrvlIns.destination && x.TripType == srchTrvlIns.tripType
                    && x.SumInsured == Convert.ToDecimal(srchTrvlIns.SumInsured) && x.CompanyId != 10004
                    //&& ((Convert.ToInt32(x.TermFrom) <= stayDays
                    //&& Convert.ToInt32(x.TermTo) >= stayDays))
                    ).Select(
                    x => new View_TravelInsuranceModel
                    {
                        CompanyName = x.CompanyName,
                        Travel_id = x.Travel_id,
                        Age_From = x.Age_From,
                        Age_To = x.Age_To,
                        Plans = x.Plans,
                        Currency = x.Currency,
                        SumInsured = x.SumInsured,
                        CoverType = x.CoverType,
                        TripType = x.TripType,
                        No_Of_Memebers = x.No_Of_Memebers,
                        TermFrom = x.TermFrom,
                        TermTo = x.TermTo,
                        Premium = x.Premium,
                        Plan_ID = x.Plan_ID,
                        PCode = x.PCode,
                        CompanyId = x.CompanyId,
                        Destination = x.Destination,
                        destinationId = x.destinationId,
                        LogoImage = x.LogoImage
                    }).ToList();
                destinations = destinations.Where(x => Convert.ToInt32(x.TermFrom) <= stayDays
                    && Convert.ToInt32(x.TermTo) >= stayDays).OrderBy(x => Convert.ToInt64(x.Premium)).ToList();
                return destinations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<View_TravelInsuranceModel>();

        }
        public string GetBookingid()
        {
            string quoteNo = string.Empty;
            try
            {

                quoteNo = "retail-" + DateTime.Now.ToString("yyyy") + "-" + (new Random()).Next(100000000).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quoteNo;
        }
        public string GettravelId()
        {
            string quoteNo = string.Empty;
            try
            {

                quoteNo = DateTime.Now.ToString("ddMMyyyy") + (new Random()).Next(10).ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quoteNo;
        }

        public List<View_TravelInsuranceModel> GetPakageList(View_TravelInsuranceModel srchTrvlIns)
        {
            try
            {
                int ageSelf = Convert.ToInt32(srchTrvlIns.ageSelf);
                //int stayDays = Convert.ToInt32(srchTremIns.stayDays);
                var destinations = db.tbl_GodigitPakege.AsEnumerable().Where(x =>
                    (Convert.ToInt32(x.AgeFrom) >= ageSelf
                    && Convert.ToInt32(x.Ageto) <= ageSelf)
                    && x.PakageName == srchTrvlIns.destination
                    && x.Suminsured == Convert.ToDecimal(srchTrvlIns.SumInsured)
                    ).Select(
                    x => new View_TravelInsuranceModel
                    {
                        pakegecode = x.PakageCode,
                        SumInsured = Convert.ToDecimal(x.Suminsured)
                    }).ToList();

                return destinations.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<View_TravelInsuranceModel>();
        }

        public List<View_TravelInsuranceModel> GetPremiumGodigitResponce(View_TravelInsuranceModel srchTremIns)
        {
            try
            {
                string geography = "";
                if (srchTremIns.destination == "Asia excluding Japan")
                {
                    geography = "ASA";
                }
                else if (srchTremIns.destination == "World-wide excluding USA and Canada")
                {
                    geography = "ROW";
                }
                else if (srchTremIns.destination == "World-wide including USA and Canada")
                {
                    geography = "UCI";
                }
                int insured1 = 1;
                int insured2 = 0; int insured3 = 0; int insured4 = 0; int insured5 = 0; int insured6 = 0;
                if (srchTremIns.ageSpouse != "0")
                { insured2 = 1; } if (srchTremIns.ageChild1 != "0")
                { insured3 = 1; } if (srchTremIns.ageChild2 != "0")
                { insured4 = 1; } if (srchTremIns.ageFather != "0")
                { insured5 = 1; } if (srchTremIns.ageMother != "0") { insured6 = 1; }
                int insuerp = insured1 + insured2 + insured3 + insured4 + insured5 + insured6;
                string dobcal = DateTime.Today.ToString("yyyy");
                int dob = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.ageSelf);
                int dob1 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.ageSpouse);
                int dob2 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.ageChild1);
                int dob3 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.ageChild2);
                string dateage = dob + "-01-01";
                string dateage1 = dob1 + "-01-01";
                string dateage2 = dob2 + "-01-01";
                string dateage3 = dob3 + "-01-01";
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                var customer = "";
                if (Convert.ToInt32(srchTremIns.ageSelf) > 18 && insuerp != 1)
                {
                    if (insuerp == 2)
                    { customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.City + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' }], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.tripEndDate + "', 'travelStartDate': '" + srchTremIns.tripStartDate + "', 'discountFlag': '' }"; }
                    else if (insuerp == 3)
                    { customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.City + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage2 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' }], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.tripEndDate + "', 'travelStartDate': '" + srchTremIns.tripStartDate + "', 'discountFlag': '' }"; }
                    else if (insuerp == 4)
                    {
                        customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.City + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage2 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage3 + "', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '4', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' } ], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.tripEndDate + "', 'travelStartDate': '" + srchTremIns.tripStartDate + "', 'discountFlag': '' }";
                    }
                }
                else if (Convert.ToInt32(srchTremIns.ageSelf) > 18 && srchTremIns.ageSpouse == "0")
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.City + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + srchTremIns.travellerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '1990-02-02', 'mobile': '" + srchTremIns.Phone + "', 'email': '" + srchTremIns.Email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' } ], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.tripEndDate + "', 'travelStartDate': '" + srchTremIns.tripStartDate + "', 'discountFlag': '' }";
                }
                var response = requestHandler.GodigitResponse(GodigitbaseUrl, customer);

                List<View_TravelInsuranceModel> lst = new List<View_TravelInsuranceModel>();

                //View_TravelInsuranceModel list = new View_TravelInsuranceModel();
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    JObject joResponse = JObject.Parse(responseString);
                    JArray array = (JArray)joResponse["insuredPersons"];
                    string Premium = array[0].ToString();
                    int paxid = 1;
                    JObject joResponse1 = JObject.Parse(Premium);
                    JObject ojObject = (JObject)joResponse1["paymentDetails"];
                    var responce = ojObject.ToString();
                    var result1 = JsonConvert.DeserializeObject<GodigitDetails>(Premium);
                    var result2 = JsonConvert.DeserializeObject<GodigitDetails>(responseString);
                    srchTremIns.bookingId = result2.bookingId;
                    srchTremIns.paxId = result1.paxId;
                    srchTremIns.policyID = result1.policyID;
                    srchTremIns.transactionDate = result1.transactionDate;
                    srchTremIns.policyStatus = result1.policyStatus;
                    srchTremIns.applicationId = result1.applicationId;
                    srchTremIns.paymentLink = result1.paymentLink;
                    var result = JsonConvert.DeserializeObject<GodigitDetails>(responce);
                    srchTremIns.Premium = Convert.ToDecimal(result.policyNetPremium);
                    srchTremIns.policyPremiumd = result.policyPremium;
                    srchTremIns.sgst = result.sgst;
                    srchTremIns.cgst = result.cgst;
                    srchTremIns.igst = result.igst;
                    srchTremIns.invoiceNumber = result.invoiceNumber;
                    srchTremIns.LogoImage = "/Logo/Godigit.png";
                    srchTremIns.Plans = "Go Digit";
                    srchTremIns.CompanyId = 10008;
                    srchTremIns.Travel_id = 0;
                    //srchTremIns.Travel_id = Convert.ToInt64(GettravelId());
                    srchTremIns.SearchId = SaveTravelSearch1(srchTremIns);
                    srchTremIns.SearchId = SaveGodigitResponce(srchTremIns);
                    lst.Add(srchTremIns);
                }
                //  List<View_TravelInsuranceModel> lst1 = new List<View_TravelInsuranceModel>();
                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<View_TravelInsuranceModel>();


        }
        public long SaveGodigitResponce(View_TravelInsuranceModel srchTrvlIns)
        {
            try
            {

                var tDetails = new Godigit_Responce_Data
                {
                    BookingID = srchTrvlIns.SearchId.ToString(),
                    paxId = Convert.ToInt16(srchTrvlIns.paxId),
                    policyPremium = Convert.ToDecimal(srchTrvlIns.policyPremiumd),
                    Sgst = Convert.ToDecimal(srchTrvlIns.sgst),
                    Cgst = Convert.ToDecimal(srchTrvlIns.cgst),
                    Ugst = Convert.ToDecimal(srchTrvlIns.ugst),
                    igst = Convert.ToDecimal(srchTrvlIns.igst),
                    invoiceNumber = srchTrvlIns.invoiceNumber,
                    policyNetPremium = srchTrvlIns.Premium,
                    policyID = srchTrvlIns.policyID,
                    transactionDate = srchTrvlIns.transactionDate,
                    code = Convert.ToInt16(srchTrvlIns.code),
                    message = srchTrvlIns.message,
                    policyStatus = srchTrvlIns.policyStatus,
                    applicationId = srchTrvlIns.applicationId,
                    paymentLink = srchTrvlIns.paymentLink,
                    CompanyID = Convert.ToInt16(srchTrvlIns.CompanyId),
                    PlanName = srchTrvlIns.Plans,
                    Adult = srchTrvlIns.No_Of_Memebers.ToString(),
                    Child = srchTrvlIns.No_Of_Child.ToString()
                };
                db.Godigit_Responce_Data.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.BookingID;

                return Convert.ToInt64(tId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }
        public string PolicyBhartiaxaStatus(GetPaymentStatusBhartiAxa payment)
        {
            try
            {
                var tDetails = new tbl_BhartiAxaPaymentStaus
                {
                    productID = payment.productID,
                    orderNo = payment.orderNo,
                    amount = payment.amount,
                    status = payment.status,
                    transactionRef = payment.transactionRef,
                    policyNo = payment.policyNo,

                    link = payment.link,
                    emailId = payment.emailId

                };
                db.tbl_BhartiAxaPaymentStaus.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.ID;

                return tId.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public string SaveGodigitResponceNew(GetQuotationDetail srchTrvlIns)
        {
            try
            {

                var tDetails = new Godigit_Responce_Data
                {
                    BookingID = srchTrvlIns.searchId1.ToString(),
                    paxId = Convert.ToInt16(srchTrvlIns.paxId),
                    policyPremium = Convert.ToDecimal(srchTrvlIns.policyPremiumd),
                    Sgst = Convert.ToDecimal(srchTrvlIns.sgst),
                    Cgst = Convert.ToDecimal(srchTrvlIns.cgst),
                    Ugst = Convert.ToDecimal(srchTrvlIns.ugst),
                    igst = Convert.ToDecimal(srchTrvlIns.igst),

                    invoiceNumber = srchTrvlIns.invoiceNumber,

                    policyNetPremium = Convert.ToDecimal(srchTrvlIns.policyNetPremium),

                    policyID = srchTrvlIns.policyID,
                    transactionDate = srchTrvlIns.transactionDate,
                    code = Convert.ToInt16(srchTrvlIns.code),
                    message = srchTrvlIns.message,
                    policyStatus = srchTrvlIns.policyStatus,
                    applicationId = srchTrvlIns.applicationId,
                    paymentLink = srchTrvlIns.paymentLink,
                    CompanyID = Convert.ToInt16(srchTrvlIns.CompanyId),
                    PlanName = srchTrvlIns.Plans,
                    Adult = srchTrvlIns.NoOfAdults,
                    Child = srchTrvlIns.NoOfChildren,
                };
                db.Godigit_Responce_Data.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.BookingID;

                return tId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";

        }
        public long SaveTravelSearch1(View_TravelInsuranceModel srchTrvlIns)
        {
            try
            {
                if (srchTrvlIns.ageSpouse == null || srchTrvlIns.ageChild1 == null || srchTrvlIns.ageChild2 == null || srchTrvlIns.ageBrother == null || srchTrvlIns.ageSpouse == null || srchTrvlIns.ageMother == null)
                {
                    srchTrvlIns.ageSpouse = "0";
                    srchTrvlIns.ageChild1 = "0";
                    srchTrvlIns.ageChild2 = "0";
                    srchTrvlIns.ageBrother = "0";
                    srchTrvlIns.ageFather = "0";
                    srchTrvlIns.ageMother = "0";
                }
                var tDetails = new TravelInsuranceSearchBy
                {
                    age = Convert.ToInt16(srchTrvlIns.ageSelf),
                    city = srchTrvlIns.City,
                    contactno = srchTrvlIns.Phone,
                    createdDate = DateTime.UtcNow,
                    destination = srchTrvlIns.destination,
                    emailid = srchTrvlIns.Email,
                    endDate = DateTime.Parse(srchTrvlIns.tripEndDate),

                    startDate = DateTime.Parse(srchTrvlIns.tripStartDate),

                    TravellerName = srchTrvlIns.travellerName,

                    ageSpouse = Convert.ToInt32(srchTrvlIns.ageSpouse),
                    ageChild1 = Convert.ToInt32(srchTrvlIns.ageChild1),
                    ageChild2 = Convert.ToInt32(srchTrvlIns.ageChild2),
                    ageBrother = Convert.ToInt32(srchTrvlIns.ageBrother),
                    ageFather = Convert.ToInt32(srchTrvlIns.ageFather),
                    ageMother = Convert.ToInt32(srchTrvlIns.ageMother)

                };
                db.TravelInsuranceSearchBies.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.SearchId;

                return tId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }
        public long SaveResponceGodigit(SearchTravelInsurance Responcedetail)
        {
            try
            {
                var tDetails = new tbl_GetPremiumResponce
                {

                    accidentalDeathPremium = "",
                    basePremium = "",
                    betterHalfPremium = "",
                    criticalIllnessPremium = "",
                    error = "",
                    hcbPremium = "",
                    pdfName = "",
                    permanentDisabilityPremium = "",
                    premium = "",
                    premiumWaiver = "",
                    sumAssured = "",
                    totalPremium = "",
                    Company = 0,
                    PlanName = "",
                    CustID = 0,
                    CoverAge = "1",

                };
                db.tbl_GetPremiumResponce.Add(tDetails);
                db.SaveChanges();
                var tid = tDetails.ID;
                return Convert.ToInt64(tid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        //.................Godigit...........................
        public GetQuotationDetail GetQuotationDetailGodigit(string planId, string TId, string SumInsuried)
        {
            GetQuotationDetail quotationDetail = new GetQuotationDetail();
            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                string sumin = SumInsuried;
                quotationDetail.CompanyID = 10008;
                var view_Insurance = db.Companies.AsEnumerable().Where(x =>
                  x.CompanyID == quotationDetail.CompanyID).FirstOrDefault();
                var travelInsuranceSearchBies = db.TravelInsuranceSearchBies.AsEnumerable().Where(x => x.SearchId == _TId).FirstOrDefault();
                quotationDetail.proposerName = travelInsuranceSearchBies.TravellerName;
                quotationDetail.email = travelInsuranceSearchBies.emailid;
                quotationDetail.city = travelInsuranceSearchBies.city;
                quotationDetail.mobile = travelInsuranceSearchBies.contactno;
                quotationDetail.age = travelInsuranceSearchBies.age.ToString();
                quotationDetail.age1 = travelInsuranceSearchBies.ageSpouse.ToString();
                quotationDetail.age2 = travelInsuranceSearchBies.ageChild1.ToString();
                quotationDetail.age3 = travelInsuranceSearchBies.ageChild2.ToString();
                string destination = travelInsuranceSearchBies.destination.ToString();
                int AgeSelf = 0; int AgeSOpuse = 0; int AgeChild1 = 0; int AgeChild2 = 0;
                if (quotationDetail.age != "0")
                {
                    AgeSelf = 1;
                }
                if (quotationDetail.age1 != "0")
                {
                    AgeSOpuse = 1;
                }
                if (quotationDetail.age2 != "0")
                {
                    AgeChild1 = 1;
                }
                if (quotationDetail.age3 != "0")
                {
                    AgeChild2 = 1;
                }
                int NoofAdult = AgeSelf + AgeSOpuse;
                quotationDetail.NoOfAdults = NoofAdult.ToString();
                int NoofChild = AgeChild1 + AgeChild2;
                quotationDetail.NoOfChildren = NoofChild.ToString();
                quotationDetail.travelstartdate = Convert.ToDateTime(travelInsuranceSearchBies.startDate).ToString("yyyy-MM-dd");
                quotationDetail.travelenddate = Convert.ToDateTime(travelInsuranceSearchBies.endDate).ToString("yyyy-MM-dd");
                int ageSelf = Convert.ToInt32(quotationDetail.age);
                var destinations = db.tbl_GodigitPakege.AsEnumerable().Where(x =>
                   (Convert.ToInt32(x.AgeFrom) >= ageSelf
                   && Convert.ToInt32(x.Ageto) <= ageSelf)
                   && x.PakageName == destination
                 && x.Suminsured == Convert.ToInt32(SumInsuried)
                   ).Select(
                   x => new View_TravelInsuranceModel
                   {
                       pakegecode = x.PakageCode,
                       SumInsured = Convert.ToDecimal(x.Suminsured),
                       pakageName = x.PakageName

                   }).ToList();
                var commonList = destinations.ToList();
                foreach (var item in commonList)
                {
                    quotationDetail.pakegecode = item.pakegecode;
                    quotationDetail.sumAssured = item.SumInsured.ToString();
                    quotationDetail.Plans = item.pakageName;
                }
                //..........................................
                quotationDetail.searchId = GetGodigitDetails(quotationDetail);

                return quotationDetail;
            }

            catch (Exception ex)
            {
                (new ErrorLog()).Error("getdetail-business=", ex.Message.ToString());
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }
        public string GetGodigitDetails(GetQuotationDetail srchTremIns)
        {
            try
            {
                string geography = "";
                if (srchTremIns.Plans == "Asia excluding Japan")
                {
                    geography = "ASA";
                }
                else if (srchTremIns.Plans == "World-wide excluding USA and Canada")
                {
                    geography = "ROW";
                }
                else if (srchTremIns.Plans == "World-wide including USA and Canada")
                {
                    geography = "UCI";
                }
                int insured1 = 1;
                int insured2 = 0; int insured3 = 0; int insured4 = 0; int insured5 = 0; int insured6 = 0;
                if (srchTremIns.age1 != "0")
                { insured2 = 1; } if (srchTremIns.age2 != "0")
                { insured3 = 1; } if (srchTremIns.age3 != "0")
                { insured4 = 1; }
                //if (srchTremIns.ageFather != "0")
                //{ insured5 = 1; } if (srchTremIns.ageMother != "0") { insured6 = 1; }
                int insuerp = insured1 + insured2 + insured3 + insured4;
                string dobcal = DateTime.Today.ToString("yyyy");
                int dob = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.age);
                int dob1 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.age1);
                int dob2 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.age2);
                int dob3 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTremIns.age3);
                string dateage = dob + "-01-01";
                string dateage1 = dob1 + "-01-01";
                string dateage2 = dob2 + "-01-01";
                string dateage3 = dob3 + "-01-01";
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                srchTremIns.proposerdOB = "01-01-" + dob;
                srchTremIns.insured0dOB = "01-01-" + dob1;
                srchTremIns.insured1dOB = "01-01-" + dob2;
                srchTremIns.insured2dOB = "01-01-" + dob3;
                var customer = "";
                if (insuerp == 1)
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.proposerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.city + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' }], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.travelenddate + "', 'travelStartDate': '" + srchTremIns.travelstartdate + "', 'discountFlag': '' }";
                }
                else if (insuerp == 2)
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.proposerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.city + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' } ], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.travelenddate + "', 'travelStartDate': '" + srchTremIns.travelstartdate + "', 'discountFlag': '' }";
                }
                else if (insuerp == 3)
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.proposerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.city + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage2 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' } ], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.travelenddate + "', 'travelStartDate': '" + srchTremIns.travelstartdate + "', 'discountFlag': '' }";
                }
                else if (insuerp == 4)
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + srchTremIns.proposerName + "', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': '" + srchTremIns.city + "', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D89U0112' } ], 'dob': '" + dateage + "', 'mobile': '9353568667', 'email': 'Bindu.S@godigit.com', 'type': 'N' }, 'insuredPersons': [ { 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage1 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage2 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' },{ 'firstName': 'Neha', 'lastName': '', 'address': { 'line1': 'N.W. College', 'line2': '', 'line3': '', 'state': '27', 'city': 'Pune', 'country': 'IND', 'pinCode': '411001' }, 'documents': [ { 'type': 'PPT', 'id': 'D890112' } ], 'dob': '" + dateage3 + "', 'mobile': '" + srchTremIns.mobile + "', 'email': '" + srchTremIns.email + "', 'profession': '', 'paxId': '4', 'nominee': { 'relationship': 'S', 'name': 'Uduwei Uhdewui' }, 'type': 'N', 'gender': 'ML' } ], 'insuredPersonsCount': '" + insuerp + "', 'bookingDate': '" + date + "', 'packageName': '" + srchTremIns.pakegecode + "', 'geography': '" + geography + "', 'travelEndDate': '" + srchTremIns.travelenddate + "', 'travelStartDate': '" + srchTremIns.travelstartdate + "', 'discountFlag': '' }";
                }
                var response = requestHandler.GodigitResponse(GodigitbaseUrl, customer);

                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    JObject joResponse = JObject.Parse(responseString);
                    JArray array = (JArray)joResponse["insuredPersons"];
                    string Premium = array[0].ToString();
                    int paxid = 1;
                    JObject joResponse1 = JObject.Parse(Premium);
                    JObject ojObject = (JObject)joResponse1["paymentDetails"];
                    var responce = ojObject.ToString();
                    var result1 = JsonConvert.DeserializeObject<GodigitDetails>(Premium);
                    var result2 = JsonConvert.DeserializeObject<GodigitDetails>(responseString);
                    srchTremIns.bookingId = result2.bookingId;
                    srchTremIns.paxId = result1.paxId;
                    srchTremIns.policyID = result1.policyID;
                    srchTremIns.transactionDate = result1.transactionDate;
                    srchTremIns.policyStatus = result1.policyStatus;
                    srchTremIns.applicationId = result1.applicationId;
                    srchTremIns.paymentLink = result1.paymentLink;
                    var result = JsonConvert.DeserializeObject<GodigitDetails>(responce);
                    srchTremIns.totalPremium = result.policyPremium;
                    srchTremIns.policyPremiumd = result.policyPremium;
                    srchTremIns.policyNetPremium = result.policyNetPremium;
                    srchTremIns.sgst = result.sgst;
                    srchTremIns.cgst = result.cgst;
                    srchTremIns.invoiceNumber = result.invoiceNumber;
                    srchTremIns.LogoImage = "/Logo/Godigit.png";
                    srchTremIns.Plans = srchTremIns.Plans;
                    srchTremIns.CompanyId = 10008;
                    srchTremIns.Travel_id = 0;
                    //   srchTremIns.searchId = SaveGodigitResponceNew(srchTremIns);
                    //lst.Add(srchTremIns);
                }
                //  List<View_TravelInsuranceModel> lst1 = new List<View_TravelInsuranceModel>();
                var tid = "0";
                return tid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        public GetQuotationDetail GetQuotationDetail(string planId, string TId, string compid)
        {
            GetQuotationDetail quotationDetail = new GetQuotationDetail();
            Star_ProposalCreation star_ProposalCreation = new Star_ProposalCreation();

            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                var travelInsuranceSearchBies = db.TravelInsuranceSearchBies.AsEnumerable().Where(x => x.SearchId == _TId).FirstOrDefault();
                if (compid == "10004")
                {
                    var viewBhartiaxa = db.Godigit_Responce_Data.AsEnumerable().Where(x =>
                    x.BookingID == Convert.ToDouble(_planId).ToString()).LastOrDefault();
                    quotationDetail = new GetQuotationDetail
                    {
                        sumAssured = viewBhartiaxa.invoiceNumber.ToString(),
                        // Amount = view_Insurance.Premium.ToString(),
                        Amount = "1",
                        NoOfAdults = viewBhartiaxa.Adult,
                        NoOfChildren = viewBhartiaxa.Child,
                        Plan = viewBhartiaxa.PlanName,
                        CompanyID = Convert.ToInt32(viewBhartiaxa.CompanyID),
                        travelenddate = Convert.ToDateTime(travelInsuranceSearchBies.endDate).ToString("yyyy-MM-dd"),
                        travelstartdate = Convert.ToDateTime(travelInsuranceSearchBies.startDate).ToString("yyyy-MM-dd"),
                        totalPremium = Convert.ToInt32(viewBhartiaxa.policyNetPremium).ToString(),
                        serviceTax = Convert.ToInt32(viewBhartiaxa.Sgst).ToString(),
                        policyID = viewBhartiaxa.policyID,
                        applicationId = viewBhartiaxa.applicationId,
                    };
                    quotationDetail.PlanId = viewBhartiaxa.BookingID.ToString();

                }
                else
                {
                    var view_Insurance = db.View_TravelInsurance.AsEnumerable().Where(x =>
                   x.Travel_id == _planId).FirstOrDefault();



                    //      BindAssigneeNominee("travel", Convert.ToInt32(view_Insurance.CompanyId));
                    switch (view_Insurance.CompanyId)
                    {

                        case 10004:
                            quotationDetail = new GetQuotationDetail
                            {
                                sumAssured = view_Insurance.SumInsured.ToString(),
                                 Amount = view_Insurance.Premium.ToString(),
                               // Amount = "1",
                                NoOfAdults = "1",
                                NoOfChildren = "0",
                                Plan = view_Insurance.Plans,
                                CompanyID = Convert.ToInt32(view_Insurance.CompanyId),
                                travelenddate = Convert.ToDateTime(travelInsuranceSearchBies.endDate).ToString("yyyy-MM-dd"),
                                travelstartdate = Convert.ToDateTime(travelInsuranceSearchBies.startDate).ToString("yyyy-MM-dd"),
                            };
                            break;
                        case 10005:
                            var travellerDoB = DateTime.Now.AddYears(Convert.ToInt32("-" + travelInsuranceSearchBies.age.ToString())).ToString("MMMM dd, yyyy").Replace("-", "/");
                            var insuredList = new List<Insured>();
                            insuredList.Add(new Insured
                            {
                                dob = travellerDoB,
                                sumInsuredId = 0
                            });
                            star_ProposalCreation = new Star_ProposalCreation
                            {

                                planId = Convert.ToInt32(view_Insurance.Plan_ID.ToString()),
                                travelStartOn = Convert.ToDateTime(travelInsuranceSearchBies.startDate).ToString("MMMM dd, yyyy").Replace("-", "/"),
                                travelEndOn = Convert.ToDateTime(travelInsuranceSearchBies.endDate).ToString("MMMM dd, yyyy").Replace("-", "/"),
                                /// PolicyTypeName = view_Insurance.Plans.ToString(),
                                insureds = insuredList
                            };
                            string postData = JsonConvert.SerializeObject(star_ProposalCreation);
                            var response = requestHandler.Response(baseUrl + "/proposal/premium/calculate", postData);
                            if (response["Status"] == "200")
                            {
                                var responseString = response["response"].ToString();
                                var result = JsonConvert.DeserializeObject<StartTravelPremiumAPIResponse>(responseString);
                                quotationDetail = new GetQuotationDetail
                                {
                                    sumAssured = view_Insurance.SumInsured.ToString(),
                                    Amount = result.premium.ToString(),
                                    PlanId = view_Insurance.Plan_ID.ToString(),
                                    serviceTax = result.serviceTax.ToString(),
                                    totalPremium = result.totalPremium.ToString(),
                                    NoOfAdults = view_Insurance.No_Of_Memebers.ToString(),
                                    NoOfChildren = "0",
                                    Plan = view_Insurance.Plans,
                                    travelenddate = Convert.ToDateTime(travelInsuranceSearchBies.endDate).ToString("dd/MM/yyyy"),
                                    travelstartdate = Convert.ToDateTime(travelInsuranceSearchBies.startDate).ToString("dd/MM/yyyy"),
                                    CompanyID = Convert.ToInt32(view_Insurance.CompanyId)
                                };
                            }
                            else
                            {
                                // error response handle code
                            }
                            break;
                        default:
                            break;
                    }
                }

                quotationDetail.Channel = "cypStr";
                quotationDetail.OrderNo = "";
                quotationDetail.QuoteNo = "";
                quotationDetail.IsMobile = "N";
                quotationDetail.Product = "STR";
                quotationDetail.searchId = TId;
                //  quotationDetail.PlanId = view_Insurance.Plan_ID.ToString();
                quotationDetail.proposerName = travelInsuranceSearchBies.TravellerName;
                quotationDetail.email = travelInsuranceSearchBies.emailid;
                quotationDetail.city = travelInsuranceSearchBies.city;
                quotationDetail.mobile = travelInsuranceSearchBies.contactno;
                return quotationDetail;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error("getdetail-business=", ex.Message.ToString());
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }

        public string SubmitProposal(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                getQuotationDetail.PlanId = getQuotationDetail.PlanId == "" ? "0" : getQuotationDetail.PlanId;
                getQuotationDetail.proposerdOB = Convert.ToDateTime(getQuotationDetail.proposerdOB).ToString("dd-MM-yyyy").Replace("-", "/");
                getQuotationDetail.PassportExpiry = Convert.ToDateTime(getQuotationDetail.PassportExpiry).ToString("dd-MM-yyyy").Replace("-", "/");
                getQuotationDetail.dOB = Convert.ToDateTime(getQuotationDetail.dOB).ToString("dd-MM-yyyy").Replace("-", "/");

                var starProposalCreateRequest = new StarProposalCreateRequest
                {

                    eiaNumber = getQuotationDetail.eiaNumber ?? "",
                    gstIdNumber = "",//getQuotationDetail.gstIdNumber,
                    gstTypeId = "",//getQuotationDetail.gstTypeId,
                    physicianContactNumber = getQuotationDetail.physicianContactNumber,
                    physicianName = getQuotationDetail.physicianName,
                    placeOfVisit = getQuotationDetail.placeOfVisit, //"CHN"
                    planId = Convert.ToInt64(getQuotationDetail.PlanId),
                    proposerAddressOne = getQuotationDetail.address == "" ? "Lucknow" : getQuotationDetail.address,

                    proposerAreaId = string.IsNullOrEmpty(getQuotationDetail.proposerAreaId) ? "0" : getQuotationDetail.proposerAreaId,
                    proposerDob = DateTime.ParseExact(getQuotationDetail.proposerdOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy"),
                    proposerEmail = getQuotationDetail.email,
                    proposerName = getQuotationDetail.proposerName,
                    proposerPhone = getQuotationDetail.mobile,
                    travelDeclaration = getQuotationDetail.travelDeclaration == true ? 1 : 0,
                    travelEndOn = DateTime.ParseExact(getQuotationDetail.travelenddate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy"),
                    travelPurposeId = Convert.ToInt32(getQuotationDetail.travelPurposeId),
                    travelStartOn = DateTime.ParseExact(getQuotationDetail.travelstartdate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy"),
                    proposerAddressTwo = getQuotationDetail.address2 == "" ? "Lucknow" : getQuotationDetail.address2,


                };
                starProposalCreateRequest.insureds = new InsuredPerson
                {
                    assigneeName = getQuotationDetail.assigneeName ?? "abc",
                    assigneeRelationshipId = getQuotationDetail.assigneeRelationshipId ?? "1",
                    dob = DateTime.ParseExact(getQuotationDetail.dOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy"),
                    illness = getQuotationDetail.Illness ?? "None",
                    name = getQuotationDetail.insuredPersonName,
                    passportExpiry = DateTime.ParseExact(getQuotationDetail.PassportExpiry, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy"),
                    passportNumber = getQuotationDetail.passportNo,
                    relationshipId = Convert.ToInt32(getQuotationDetail.Relation.Replace(",", "")),
                    sex = getQuotationDetail.gender,
                    visaType = getQuotationDetail.visaType


                };
                string postData = JsonConvert.SerializeObject(starProposalCreateRequest);
                var response = requestHandler.Response(baseUrl + "/policy/proposals", postData.Replace("insureds", "insureds[0]"));
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    var result = JsonConvert.DeserializeObject<TravelProposalDetail>(responseString);
                    var proposalDetail = new TravelProposalDetail
                    {
                        BilledDate = result != null ? result.BilledDate : Convert.ToDateTime("01-01-1900"),
                        FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                        IsBillPayed = result != null ? true : false,
                        ReferenceId = result != null ? result.ReferenceId : "",

                    };
                    db.TravelProposalDetails.Add(proposalDetail);
                    db.SaveChanges();
                    return proposalDetail.ReferenceId;
                }
                else
                {
                    // Code for handle error response
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        //public string GetDataGodigit()
        //{
        //    var postdata = new { APIKEY = apiKeyGodigit, referenceId = GodigitbaseUrl };
        //    string postDataJSON = JsonConvert.SerializeObject(postdata);
        //    var response = requestHandler.GenerateToken(postDataJSON, GodigitbaseUrl);
        //    if (response["Status"] == "200")
        //    {
        //        var responseString = response["response"].ToString();

        //        var result = JsonConvert.DeserializeObject<ResponseTokenService>(responseString);
        //        if (result != null)
        //        {
        //            return result.redirectToken;
        //        }
        //    }
        //    else
        //    {

        //    }
        //    return "";
        //}
        public string GenerateToken(string refId)
        {
            try
            {
                var postdata = new { APIKEY = apiKey, SECRETKEY = secretKey, referenceId = refId };
                string postDataJSON = JsonConvert.SerializeObject(postdata);
                var response = requestHandler.GenerateToken(postDataJSON, refId);
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();

                    var result = JsonConvert.DeserializeObject<ResponseTokenService>(responseString);
                    if (result != null)
                    {
                        return result.redirectToken;
                    }
                }
                else
                {

                }
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        public Dictionary<string, string> PolicyStatus(string purchaseToken)
        {
           
                Dictionary<string, string> dicObj = new Dictionary<string, string>();
            try{
                var postdata = new { APIKEY = apiKey, SECRETKEY = secretKey, purchaseToken = purchaseToken };
                string postDataJSON = JsonConvert.SerializeObject(postdata);
                var response = requestHandler.PurchaseStatus(postDataJSON, purchaseToken);
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    ResponsePurchaseToken resultPurchaseToken = JsonConvert.DeserializeObject<ResponsePurchaseToken>(response["response"]);
                    if (resultPurchaseToken != null)
                    {
                        dicObj.Add("PolicyStatus", resultPurchaseToken.status);
                        dicObj.Add("RefId", resultPurchaseToken.referenceId);
                        var postdataPolicy = new { APIKEY = apiKey, SECRETKEY = secretKey, referenceId = resultPurchaseToken.referenceId };
                        postDataJSON = JsonConvert.SerializeObject(postdataPolicy);
                        var responsePolicyStatus = requestHandler.PolicyStatus(postDataJSON, resultPurchaseToken.referenceId);
                        if (responsePolicyStatus["Status"] == "200")
                        {
                            var responseStringPolicyStatus = responsePolicyStatus["response"].ToString();

                            if (responseStringPolicyStatus.Contains("Note"))
                            {
                                var result = JsonConvert.DeserializeObject(responseStringPolicyStatus);
                                dicObj.Add("Note", result.ToString());

                            }
                            else
                            {
                                var result = JsonConvert.DeserializeObject<ResponsePolicyStatus>(responseString.Replace("Reference Number", "Reference_Number").Replace("Policy Number", "Policy_Number"));
                                if (result != null)
                                {
                                    dicObj.Add("RefNumber", result.Reference_Number);
                                    dicObj.Add("PolicyNumber", result.Policy_Number);
                                }
                            }
                        }
                        else
                        {

                        }

                    }
                }
                else
                {
                    // Code for handle error response
                }
                return dicObj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dicObj;
        }

        public byte[] GetDocument(string refId)
        {
            // WebResponse WebResponse;
            if (!string.IsNullOrEmpty(refId))
            {
                var postdata = new { APIKEY = apiKey, SECRETKEY = secretKey, referenceId = refId };
                string postDataJSON = JsonConvert.SerializeObject(postdata);
                return requestHandler.PolicySchedule(postDataJSON, refId);
            }

            return new byte[1];
        }
        public List<customSelectList> GetPlaceOfVisit(string compid)
        {
            try
            {
                if (compid == "10005")
                {
                    var bannedCountry = db.tblBannedCountries.Select(x => x.CODE).ToList();
                    var POVList = db.tbl_StarCountyList.AsQueryable().Where(x => !bannedCountry.Any(y => y.Equals(x.Code))).Select(x => new customSelectList { text = x.Code, value = x.Country }).Distinct().ToList();
                    return POVList;
                }
                else if (compid == "10004")
                {
                    var bannedCountry = db.tbl_BhartiAxaBanned_Country.Select(x => x.Country).ToList();
                    var POVList = db.tbl_Country.AsQueryable().Where(x => !bannedCountry.Any(y => y.Equals(x.County))).Select(x => new customSelectList { text = x.Code, value = x.County }).Distinct().ToList();
                    return POVList;
                }
                return new List<customSelectList>();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<Area> GetArea(string pinCode, string cityId)
        {
            var areaList = new List<Area>();
            areaList.Add(new Area { areaID = 0, areaName = "Select" });
            try
            {
                if (!string.IsNullOrEmpty(pinCode))
                {
                    var responseArea = requestHandler.GetAreaId(pinCode, cityId);

                    if (responseArea["Status"] == "200")
                    {
                        var responseString = responseArea["response"].ToString();
                        //if (responseString.Contains("Note"))
                        //{
                        //    var result = JsonConvert.DeserializeObject(responseString);
                        //    dicObj.Add("Note", result.ToString());

                        //}
                        //else
                        //{
                        var result = JsonConvert.DeserializeObject<AreaList>(responseString);
                        areaList.AddRange(result.area);
                        return areaList;
                        //}
                    }
                    else
                    {
                        return areaList;
                        // Code for handle error response
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return areaList;
        }

        public string SubmitProposalGodigit(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                //var getGeo = db.tbl_GodigitCounty.AsEnumerable().Where(x =>
                //  x.CountryID == getQuotationDetail.Country).LastOrDefault();
                string geography = "";
                if (getQuotationDetail.Plans == "Asia excluding Japan")
                {
                    getQuotationDetail.code = "ASA";
                }
                else if (getQuotationDetail.Plans == "World-wide excluding USA and Canada")
                {
                    getQuotationDetail.code = "ROW";
                }
                else if (getQuotationDetail.Plans == "World-wide including USA and Canada")
                {
                    getQuotationDetail.code = "UCI";
                }




                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string Dob = DateTime.ParseExact(getQuotationDetail.dOB, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                string proposerdOB = DateTime.ParseExact(getQuotationDetail.proposerdOB, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                string proposerdOB1 = ""; string proposerdOB2 = ""; string proposerdOB3 = "";
                if (getQuotationDetail.NoOfAdults == "2")
                {
                    proposerdOB1 = DateTime.ParseExact(getQuotationDetail.insured0dOB, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                if (getQuotationDetail.NoOfChildren == "1")
                {
                    proposerdOB2 = DateTime.ParseExact(getQuotationDetail.insured1dOB, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                if (getQuotationDetail.NoOfChildren == "2")
                {
                    proposerdOB3 = DateTime.ParseExact(getQuotationDetail.insured2dOB, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                }
                var customer = "";
                if (getQuotationDetail.NoOfAdults == "1")
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + proposerdOB + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + Dob + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': '" + getQuotationDetail.assigneeRelationshipId + "', 'name': '" + getQuotationDetail.NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.GodigitPgender + "' }], 'insuredPersonsCount': '1', 'bookingDate': '" + date + "', 'packageName': '" + getQuotationDetail.pakegecode + "', 'geography': '" + getQuotationDetail.code + "', 'travelEndDate': '" + getQuotationDetail.travelenddate + "', 'travelStartDate': '" + getQuotationDetail.travelstartdate + "', 'discountFlag': '' ,'countryTravel':'" + getQuotationDetail.Country + "'}";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "0")
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + proposerdOB + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + Dob + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': '" + getQuotationDetail.assigneeRelationshipId + "', 'name': '" + getQuotationDetail.NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.GodigitPgender + "' },{ 'firstName': '" + getQuotationDetail.insured0insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured0passportNo + "' } ], 'dob': '" + proposerdOB1 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': '" + getQuotationDetail.insured0assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured0NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured0gender + "' }], 'insuredPersonsCount': '2', 'bookingDate': '" + date + "', 'packageName': '" + getQuotationDetail.pakegecode + "', 'geography': '" + getQuotationDetail.code + "', 'travelEndDate': '" + getQuotationDetail.travelenddate + "', 'travelStartDate': '" + getQuotationDetail.travelstartdate + "', 'discountFlag': '' ,'countryTravel':'" + getQuotationDetail.Country + "'}";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "1")
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + proposerdOB + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + Dob + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': '" + getQuotationDetail.assigneeRelationshipId + "', 'name': '" + getQuotationDetail.NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.GodigitPgender + "' },{ 'firstName': '" + getQuotationDetail.insured0insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured0passportNo + "' } ], 'dob': '" + proposerdOB1 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': '" + getQuotationDetail.insured0assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured0NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured0gender + "' },{ 'firstName': '" + getQuotationDetail.insured1insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured1passportNo + "' } ], 'dob': '" + proposerdOB2 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': '" + getQuotationDetail.insured1assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured1NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured1gender + "' }], 'insuredPersonsCount': '3', 'bookingDate': '" + date + "', 'packageName': '" + getQuotationDetail.pakegecode + "', 'geography': '" + getQuotationDetail.code + "', 'travelEndDate': '" + getQuotationDetail.travelenddate + "', 'travelStartDate': '" + getQuotationDetail.travelstartdate + "', 'discountFlag': '' ,'countryTravel':'" + getQuotationDetail.Country + "'}";
                }
                else if (getQuotationDetail.NoOfAdults == "2" && getQuotationDetail.NoOfChildren == "2")
                {
                    customer = "{ 'bookingId': '" + GetBookingid() + "', 'type': 'I', 'purpose': 'L', 'customer': { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + proposerdOB + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'type': 'N' }, 'insuredPersons': [ { 'firstName': '" + getQuotationDetail.proposerName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.passportNo + "' } ], 'dob': '" + Dob + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '1', 'nominee': { 'relationship': '" + getQuotationDetail.assigneeRelationshipId + "', 'name': '" + getQuotationDetail.NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.GodigitPgender + "' },{ 'firstName': '" + getQuotationDetail.insured0insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured0passportNo + "' } ], 'dob': '" + proposerdOB1 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '2', 'nominee': { 'relationship': '" + getQuotationDetail.insured0assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured0NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured0gender + "' },{ 'firstName': '" + getQuotationDetail.insured1insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured1passportNo + "' } ], 'dob': '" + proposerdOB2 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '3', 'nominee': { 'relationship': '" + getQuotationDetail.insured1assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured1NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured1gender + "' },{ 'firstName': '" + getQuotationDetail.insured2insuredPersonName + "', 'lastName': '', 'address': { 'line1': '" + getQuotationDetail.address + "', 'line2': '', 'line3': '', 'state': '" + getQuotationDetail.State + "', 'city': '" + getQuotationDetail.city + "', 'country': 'IND', 'pinCode': '" + getQuotationDetail.pinCode + "' }, 'documents': [ { 'type': 'PPT', 'id': '" + getQuotationDetail.insured2passportNo + "' } ], 'dob': '" + proposerdOB3 + "', 'mobile': '" + getQuotationDetail.mobile + "', 'email': '" + getQuotationDetail.email + "', 'profession': '', 'paxId': '4', 'nominee': { 'relationship': '" + getQuotationDetail.insured2assigneeRelationshipId + "', 'name': '" + getQuotationDetail.insured2NomineeName + "' }, 'type': 'N', 'gender': '" + getQuotationDetail.insured2gender + "' } ], 'insuredPersonsCount': '4', 'bookingDate': '" + date + "', 'packageName': '" + getQuotationDetail.pakegecode + "', 'geography': '" + getQuotationDetail.code + "', 'travelEndDate': '" + getQuotationDetail.travelenddate + "', 'travelStartDate': '" + getQuotationDetail.travelstartdate + "', 'discountFlag': '' ,'countryTravel':'" + getQuotationDetail.Country + "'}";
                }

                getQuotationDetail.paymentid = "paymentlink";
                customer = customer.Replace("'", "\"");
                var response = requestHandler.GodigitResponse(GodigitbaseUrl, customer);

                if (response["Status"] == "200")
                {
                    GetQuotationDetail srchTremIns = new GetQuotationDetail();
                    var responseString = response["response"].ToString();
                    JObject joResponse = JObject.Parse(responseString);
                    JArray array = (JArray)joResponse["insuredPersons"];
                    string Premium = array[0].ToString();
                    int paxid = 1;
                    JObject joResponse1 = JObject.Parse(Premium);
                    JObject ojObject = (JObject)joResponse1["paymentDetails"];
                    var responce = ojObject.ToString();
                    var result1 = JsonConvert.DeserializeObject<GodigitDetails>(Premium);
                    var result2 = JsonConvert.DeserializeObject<GodigitDetails>(responseString);
                    getQuotationDetail.bookingId = result2.bookingId;
                    getQuotationDetail.paxId = result1.paxId;
                    getQuotationDetail.policyID = result1.policyID;
                    getQuotationDetail.transactionDate = result1.transactionDate;
                    getQuotationDetail.policyStatus = result1.policyStatus;
                    getQuotationDetail.applicationId = result1.applicationId;
                    getQuotationDetail.paymentLink = result1.paymentLink;
                    var result = JsonConvert.DeserializeObject<GodigitDetails>(responce);
                    getQuotationDetail.totalPremium = result.policyNetPremium;
                    getQuotationDetail.policyPremiumd = result.policyPremium;
                    //  getQuotationDetail.policyPremiumd = "1";
                    getQuotationDetail.sgst = result.sgst;
                    getQuotationDetail.cgst = result.cgst;
                    getQuotationDetail.igst = result.igst;
                    getQuotationDetail.invoiceNumber = result.invoiceNumber;
                    getQuotationDetail.LogoImage = "/Logo/Godigit.png";
                    getQuotationDetail.Plans = "Go Digit";
                    getQuotationDetail.CompanyId = 10008;
                    getQuotationDetail.Travel_id = 0;
                    getQuotationDetail.policyNetPremium = result.policyPremium;
                    getQuotationDetail.searchId = SaveGodigitResponceNew(srchTremIns);


                }

                else
                {
                    // Code for handle error response
                }
                return getQuotationDetail.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        /// //pppppppppppppppppppppppppppppp
        public String PolicyPay(string transactionNumber)
        {
            try
            {
                Dictionary<string, string> dicObj = new Dictionary<string, string>();
                string data = "";
                var postdata = new { transactionNumber = transactionNumber };
                string postDataJSON = JsonConvert.SerializeObject(postdata);
                var response = requestHandler.GodigitPayment(transactionNumber);
                var responseString = response["response"].ToString().Replace("[", "").Replace("]", "");
                if (response["Status"] == "200")
                {
                    var result = JsonConvert.DeserializeObject<GodigitResponceDetails>(responseString);
                    dicObj.Add("applicationNumber", result.applicationNumber);
                    data = result.applicationNumber;



                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        public string GetPolicyPDf(GodigitDetails policynumber)
        {
            try
            {
                var url = "https://preprod-pdfservice.godigit.com/PolicyDocumentGeneration/PDFGeneration/v1/rest/digit/generatePolicyDocument";
                var policydata = "{'policyNumber':'" + policynumber.policyID + "','policyId':'','productId':''}";
                var responce = requestHandler.GodigitPolicyPDF(url, policydata);
                if (responce["Status"] == "200")
                {

                    var responseString = responce["response"].ToString();
                    var result2 = JsonConvert.DeserializeObject<GodigitDetails>(responseString);
                    policynumber.schedulePath = result2.schedulePath;
                    policynumber.message = "success";
                }
                else
                {
                    policynumber.message = "Fail";
                }
                return policynumber.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
        /// //pppppppppppppppppppppppppppppp
        public List<AllCompanyDetails> GetAllCompanyDetails(AllCompanyDetails objuserlhealth)
        {
            try
            {


                var data = db.All_Company_Details.AsEnumerable().Select(
                     x => new AllCompanyDetails
                     //var data = db.All_Company_Details.Where(x => x.Company_Id == objuserlhealth.Company_Id)
                     //.Select(x => new AllCompanyDetails
                     {
                         Id = x.Id,
                         Benefit = x.Benefit,
                         Co_Pay = x.Co_Pay,
                         Company_Id = x.Company_Id,
                         Company_Plan = x.Company_Plan,
                         Day_Care_Treatment = x.Day_Care_Treatment,
                         Post_Hospitalization = x.Post_Hospitalization,
                         Pre_Hospitalization = x.Pre_Hospitalization,
                         Restoration_Benefit = x.Restoration_Benefit,
                         Room_Rent = x.Room_Rent,
                         Ambulance_Charges = x.Ambulance_Charges,
                         Status = x.Status,

                     }).ToList();

                return data;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }



            return new List<AllCompanyDetails>();

        }
        //.........For Compare Data..........................
        public List<CompareDetails> GetCompareDetails(CompareDetails objuserlhealth)
        {
            try
            {


                var data = db.Comparision_Health_tbl.AsEnumerable().Select(
                 x => new CompareDetails

                 {

                     Benefit = x.Benefit,
                     Co_Pay = x.Co_Pay,
                     Room_Rent = x.Room_Rent,
                     OPD = x.OPD,
                     Day_Care_Treatment = x.Day_Care_Treatment,
                     Medical_Checkup = x.Medical_Checkup,
                     Pre_Existing_Disease_Covered_After = x.Pre_Existing_Disease_Covered_After,
                     Domicilliary_Expenses = x.Domicilliary_Expenses,
                     Organ_Donar_Expenses = x.Organ_Donar_Expenses,
                     Hospital_Cash_Daily_Limit = x.Hospital_Cash_Daily_Limit,
                     Maternity_Benefit = x.Maternity_Benefit,
                     New_Born_Baby = x.New_Born_Baby,
                     Pre_Hospitalization = x.Pre_Hospitalization,
                     Post_Hospitalization = x.Post_Hospitalization,
                     Ambulance_Charges = x.Ambulance_Charges,
                     Health_Check_Up = x.Health_Check_Up,
                     Restoration_Benefit = x.Restoration_Benefit,
                     Free_Look_Period = x.Free_Look_Period,
                     Company_Id = x.Company_Id,
                     Company_Plan = x.Company_Plan,
                     Status = x.Status
                 }).ToList();

                return data;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<CompareDetails>();

        }
        //..........End Details...................
    }
}
