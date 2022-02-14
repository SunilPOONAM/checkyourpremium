
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;
using Domain;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Business
{
    public interface IHealthBusiness
    {
        List<destinationList> GetAllCity();
        List<destinationList> GetAllCoverAge();
        List<destinationList> GetAllYear();
        List<destinationList> GetBlockCity(string pinCode);
        List<destinationList> GetBajajRelation();//pppppppppppppppppppppp
        List<destinationList> GetBajajOccupation();//ppppppppppppppppppppppp
        List<View_HealthinsuranceModel> GetHealthPremiumList(HealthPlanDetails srchHealthIns);
        List<View_HealthinsuranceModel> GetPremiumList(View_HealthinsuranceModel srchTrvlIns);
        long SaveHealthSearch(HealthPlanDetails srchHealthlIns);
        //long SavePolicy(PolicyStatusDetails srchHealthlIns);
        RegistrationDetails GetQuotationDetail(string planId, string TId);
        RegistrationDetails GetQuotationBsjajDetail(string planId, string TId, string QuoteNo);
        List<AllCompanyDetails> GetAllCompanyDetails(AllCompanyDetails objuserlhealth);
        List<CompareDetails> GetCompareDetails(CompareDetails objuserlhealth);
        string SubmitProposal(RegistrationDetails getQuotationDetail);
        string GenerateToken(string refId);
        Dictionary<string, string> PolicyStatus(string purchaseToken);
        byte[] GetDocument(string refId);
        List<customSelectList> GetRelations(string planId);
        List<customSelectList> GetOccupation(string planId);
        List<Area> GetArea(string pinCode, string cityId);
        //List<string> GetCityList(string pinCode);
        List<City> GetCityList(string pinCode);
        List<customSelectList> GetAssigneeNominee(string type, Int32 planid);

    }

    public class HealthBusiness : IHealthBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private string baseUrl; string secretKey = string.Empty; string apiKey = string.Empty;
        private string bajajurl;
        string BajajUser = string.Empty;
        string BajajPassword = string.Empty; string bajajurlpro = string.Empty;
        private RequestHandler requestHandler = new RequestHandler();


        public HealthBusiness()
        {
            db = new CheckyourpremiumliveEntities();
            baseUrl = System.Configuration.ConfigurationSettings.AppSettings["StarBaseURL"].ToString();
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
            bajajurl = System.Configuration.ConfigurationSettings.AppSettings["BajajHealth"].ToString();
            bajajurlpro = System.Configuration.ConfigurationSettings.AppSettings["BajajHealthpro"].ToString();
            BajajUser = System.Configuration.ConfigurationSettings.AppSettings["BajajUser"].ToString();
            BajajPassword = System.Configuration.ConfigurationSettings.AppSettings["BajajPassword"].ToString();
        }
        public List<customSelectList> GetRelations(string planId)
        {
            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                //  Int64 _TId = Convert.ToInt64(TId);
                var view_Insurance = db.View_HealthChartData.AsEnumerable().Where(x =>
                    x.PremiumChartID == _planId).FirstOrDefault();

                var purposeListAll = db.tblRelationMasters.AsQueryable().AsEnumerable().Where(x => x.RelationType == (view_Insurance == null ? "" : view_Insurance.PremiumDesc)).Distinct().ToList();
                var purposeList = new List<customSelectList>();
                if (planId != "0")
                {

                    purposeList = purposeListAll.Select(x => new customSelectList { text = x.RelationCode.ToString(), value = x.Relation }).Distinct().ToList();
                }
                else
                {
                    purposeList = purposeListAll.Select(x => new customSelectList { text = x.Relation.ToString(), value = x.Relation }).Distinct().ToList();
                }
                return purposeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        //poonam..............................
        public List<destinationList> GetBajajRelation()
        {
            try
            {
                var Coverage = db.Bajaj_Relation.AsQueryable().AsEnumerable().Select(x => x.Relation.ToString()).Distinct().ToList().Select(x => new destinationList { text = x, value = x }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();
        }
        public List<destinationList> GetBajajOccupation()
        {
            try
            {
                var Coverage = db.Bajaj_Occupation.AsQueryable().AsEnumerable().Select(x => new destinationList { text = x.OccupationId.ToString(), value = x.OccupationValue }).Distinct().ToList();
                //.Select(x => x.OccupationValue.ToString()).Distinct().ToList().Select(x => new destinationList { text = x, value = x }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();
        }
        //End....................
        public List<customSelectList> GetOccupation(string planId)
        {
            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                //  Int64 _TId = Convert.ToInt64(TId);
                var view_Insurance = db.View_HealthChartData.AsEnumerable().Where(x =>
                    x.PremiumChartID == _planId).FirstOrDefault();
                var purposeList = db.tbl_OccupationMaster.AsQueryable().AsEnumerable().Where(x => x.OType == view_Insurance.PremiumDesc).Select(x => new customSelectList { text = x.OccupationCode.ToString(), value = x.Occupation }).Distinct().ToList();
                return purposeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<destinationList> GetAllCity()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var Cities = db.tblCities.AsQueryable().Select(x => new destinationList { text = x.City_Name, value = x.City_Name }).Distinct().ToList();
                return Cities;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();

        }
        public List<destinationList> GetAllCoverAge()
        {
            try
            {
                var Coverage = db.View_HealthChartData.AsQueryable().AsEnumerable().Where(x => Convert.ToInt64(x.SumInsured) != 0).Select(x => new { SumInsured = Convert.ToInt64(x.SumInsured).ToString() }).Distinct().ToList().OrderBy(x => Convert.ToInt64(x.SumInsured)).Select(x => new destinationList { text = x.SumInsured, value = x.SumInsured }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();
        }
        public List<destinationList> GetAllYear()
        {
            try
            {
                var Coverage = db.View_HealthChartData.AsQueryable().AsEnumerable().Select(x => x.Duration.ToString()).Distinct().ToList().Select(x => new destinationList { text = x, value = x }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();
        }
        public List<destinationList> GetBlockCity(string pinCode)
        {
            try
            {

                var Cities = db.tbl_BlockCity.AsQueryable().AsEnumerable().Where(x => x.PinCode == Convert.ToInt32(pinCode)).Select(x => new destinationList { text = x.BlockCity, value = x.BlockCity }).Distinct().ToList();
                return Cities;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();
        }
        public List<customSelectList> GetAssigneeNominee(string type, Int32 planid)
        {
            try
            {
                var CompanyId = db.View_HealthChartData.AsEnumerable().Where(x =>
                    x.PremiumChartID == planid).FirstOrDefault().CompanyId;
                var Coverage = db.AssigneeRelations.AsQueryable().AsEnumerable().Where(x => x.CompanyId == CompanyId && x.InsuranceType == type).Select(x => x.AssigneeRelationshipName.ToString()).Distinct().ToList().Select(x => new customSelectList { text = x, value = x }).ToList();
                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public string RandomNum()
        {
            string quoteNo = string.Empty;
            try
            {

                quoteNo = (new Random()).Next(1000000000).ToString();
                long qut = Convert.ToInt64(quoteNo);
                var result = db.Health_Insurance_Record_Saved.SingleOrDefault(x => x.tid == qut);
                if (result == null)
                {
                    return quoteNo;
                }
                else
                {
                    return RandomNum();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public long SaveHealthSearch(HealthPlanDetails srchHealthlIns)
        {
            try
            {
                //Child = true ?1:0;
                //Adult = true ? 1 : 0;
                //Self = true ? 1 : 0;  
                Random r = new Random();
                long num = Convert.ToInt64(RandomNum());

                int Adult1 = 0; int Adult2 = 0;
                int Child = Convert.ToInt32(srchHealthlIns.son);
                int Adult = Convert.ToInt32(srchHealthlIns.chkspouses);
                int Self = Convert.ToInt32(srchHealthlIns.chkself);
                int ageSelf = Convert.ToInt32(srchHealthlIns.ageSelf);
                if (ageSelf != 0)
                {
                    Adult1 = 1;
                }
                int ageSpouse = Convert.ToInt32(srchHealthlIns.ageSpouse);
                if (ageSpouse != 0)
                {
                    Adult2 = 1;
                }
                Adult = Adult1 + Adult2;
                var tDetails = new Health_Insurance_Record_Saved
                {

                    Gender = srchHealthlIns.Gender,
                    Name = srchHealthlIns.Full_Name,
                    MobileNo = srchHealthlIns.MobileNo,
                    City = srchHealthlIns.City,
                    Income = srchHealthlIns.income,
                    Child = Child.ToString(),
                    //Self_Age=srchHealthlIns.ageSelf,
                    //Self = srchHealthlIns.Self,
                    Self_Age = srchHealthlIns.ageSelf,
                    Adult = Adult.ToString(),
                    Self = Self.ToString(),
                    spouce_Age = srchHealthlIns.ageSpouse,
                    child1_Age = srchHealthlIns.ageChild1,
                    child2_Age = srchHealthlIns.ageChild2,
                    tid = num,
                    createDate=DateTime.Now

                };

                db.Health_Insurance_Record_Saved.Add(tDetails);
                db.SaveChanges();

                var tId = Convert.ToInt64(tDetails.tid);

                return tId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }
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
        //..................For Bajaj..............
        public List<View_HealthinsuranceModel> GetPremiumList(View_HealthinsuranceModel srchTrvlIns)
        {
            try
            {
                if (srchTrvlIns.Spouses == "2")
                {
                    srchTrvlIns.Spouses = "1";
                }
                string membercombo = ""; string productcode = ""; int totalmem = 0;
                if (srchTrvlIns.Self == "1" && srchTrvlIns.Spouses == "0" && srchTrvlIns.son == "0")
                { membercombo = "1A"; productcode = "8429"; totalmem = 1; }
                else if (srchTrvlIns.Self == "1" && srchTrvlIns.Spouses == "1" && srchTrvlIns.son == "0")
                { membercombo = "2A"; productcode = "8430"; totalmem = 2; }
                else if (srchTrvlIns.Self == "1" && srchTrvlIns.Spouses == "1" && srchTrvlIns.son == "1")
                { membercombo = "2A+1C"; productcode = "8430"; totalmem = 3; }
                else if (srchTrvlIns.Self == "1" && srchTrvlIns.Spouses == "1" && srchTrvlIns.son == "2")
                { membercombo = "2A+2C"; productcode = "8430"; totalmem = 4; }
                string Edate = DateTime.Today.AddYears(+1).AddDays(-1).ToString("dd MMM yyyy");
                string Sdatetoday = DateTime.Today.ToString("dd MMM yyyy");
                string dobcal = DateTime.Today.ToString("yyyy");
                int dob = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTrvlIns.ageSelf);
                int dob1 = Convert.ToInt32(dobcal) - Convert.ToInt32(srchTrvlIns.AgeFrom);
                decimal dob2 = Convert.ToDecimal(dobcal) - Convert.ToDecimal(srchTrvlIns.AgeTo);//15ppppp
                decimal dob3 = Convert.ToDecimal(dobcal) - Convert.ToDecimal(srchTrvlIns.AgeFromDuration);

                int d = Convert.ToInt32(dob2);
                int d1 = Convert.ToInt32(dob3);
                string dateage = "01-Jan-" + dob.ToString();
                string dateage1 = "01-Jan-" + dob1.ToString();
                string dateage2 = "01-Jan-" + d.ToString();
                string dateage3 = "01-Jan-" + d1.ToString();

                if (srchTrvlIns.CoverForYear == "1")
                {
                    srchTrvlIns.CoverForYear = "1200";
                }
                else if (srchTrvlIns.CoverForYear == "2")
                {
                    srchTrvlIns.CoverForYear = "2400";
                }
                else if (srchTrvlIns.CoverForYear == "3")
                {
                    srchTrvlIns.CoverForYear = "3600";
                }
                var result = "";
                var zone = "";
                // srchTrvlIns.Type = srchTrvlIns.Type.ToUpper();
                var zoneid = db.tbl_BajajZone.AsEnumerable().Where(x => x.Zone == srchTrvlIns.Type).ToList();
                if (zoneid.Count != 0)
                {
                    zone = "1";
                }
                else
                {
                    zone = "2";
                }
                var CompanyId = db.tbl_BajajBlockCity.AsEnumerable().Where(x => x.PolicyCity == srchTrvlIns.Type).ToList();
                //  string d = CompanyId.ToString();
                if (CompanyId.Count == 0)
                {
                    if (membercombo == "1A")
                    {
                        result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + srchTrvlIns.CoverForYear + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + srchTrvlIns.ageSelf + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + srchTrvlIns.SumAssured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + zone + "', 'polcovvolntrycp':'', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + srchTrvlIns.SumAssured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                    }
                    else if (membercombo == "2A")
                    {
                        result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + srchTrvlIns.CoverForYear + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + srchTrvlIns.ageSelf + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + srchTrvlIns.AgeFrom + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' } ], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + srchTrvlIns.SumAssured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + zone + "', 'polcovvolntrycp':'', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + srchTrvlIns.SumAssured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                    }
                    else if (membercombo == "2A+1C")
                    {
                        result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + srchTrvlIns.CoverForYear + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + srchTrvlIns.ageSelf + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + srchTrvlIns.AgeFrom + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memrelation': 'SON', 'memdob': '" + dateage2 + "', 'memage': '" + srchTrvlIns.AgeTo + "', 'memgender': 'M', 'memheightcm': '145', 'memweightkg': '40', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' }], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + srchTrvlIns.SumAssured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + zone + "', 'polcovvolntrycp':'', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + srchTrvlIns.SumAssured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memaddflag': 'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                    }
                    else if (membercombo == "2A+2C")
                    {

                        result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + srchTrvlIns.CoverForYear + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + srchTrvlIns.ageSelf + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + srchTrvlIns.AgeFrom + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memrelation': 'SON', 'memdob': '" + dateage2 + "', 'memage': '" + srchTrvlIns.AgeTo + "', 'memgender': 'M', 'memheightcm': '145', 'memweightkg': '40', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' }, { 'membername': 'M Sunny', 'memrelation': 'SON', 'memdob': '" + dateage3 + "', 'memage': '" + srchTrvlIns.AgeToDuration + "', 'memgender': 'M', 'memheightcm': '165', 'memweightkg': '50', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' } ], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + srchTrvlIns.SumAssured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + zone + "', 'polcovvolntrycp':'', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + srchTrvlIns.SumAssured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memaddflag': 'Y' }, { 'membername': 'M suuny', 'memaddflag': 'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                    }
                    int sumassured = Convert.ToInt32(srchTrvlIns.SumAssured);
                    if (membercombo == "1A")
                    {
                        if (sumassured < 500000 || sumassured > 2000000)
                        {
                            return new List<View_HealthinsuranceModel>();
                        }
                    }
                    else if (membercombo == "2A" || membercombo == "2A+1C" || membercombo == "2A+2C")
                    {
                        if (sumassured < 200000 || sumassured > 2000000)
                        {
                            return new List<View_HealthinsuranceModel>();
                        }
                    }

                    result = result.Replace("'", "\"");
                    var response = requestHandler.Response(bajajurl, result);
                    List<View_HealthinsuranceModel> lst = new List<View_HealthinsuranceModel>();
                    if (response["Status"] == "200")
                    {
                        var responseString = response["response"].ToString();
                        JObject joResponse = JObject.Parse(responseString);
                        JObject ojObject = (JObject)joResponse["genpremdtlsres"];
                        JObject ojObject2 = (JObject)joResponse["servicetaxmstobj"];
                        var responce = ojObject.ToString();
                        var responce2 = ojObject2.ToString();
                        var bajajresult = JsonConvert.DeserializeObject<Genpremdtlsres>(responce);
                        var bajajrefresult = JsonConvert.DeserializeObject<RootObject>(responseString);
                        var bajajref = JsonConvert.DeserializeObject<Servicetaxmstobj>(responce2);
                        bajajrefresult.transactionid = bajajrefresult.transactionid;
                        bajajresult.contractid = bajajresult.contractid;
                        bajajresult.quoterefno = bajajresult.quoterefno;
                        bajajresult.refno = bajajresult.refno;
                        bajajresult.scrutinyno = bajajresult.scrutinyno;
                        bajajresult.policyref = bajajresult.policyref;
                        bajajresult.netpremium = bajajresult.netpremium;
                        bajajresult.servicetaxamt = bajajresult.servicetaxamt;
                        bajajresult.educessamt = bajajresult.educessamt;
                        bajajresult.finalpremium = bajajresult.finalpremium;
                        bajajref.taxcode = bajajref.taxcode;
                        bajajref.taxtype = bajajref.taxtype;
                        bajajref.addtaxval4 = bajajref.addtaxval4;
                        srchTrvlIns.companyid = 10009;
                        srchTrvlIns.PremiumDesc = "Bajaj Allianz ";
                        srchTrvlIns.Logo = "/Logo/bajaj.png";
                        srchTrvlIns.Premium = bajajresult.totalpremium;
                        srchTrvlIns.PremiumChartID = "0";
                        srchTrvlIns.ProductName = "Star Health Insurance";
                        //.....................Save Bajaj 
                        var tDetails = new tbl_BajajResponce_Data
                        {
                            TransactionId = bajajrefresult.transactionid,
                            Contractid = srchTrvlIns.CoverForYear,
                            Quotrefno = zone,
                            RefNO = bajajresult.refno,
                            ScurityNo = bajajresult.scrutinyno,
                            PolicyRef = bajajresult.policyref,
                            NetPremium = Convert.ToInt32(bajajresult.netpremium),
                            Servicetaxamt = Convert.ToInt32(bajajresult.servicetaxamt),
                            edueAmt = Convert.ToInt32(bajajresult.educessamt),
                            TotalPremium = Convert.ToInt32(srchTrvlIns.Premium),
                            FinalPremium = Convert.ToInt32(bajajresult.finalpremium),
                            taxCode = bajajref.taxcode,
                            TaxType = bajajref.taxtype,
                            AddTaxValue = Convert.ToInt32(bajajref.addtaxval4),
                            CompanyId = 10009,
                            PalnID = srchTrvlIns.PremiumChartID,
                            IDData = Convert.ToInt32(srchTrvlIns.PlanID),
                            SumInsured = srchTrvlIns.SumAssured,
                            Adult = Convert.ToInt32(srchTrvlIns.Self),
                            Spouse = Convert.ToInt32(srchTrvlIns.Spouses),
                            Child = Convert.ToInt32(srchTrvlIns.son),
                            ProductCode = Convert.ToInt32(productcode),
                            MemberCombo = membercombo,
                            //createdate=DateTime.Now
                        };

                        db.tbl_BajajResponce_Data.Add(tDetails);
                        db.SaveChanges();
                        var tId = tDetails.ID;
                        srchTrvlIns.PlanID = tId.ToString();
                    }
                    if (srchTrvlIns.Premium != "0" &&srchTrvlIns.Premium != null)
                    {
                        lst.Add(srchTrvlIns);
                        return lst;
                    }
                }
                else
                {
                    return new List<View_HealthinsuranceModel>();

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            return new List<View_HealthinsuranceModel>();



        }

        //........................................

        public List<View_HealthinsuranceModel> GetHealthPremiumList(HealthPlanDetails srchHealthlIns)
        {
            try
            {

                int Child = Convert.ToInt32(srchHealthlIns.son);
                int Adult = Convert.ToInt32(srchHealthlIns.Spouses);
                int Self = Convert.ToInt32(srchHealthlIns.Self);
                //if (Adult == 1 && Self == 1)
                //{
                //    Adult = 2;
                //    Self = 0;
                //}
                int Adult1 = 0; int Adult2 = 0;
                if (Adult == 0 && Self == 1)
                {
                    Adult = 1;
                    Self = 1;
                }
                decimal Insureed = Convert.ToDecimal(srchHealthlIns.SumInsured);
                int insured = Convert.ToInt32(Insureed);
                int CoverForYear = Convert.ToInt32(srchHealthlIns.CoverForYear);
                //  decimal Insureed = Convert.ToDecimal("100000.00");
                int ageSelf = Convert.ToInt32(srchHealthlIns.ageSelf);
                if (ageSelf != 0)
                {
                    Adult1 = 1;
                }
                decimal ageChild1 = Convert.ToDecimal(srchHealthlIns.ageChild1);//15/06ppppppppp
                decimal ageChild2 = Convert.ToDecimal(srchHealthlIns.ageChild2);
                int agespouse = Convert.ToInt32(srchHealthlIns.ageSpouse);
                if (agespouse != 0)
                {
                    Adult2 = 1;
                }
                Adult = Adult1 + Adult2;
                int gst = Convert.ToInt32("18");
                //&& ((Convert.ToInt32(x.TermFrom) <= stayDays
                //&& Convert.ToInt32(x.TermTo) >= stayDays))
                var Policy = db.sp_HealthInsuranceList(ageSelf, Adult, Child, CoverForYear, insured).Select(x => new View_HealthinsuranceModel
                {
                    Logo = x.LogoImage,
                    PremiumDesc = x.premiumdesc,
                    SumAssured = x.SumInsured.ToString(),
                    Gsttax = gst,
                    Premium = x.Premium.ToString(),
                    Duration = x.Duration,
                    PlanID = x.PlanID.ToString(),
                    ProductName = x.CompanyName,
                    PremiumChartID = x.PremiumChartID,
                    companyid = Convert.ToInt32(x.CompanyId),
                    BeforeServiceTax = x.BeforeServiceTax.ToString()
                }).ToList();
                Policy = Policy.AsEnumerable().Where(x => x.SumAssured == srchHealthlIns.SumInsured).ToList();
                //var Policy = db.View_HealthChartData.AsQueryable().AsEnumerable().Where(x =>
                //    (Convert.ToInt32(x.AgeFrom) <= ageSelf
                //    && Convert.ToInt32(x.AgeTo) >= ageSelf)
                //    && x.Duration == "1"
                //  && x.Adults == Adult && x.Childrens == Child
                //    ).GroupBy(x => new { x.CompanyName, x.LogoImage, x.PremiumDesc, x.Duration, x.ProductName, x.PlanID, x.CompanyId }).Select(
                //    x => new View_HealthinsuranceModel
                //    {

                //        Logo = x.Key.LogoImage,
                //        PremiumDesc = x.Key.PremiumDesc,
                //        SumAssured = x.Min(m => Convert.ToDecimal(m.SumInsured)).ToString(),

                //        Gsttax = gst,
                //        Premium = x.Min(m => Convert.ToDecimal(m.Premium)).ToString(),
                //        Duration = x.Key.Duration,

                //        PlanID = x.Key.PlanID.ToString(),
                //        ProductName = x.Key.CompanyName,

                //        BeforeServiceTax = x.Min(m =>Convert.ToDecimal(m.BeforeServiceTax)).ToString(),
                //       PremiumChartID = x.Min(m => m.PremiumChartID.ToString()),
                //        companyid = Convert.ToInt32(x.Key.CompanyId)


                //    }).OrderBy(x => x.Premium).ToList();

                return Policy;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error(ex.Message.ToString(), "");
            }
            return new List<View_HealthinsuranceModel>();

        }
        public RegistrationDetails GetQuotationBsjajDetail(string planId, string TId, string QuoteNo)
        {

            RegistrationDetails quotationDetail = new RegistrationDetails();
            Health_ProposalCreation star_ProposalCreation = new Health_ProposalCreation();
            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                var bajajresponce = db.tbl_BajajResponce_Data.AsEnumerable().Where(x =>
                       x.IDData == _TId).LastOrDefault();
                var bajajData = db.Health_Insurance_Record_Saved.AsEnumerable().Where(x =>
                      x.tid == _TId).FirstOrDefault();
                quotationDetail.proposerName = bajajData.Name;
                //    quotationDetail.insureds0.name = bajajData.Name;
                quotationDetail.proposerAddressOne = bajajData.City;
                quotationDetail.Amount = bajajresponce.NetPremium.ToString();
                quotationDetail.totalPremium = bajajresponce.FinalPremium.ToString();
                quotationDetail.serviceTax = bajajresponce.AddTaxValue.ToString();
                quotationDetail.NoOfAdults = ((bajajresponce.Adult) + (bajajresponce.Spouse)).ToString();
                quotationDetail.NoOfChildren = bajajresponce.Child.ToString();
                quotationDetail.sumAssured = bajajresponce.SumInsured.ToString();
                quotationDetail.Plan = "Bajaj Health Guard individual";
                quotationDetail.schemeId = Convert.ToInt32(bajajresponce.ProductCode);
                quotationDetail.searchId = Convert.ToInt32(bajajresponce.ID);
                quotationDetail.policyCategory = bajajresponce.MemberCombo;
                quotationDetail.companyId = bajajresponce.CompanyId.ToString();
                quotationDetail.endOn = DateTime.Today.AddYears(+1).AddDays(-1).ToString("dd-MM-yyyy");
                quotationDetail.startOn = DateTime.Today.ToString("dd-MM-yyyy");
                quotationDetail.sumInsuredId = Convert.ToInt32(bajajresponce.TransactionId);

                //.................
                if (bajajresponce.Spouse == 2)
                {
                    bajajresponce.Spouse = 1;
                }
                string membercombo = ""; string productcode = ""; int totalmem = 0;
                if (bajajData.Self == "1" && bajajData.Adult == "1" && bajajData.Child == "0")
                { membercombo = "1A"; productcode = "8429"; totalmem = 1; }
                else if (bajajData.Self == "1" && bajajData.Adult == "2" && bajajData.Child == "0")
                { membercombo = "2A"; productcode = "8430"; totalmem = 2; }
                else if (bajajData.Self == "1" && bajajData.Adult == "2" && bajajData.Child == "1")
                { membercombo = "2A+1C"; productcode = "8430"; totalmem = 3; }
                else if (bajajData.Self == "1" && bajajData.Adult == "2" && bajajData.Child == "2")
                { membercombo = "2A+2C"; productcode = "8430"; totalmem = 4; }
                string Edate = DateTime.Today.AddYears(+1).AddDays(-1).ToString("dd MMM yyyy");
                string Sdatetoday = DateTime.Today.ToString("dd MMM yyyy");
                string dobcal = DateTime.Today.ToString("yyyy");
                int dob = Convert.ToInt32(dobcal) - Convert.ToInt32(bajajData.Self_Age);
                //if (bajajData.Self_Age != "")
                //{
                //   // quotationDetail.insureds0.name = quotationDetail.proposerName;

                //}
                int dob1 = Convert.ToInt32(dobcal) - Convert.ToInt32(bajajData.spouce_Age);
                decimal dob2 = Convert.ToDecimal(dobcal) - Convert.ToDecimal(bajajData.child1_Age);//1506pppppp
                decimal dob3 = Convert.ToDecimal(dobcal) - Convert.ToDecimal(bajajData.child2_Age);

                int do1 = Convert.ToInt32(dob2);
                int do2 = Convert.ToInt32(dob3);


                string dateage = "01-Jan-" + dob.ToString();
                string d = "01-01-" + dob.ToString();
                string dateage1 = "01-Jan-" + dob1.ToString();
                string d1 = "01-01-" + dob1.ToString();
                string dateage2 = "01-Jan-" + do1.ToString();
                string d2 = "01-01-" + do1.ToString();
                string dateage3 = "01-Jan-" + do2.ToString();
                string d3 = "01-01-" + do2.ToString();

                quotationDetail.proposerDob = d;
                var zone = "";


                if (bajajresponce.Contractid == "1")
                {
                    bajajresponce.Contractid = "1200";
                }
                else if (bajajresponce.Contractid == "2")
                {
                    bajajresponce.Contractid = "2400";
                }
                else if (bajajresponce.Contractid == "3")
                {
                    bajajresponce.Contractid = "3600";
                }
                var result = "";
                if (membercombo == "1A")
                {
                    result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + bajajresponce.Contractid + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + bajajData.Self_Age + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' } ], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + bajajresponce.SumInsured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + bajajresponce.Quotrefno + "', 'polcovvolntrycp':'" + QuoteNo + "', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + bajajresponce.SumInsured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                }
                else if (membercombo == "2A")
                { result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + bajajresponce.Contractid + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + bajajData.Self_Age + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + bajajData.spouce_Age + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + bajajresponce.SumInsured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + bajajresponce.Quotrefno + "', 'polcovvolntrycp':'" + QuoteNo + "', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + bajajresponce.SumInsured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }"; }
                else if (membercombo == "2A+1C")
                { result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + bajajresponce.Contractid + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + bajajData.Self_Age + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + bajajData.spouce_Age + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memrelation': 'SON', 'memdob': '" + dateage2 + "', 'memage': '" + bajajData.child1_Age + "', 'memgender': 'M', 'memheightcm': '145', 'memweightkg': '40', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' }], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + bajajresponce.SumInsured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + bajajresponce.Quotrefno + "', 'polcovvolntrycp':'" + QuoteNo + "', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + bajajresponce.SumInsured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memaddflag': 'Y' }], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }"; }
                else if (membercombo == "2A+2C")
                {
                    result = "{ 'userid':'" + BajajUser + "', 'password':'" + BajajPassword + "', 'sourcedtls':{ 'username':'', 'departmentcode':'84', 'productcode':'" + productcode + "', 'businesstype':'NB', 'imdcode':'10043080', 'subimdcode':'0', 'modulename':'WEBSERVICE' }, 'policydtls':{ 'termstartdate': '" + Sdatetoday + "', 'termenddate': '" + Edate + "', 'partnertype':'P', 'businesstype':'NB', 'policyperiod':'" + bajajresponce.Contractid + "', 'productcode':'" + productcode + "',  'scrlocationcode':'9906' }, 'previnsdtls':{ 'noofclaims':'0' }, 'hcpdtpreobj':{ 'selfcoveredflag':'Y', 'membercombo':'" + membercombo + "' }, 'hcpdtmemlist':[ { 'membername':'M Praveen Kumar', 'memrelation':'SELF', 'memdob':'" + dateage + "', 'memage':'" + bajajData.Self_Age + "', 'memgender':'M', 'memheightcm':'173', 'memweightkg':'55', 'membmi':'18.4', 'memoccupation':'1', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memrelation':'SPOUSE', 'memdob':'" + dateage1 + "', 'memage':'" + bajajData.spouce_Age + "', 'memgender':'F', 'memheightcm':'165', 'memweightkg':'50', 'membmi':'18.3', 'memoccupation':'8', 'memgrossmonthlyincome':'0', 'memnomineename':'M Manohar', 'memnomineerelation':'FATHER', 'mempreexistdisease':'0', 'memspecialcondition':'NA', 'memsmkertbco':'0', 'memasthma':'0', 'memcholstrldisordr':'0', 'memheartdisease':'0', 'memhypertension':'0', 'memdiabetes':'0', 'memobesity':'0', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memrelation': 'SON', 'memdob': '" + dateage2 + "', 'memage': '" + bajajData.child1_Age + "', 'memgender': 'M', 'memheightcm': '145', 'memweightkg': '40', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' }, { 'membername': 'M Sunny', 'memrelation': 'SON', 'memdob': '" + dateage3 + "', 'memage': '" + bajajData.child2_Age + "', 'memgender': 'M', 'memheightcm': '165', 'memweightkg': '50', 'membmi': '18.3', 'memoccupation': '8', 'memgrossmonthlyincome': '0', 'memnomineename': 'M Manohar', 'memnomineerelation': 'FATHER', 'mempreexistdisease': '0', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memaddflag': 'Y' } ], 'hcpdtmemcovlist':[ { 'memiptreatsi':'" + bajajresponce.SumInsured + "' } ], 'hcpdtpolcovobj':{ 'polcovzone':'" + bajajresponce.Quotrefno + "', 'polcovvolntrycp':'" + QuoteNo + "', 'polcovspcndt':'NA', 'polcovmedcnd':'0', 'polcovihtsi':'" + bajajresponce.SumInsured + "' }, 'hcpdtmemcovaddonlist':[ { 'membername':'M Praveen Kumar', 'memaddflag':'Y' }, { 'membername':'M Sushmitha', 'memaddflag':'Y' }, { 'membername': 'M Ram', 'memaddflag': 'Y' }, { 'membername': 'M suuny', 'memaddflag': 'Y' } ], 'hcpstagedataobj':{ 'totalmembernos':'" + totalmem + "', 'partnerpincode':'400706' }, 'rcptlist':[ { } ], 'servicetaxmstobj':{ }, 'genpremdtls':{ }, 'hcpdpolcaddobj':{ }, 'tycpaddrlist':[ { } ], 'tycpdetails':{ }, 'channeldtls':{ }, 'transactionid':null, 'geniclist':[ { } ], 'receiptftcobj':{ }, 'gencoinslist':[ { } ], 'scripdtlist':[ { } ], 'refdtlist':[ { } ], 'endtdtls':{ } }";
                }
                result = result.Replace("'", "\"");
                var response = requestHandler.Response(bajajurl, result);
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    JObject joResponse = JObject.Parse(responseString);
                    JObject ojObject = (JObject)joResponse["genpremdtlsres"];
                    JObject ojObject2 = (JObject)joResponse["servicetaxmstobj"];
                    var responce = ojObject.ToString();
                    var responce2 = ojObject2.ToString();
                    var bajajresult = JsonConvert.DeserializeObject<Genpremdtlsres>(responce);
                    var bajajrefresult = JsonConvert.DeserializeObject<RootObject>(responseString);
                    var bajajref = JsonConvert.DeserializeObject<Servicetaxmstobj>(responce2);
                    bajajrefresult.transactionid = bajajrefresult.transactionid;
                    quotationDetail.sumInsuredId = Convert.ToInt32(bajajrefresult.transactionid);
                    bajajresult.netpremium = bajajresult.netpremium;
                    bajajresult.servicetaxamt = bajajresult.servicetaxamt;
                    bajajresult.finalpremium = bajajresult.finalpremium;
                    quotationDetail.Amount = bajajresult.netpremium.ToString();
                    quotationDetail.totalPremium = bajajresult.finalpremium.ToString();
                    quotationDetail.serviceTax = bajajresult.servicetaxamt.ToString();
                    quotationDetail.eiaNumber = QuoteNo;
                    quotationDetail.socialStatusUnorganized = bajajresponce.Quotrefno;

                    quotationDetail.appointeeName = d1.ToString();
                    quotationDetail.appointeeAge = d2.ToString();
                    quotationDetail.appointeeAgeTwo = d3.ToString();
                }
                var tDetails = new tbl_BajajResponce_Data
                {
                    TransactionId = quotationDetail.sumInsuredId,
                    Contractid = bajajresponce.Contractid,
                    Quotrefno = bajajresponce.Quotrefno,
                    RefNO = "",
                    ScurityNo = "",
                    PolicyRef = "",
                    NetPremium = Convert.ToInt32(quotationDetail.Amount),
                    Servicetaxamt = Convert.ToInt32(quotationDetail.serviceTax),
                    edueAmt = 0,
                    TotalPremium = Convert.ToInt32(quotationDetail.totalPremium),
                    FinalPremium = Convert.ToInt32(quotationDetail.totalPremium),
                    taxCode = "",
                    TaxType = "",
                    AddTaxValue = 0,
                    CompanyId = 10009,
                    PalnID = planId,
                    IDData = Convert.ToInt32(TId),
                    SumInsured = bajajresponce.SumInsured,
                    Adult = Convert.ToInt32(bajajData.Self),
                    Spouse = Convert.ToInt32(bajajData.Adult),
                    Child = Convert.ToInt32(bajajData.Child),
                    ProductCode = Convert.ToInt32(productcode),
                    MemberCombo = membercombo
                };

                db.tbl_BajajResponce_Data.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.ID;
                planId = tId.ToString();
                //...................
                return quotationDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;
        }
        //public HealthProposalRequest GetQuotationBsjajDetail(string planId, int TId)
        //{
        //    HealthProposalRequest quotationDetail = new HealthProposalRequest();
        //    Health_ProposalCreation star_ProposalCreation = new Health_ProposalCreation();
        //    try
        //    {

        //        Int64 _planId = Convert.ToInt64(planId);
        //        Int64 _TId = Convert.ToInt64(TId);
        //        var bajajresponce = db.tbl_BajajResponce_Data.AsEnumerable().Where(x =>
        //               x.IDData == TId).LastOrDefault();
        //        var bajajData = db.Health_Insurance_Record_Saved.AsEnumerable().Where(x =>
        //              x.ID == TId).FirstOrDefault();
        //        quotationDetail.proposerName = bajajData.Name;
        //        quotationDetail.proposerAddressOne = bajajData.City;
        //        quotationDetail.Amount = bajajresponce.NetPremium.ToString();
        //        quotationDetail.totalPremium = bajajresponce.FinalPremium.ToString();
        //        quotationDetail.serviceTax = bajajresponce.AddTaxValue.ToString();
        //        quotationDetail.NoOfAdults = ((bajajresponce.Adult) + (bajajresponce.Spouse)).ToString();
        //        quotationDetail.NoOfChildren = bajajresponce.Child.ToString();
        //        quotationDetail.sumAssured = bajajresponce.SumInsured.ToString();
        //        quotationDetail.Plan = "Bajaj Health Guard individual";
        //        quotationDetail.schemeId = Convert.ToInt32(bajajresponce.ProductCode);
        //        quotationDetail.searchId = Convert.ToInt32(bajajresponce.ID);
        //        quotationDetail.policyCategory = bajajresponce.MemberCombo;
        //        quotationDetail.companyId = bajajresponce.CompanyId.ToString();
        //        quotationDetail.endOn = DateTime.Today.AddYears(+1).AddDays(-1).ToString("dd-MM-yyyy");
        //        quotationDetail.startOn = DateTime.Today.ToString("dd-MM-yyyy");
        //        quotationDetail.sumInsuredId = Convert.ToInt32(bajajresponce.TransactionId);
        //        //----------------------------------------------------------------------------------
        //        return quotationDetail;
        //    }
        //    catch(Exception ex)
        //    {  }
        //    return quotationDetail;
        //}
        public RegistrationDetails GetQuotationDetail(string planId, string TId)
        {
            RegistrationDetails quotationDetail = new RegistrationDetails();
            Health_ProposalCreation star_ProposalCreation = new Health_ProposalCreation();
            try
            {
                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                var view_Insurance = db.View_HealthChartData.AsEnumerable().Where(x =>
                    x.PremiumChartID == _planId).FirstOrDefault();
                var policyTypeName = "";
                switch (view_Insurance.PremiumDesc)
                {
                    case "Comprehensive":
                        policyTypeName = "COMPREHENSIVE";
                        break;
                    case "Comprehensive Individual":
                        policyTypeName = "COMPREHENSIVEIND";
                        break;
                    case "Family Health Optima":
                        policyTypeName = "FHONEW";
                        break;
                    case "Medi Classic Individual":
                        policyTypeName = "MCINEW";
                        break;
                    case "Red Carpet":
                        policyTypeName = "REDCARPETFMLY";
                        break;
                    case "Red Carpet (Individual)":
                        policyTypeName = "REDCARPET";
                        break;
                    default:
                        break;
                }
                var InsuranceSearchBies = db.Health_Insurance_Record_Saved.AsEnumerable().Where(x => x.tid == _TId).FirstOrDefault();

                decimal Gst = Convert.ToDecimal("18");
                decimal servicetax = Convert.ToDecimal(view_Insurance.BeforeServiceTax);
                decimal Amt = servicetax * Gst / 100;
                decimal Amount = servicetax + Amt;
                int self = Convert.ToInt32(InsuranceSearchBies.Self);
                int spouse = Convert.ToInt32(InsuranceSearchBies.Adult);
                if (spouse == 2)
                {
                    spouse = 1;
                }
                else if (spouse == 1)
                {
                    spouse = 0;
                }
                int child = Convert.ToInt32(InsuranceSearchBies.Child);
                view_Insurance.Adults = self + spouse;
                if (self == 1 && spouse == 1 && child == 1)
                {
                    view_Insurance.SchemeId = 5;
                }
                else if (self == 1 && spouse == 1 && child == 2)
                {
                    view_Insurance.SchemeId = 6;
                }
                else if (self == 1 && spouse == 1 && child == 0)
                {
                    view_Insurance.SchemeId = 1;
                }
                else if (self == 1 && spouse == 0 && child == 0)
                {
                    view_Insurance.SchemeId = 2;
                }
                else if (self == 1 && spouse == 0 && child == 1)
                {
                    view_Insurance.SchemeId = 2;
                }
                else if (self == 1 && spouse == 0 && child == 2)
                {
                    view_Insurance.SchemeId = 3;
                }

                //sceamid=db.tbl_Scheme.AsEnumerable().Where(x=>x.Scheme==).FirstOrDefault();
                switch (view_Insurance.CompanyId)
                {

                    case 2:
                        quotationDetail = new RegistrationDetails
                        {

                            sumAssured = view_Insurance.SumInsured.ToString(),
                            Amount = Amount.ToString(),
                            NoOfAdults = view_Insurance.Adults.ToString(),
                            NoOfChildren = view_Insurance.Childrens.ToString(),
                            Plan = view_Insurance.PremiumDesc,
                            schemeId = Convert.ToInt32(view_Insurance.SchemeId)
                        };
                        break;
                    case 10006:
                        var DoB = DateTime.Now.AddYears(Convert.ToInt32("-" + InsuranceSearchBies.Self_Age.ToString())).ToString("MMMM dd, yyyy");
                        var dob2 = string.Empty;
                        if (!string.IsNullOrEmpty(InsuranceSearchBies.spouce_Age))
                        {
                            dob2 = DateTime.Now.AddYears(Convert.ToInt32("-" + InsuranceSearchBies.spouce_Age.ToString())).ToString("MMMM dd, yyyy");
                            goto Step2;
                        }
                        if (!string.IsNullOrEmpty(InsuranceSearchBies.child1_Age))
                        {
                            dob2 = DateTime.Now.AddYears(Convert.ToInt32("-" + InsuranceSearchBies.child1_Age.ToString())).ToString("MMMM dd, yyyy");
                            goto Step2;
                        }
                        if (!string.IsNullOrEmpty(InsuranceSearchBies.child2_Age))
                        {
                            dob2 = DateTime.Now.AddYears(Convert.ToInt32("-" + InsuranceSearchBies.child2_Age.ToString())).ToString("MMMM dd, yyyy");
                            goto Step2;
                        }

                    Step2:
                        var insured_0 = new Insured
                        {
                            dob = DoB,
                            sumInsuredId = 1
                        };
                        dob2 = string.IsNullOrEmpty(dob2) ? DoB : dob2;
                        var insured_1 = new Insured
                        {
                            dob = dob2,
                            sumInsuredId = 1
                        };
                        var insured_2 = new Insured
                        {
                            dob = dob2,
                            sumInsuredId = 1
                        };
                        var insured_3 = new Insured
                        {
                            dob = dob2,
                            sumInsuredId = 1
                        };
                        if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age == null && InsuranceSearchBies.child1_Age == null && InsuranceSearchBies.child2_Age == null)
                        {

                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 1,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0

                            };
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age != null && InsuranceSearchBies.child1_Age == null && InsuranceSearchBies.child2_Age == null)
                        {
                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 1,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0,
                                insureds_1 = insured_1

                            };
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age != null && InsuranceSearchBies.child1_Age != null && InsuranceSearchBies.child2_Age == null)
                        {
                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 5,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0,
                                insureds_1 = insured_1,
                                insureds_2 = insured_2
                            };

                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age != null && InsuranceSearchBies.child1_Age != null && InsuranceSearchBies.child2_Age != null)
                        {
                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 6,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0,
                                insureds_1 = insured_1,
                                insureds_2 = insured_2,
                                insureds_3 = insured_3
                            };
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age == null && InsuranceSearchBies.child1_Age != null && InsuranceSearchBies.child2_Age != null)
                        {
                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 3,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0,
                                insureds_1 = insured_1,
                                insureds_2 = insured_2,
                                insureds_3 = insured_3
                            };
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age == null && InsuranceSearchBies.child1_Age != null && InsuranceSearchBies.child2_Age == null)
                        {
                            star_ProposalCreation = new Health_ProposalCreation
                            {
                                period = view_Insurance.Duration.ToString(),
                                PlanId = view_Insurance.PlanID.ToString(),
                                postalCode = "226002",
                                sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode),
                                schemeId = 2,//Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                insureds_0 = insured_0,
                                insureds_1 = insured_1,
                                insureds_2 = insured_2,
                                insureds_3 = insured_3
                            };
                        }

                        string postData = JsonConvert.SerializeObject(star_ProposalCreation);
                        // var response = requestHandler.Response("https://devapi.sbigeneral.in/customers/v1/premiumcalculation", postData);
                        if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age == null && InsuranceSearchBies.child1_Age == null && InsuranceSearchBies.child2_Age == null)
                        {
                            postData = postData.Replace(",\"insureds_1\":null,\"insureds_2\":null,\"insureds_3\":null", "");
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age != null && InsuranceSearchBies.child1_Age == null && InsuranceSearchBies.child2_Age == null)
                        {
                            postData = postData.Replace(",\"insureds_2\":null,\"insureds_3\":null", "");
                        }
                        else if (InsuranceSearchBies.Self_Age != null && InsuranceSearchBies.spouce_Age != null && InsuranceSearchBies.child1_Age != null && InsuranceSearchBies.child2_Age == null)
                        {
                            postData = postData.Replace(",\"insureds_3\":null", "");
                        }
                        postData = postData.Replace(",\"description\":null", "");

                        var response = requestHandler.Response("https://ig.starhealth.in/api/proposal/premium/calculate", postData.Replace("insureds_0", "insureds[0]").Replace("insureds_1", "insureds[1]").Replace("insureds_2", "insureds[2]").Replace("insureds_3", "insureds[3]"));
                        if (response["Status"] == "200")
                        {
                            var responseString = response["response"].ToString();
                            var result = JsonConvert.DeserializeObject<StartTravelPremiumAPIResponse>(responseString);
                            int pre = Convert.ToInt32(view_Insurance.Premium)*18/100;
                            int datapre = pre + Convert.ToInt32(view_Insurance.Premium);
                            quotationDetail = new RegistrationDetails
                            {
                                sumAssured = view_Insurance.SumInsured.ToString(),
                                Amount = view_Insurance.Premium.ToString(),
                             //   serviceTax = result.serviceTax.ToString(),
                             serviceTax=pre.ToString(),
                                totalPremium = datapre.ToString(),

                                NoOfAdults = view_Insurance.Adults.ToString(),
                                NoOfChildren = view_Insurance.Childrens.ToString(),
                                Plan = view_Insurance.PremiumDesc,
                                startOn = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"),
                                endOn = DateTime.Now.AddYears(Convert.ToInt32(view_Insurance.Duration)).ToString("dd/MM/yyyy"),
                                period = Convert.ToInt32(view_Insurance.Duration),
                                policyCategory = "fresh",
                                schemeId = Convert.ToInt32(view_Insurance.SchemeId),
                                policyTypeName = policyTypeName,
                                searchId = _TId

                            };
                        }
                        break;
                    default:
                        break;
                }

                quotationDetail.insureds0 = new Insured { sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode) };
                quotationDetail.insureds1 = new Insured { sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode) };
                quotationDetail.insureds2 = new Insured { sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode) };
                quotationDetail.insureds3 = new Insured { sumInsuredId = Convert.ToInt32(view_Insurance.suminsuredCode) };
                quotationDetail.proposerName = InsuranceSearchBies.Name;
                quotationDetail.proposerPhone = InsuranceSearchBies.MobileNo;
                quotationDetail.proposerDob = DateTime.Now.AddYears(Convert.ToInt32("-" + InsuranceSearchBies.Self_Age)).ToString("dd/MM/yyyy");
                quotationDetail.planId = Convert.ToInt32(planId);
                quotationDetail.companyId = view_Insurance.CompanyId.ToString();


                return quotationDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }
        public string SubmitProposal(RegistrationDetails getQuotationDetail)
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            try
            {
                string postData = "";
                switch (getQuotationDetail.companyId)
                {
                    case "2":
                        postData = @"{
  'quoteCreationRequestHeader': {
    'requestId': 'QuoteCreation-31-Jan-2018-12:34:30-600050',
    'action': 'QuoteCreation',
    'transactionTimestamp': '01-Feb-2018-01:02:03',
    'channel': 'PORTAL',
    'username': '600050',
    'userRole': '',
    'user': {
      'username': '600050',
      'userRole': ''
    }
  },
  'payload': {
    'createquoterequest': {
      'insuredDetails': [
        {
          'nomineeDetails': {},
          'existingDisability': '',
          'realtionshipWithProposer': '0',
          'occupation': '',
          'plotNo': 'RatRob Building',
          'buildingName': 'dfh dhh',
          'nomineeRelationWithPrimaryInsured': '',
          'locationId': '704test49999',
          'state': 'Maharashtra',
          'streetName': 'RatRob Town',
          'city': 'Mumbai',
          'pinCode': '400059',
          'title': '',
          'emailId': '',
          'coverDetails': [
            {
              'coverAttributesDetails': {
                'basisOfValuationContent': '1',
                'basisOfValuationBuilding': '1',
                'basisSIContent': '1',
                'firstLoss': '',
                'ttlValueContents': '',
                'ttlContentSI': '',
                'coverSumInsured': ''
              },
              'coverTypeId': '900008282',
              'delete': 'N',
              'coverId': '41169700',
              'benefits': [
                {
                  'benefitName': 'Fire - Building',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005532'
                },
                {
                  'benefitName': 'Fire - Contents',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005531'
                },
                {
                  'benefitName': 'Furniture',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005533'
                },
                {
                  'benefitName': 'Clothing',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005534'
                },
                {
                  'benefitName': 'Domestic Elec. Appliances',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005535'
                },
                {
                  'benefitName': 'Crockery and Utensils',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005536'
                },
                {
                  'benefitName': 'Contents - Others',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005537'
                },
                {
                  'benefitName': 'Jewellery &amp; Valuables Item 1',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005538'
                },
                {
                  'benefitName': 'Jewellery &amp; Valuables Item 2',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005539'
                },
                {
                  'benefitName': 'Jewellery &amp; Valuables Item 3',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005540'
                },
                {
                  'benefitName': 'Jewellery &amp; Valuables Item 4',
                  'benefitId': '41169700',
                  'benefitSumInsured': '1000000',
                  'benefitTypeId': '900005541'
                }
              ],
              'coverTypeName': 'Fire &amp; Special Perils'
            },
            {
              'coverAttributesDetails': {},
              'coverTypeId': '900008283',
              'delete': 'N',
              'coverId': '41169780',
              'coverTypeName': 'Terrorism'
            },
            {
              'coverAttributesDetails': {
                'coverSumInsured': '2000000'
              },
              'coverTypeId': '900008294',
              'delete': 'N',
              'coverId': '41169800',
              'coverTypeName': 'Key Replacement'
            },
            {
              'coverAttributesDetails': {
                'coverNoOfMonths': '5',
                'monthlyRentCover': '5',
                'coverSumInsured': ''
              },
              'coverTypeId': '900008305',
              'delete': 'N',
              'coverId': '41169820',
              'coverTypeName': 'Loss of Rent'
            }
          ],
          'partyId': '0000000000135515',
          'dob': '',
          'gender': '',
          'locality': 'J.B. Nagar',
          'district': 'Mumbai',
          'mobileNumber': ''
        }
      ],
      'product': 'SHIP',
      'proposerDetails': {
        'lastName': 'Lotlikar',
        'plotNo': 'RatRob Building',
        'buildingName': 'dfh dhh',
        'location': 'J.B. Nagari',
        'streetName': 'RatRob Town',
        'city': '3000000400',
        'pinCode': '400059',
        'emailId': 'abc@gmail.com',
        'title': 'Mr',
        'address': 'MIDC, ADASF, YUIOI, Chandani, Burhanpur, 450221',
        'partyID': '0000000000135515',
        'dob': '1980-01-01T00:00:00.000+05:30',
        'gender': 'M',
        'firstName': 'Prashant abcd',
        'mobileNumber': '9820009790'
      },
      'clauses': [
        {}
      ],
      'locationDetails': {
        'yearOfConstruction': '2018',
        'fireExtAppliances': '4',
        'typeAdjArea': '1',
        'securityFeatures': '5',
        'builtArea': '2',
        'basementProperty': '2',
        'typeOfBuilding': '1'
      },
      'commercialConsideration': {
        'underwritingDiscount': '0',
        'underwriting_Loading': '0'
      },
      'quoteDetails': {
        'policyInfo': {
          'startDate': '2018-01-02T00:00:00.000+05:30',
          'channelType': '',
          'ttlClaimedAmt': '0',
          'numberPreviousClaims': '2',
          'endDate': '2021-01-01T23:59:59.999+05:30',
          'previousPolicy': '2',
          'policyDuration': '3'
        },
        'locationAddressDlt': '678#%lmn#%opq#%rst#%uvw#%xyz#%600113~58889',
        'insuredId': '676321',
        'quoteNo': '0000000000827739',
        'expiryDate': '2021-01-01T23:59:59.999+05:30',
        'policyID': '644436',
        'effectiveDate': '2018-01-02T00:00:00.000+05:30',
        'ppId': '12611460',
        'agreementCode': '2963'
      }
    }
  }
}
".Replace("'", "\"");
                        response = (new SBIHealthRequest()).QuoteCreation(postData);
                        break;
                    case "10006":

                        getQuotationDetail.APIKEY = apiKey;
                        getQuotationDetail.SECRETKEY = secretKey;
                        getQuotationDetail.startOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        getQuotationDetail.endOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        getQuotationDetail.proposerDob = DateTime.ParseExact(getQuotationDetail.proposerDob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");

                        //getQuotationDetail.startOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        //getQuotationDetail.endOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        //getQuotationDetail.proposerDob = DateTime.ParseExact(getQuotationDetail.proposerDob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        getQuotationDetail.insureds0.dob = DateTime.ParseExact(getQuotationDetail.insureds0.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //  // getQuotationDetail.startOn = Convert.ToDateTime(getQuotationDetail.startOn).ToString("dd-MM-yyyy").Replace("-", "/");
                     // //  getQuotationDetail.endOn = Convert.ToDateTime(getQuotationDetail.endOn).ToString("dd-MM-yyyy").Replace("-", "/");
                     // //  getQuotationDetail.proposerDob = Convert.ToDateTime(getQuotationDetail.proposerDob).ToString("dd-MM-yyyy").Replace("-", "/");

                     ////   getQuotationDetail.insureds0.dob = Convert.ToDateTime(getQuotationDetail.insureds0.dob).ToString("MM-dd-yyyy").Replace("-", "/");
                     //   //getQuotationDetail.insureds0.dob = Convert.ToDateTime(getQuotationDetail.insureds0.dob).ToString("dd/MM/yyyy").Replace("-", "/");
                     //   //getQuotationDetail.startOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //   //getQuotationDetail.endOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //   //getQuotationDetail.proposerDob = DateTime.ParseExact(getQuotationDetail.proposerDob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //   getQuotationDetail.startOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //   getQuotationDetail.endOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                     //   getQuotationDetail.proposerDob = DateTime.ParseExact(getQuotationDetail.proposerDob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");

                     //   getQuotationDetail.insureds0.dob = DateTime.ParseExact(getQuotationDetail.insureds0.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");
                        getQuotationDetail.socialStatusBpl = getQuotationDetail.socialStatus;
                        getQuotationDetail.socialStatusDisabled = getQuotationDetail.socialStatus;
                        getQuotationDetail.socialStatusInformal = getQuotationDetail.socialStatus;
                        getQuotationDetail.socialStatusUnorganized = getQuotationDetail.socialStatus;
                        //getQuotationDetail.insureds0.relationshipId = getQuotationDetail.insureds0.relationshipId != 1 ? 0 : getQuotationDetail.insureds0.relationshipId;
                        getQuotationDetail.insureds0.relationshipId = getQuotationDetail.insureds0.relationshipId;
                        getQuotationDetail.proposerAreaId = string.IsNullOrEmpty(getQuotationDetail.proposerAreaId) ? "" : getQuotationDetail.proposerAreaId;
                        getQuotationDetail.proposerResidenceAreaId = string.IsNullOrEmpty(getQuotationDetail.proposerResidenceAreaId) ? "" : getQuotationDetail.proposerResidenceAreaId;
                        if (getQuotationDetail.proposerResidenceAreaId == "0")
                        {
                            getQuotationDetail.proposerResidenceAreaId = "";
                        }
                        getQuotationDetail.proposerResidenceAreaPinCode = string.IsNullOrEmpty(getQuotationDetail.proposerResidenceAreaPinCode) ? "" : getQuotationDetail.proposerResidenceAreaPinCode;
                        getQuotationDetail.proposerResidenceAreaCityId = string.IsNullOrEmpty(getQuotationDetail.proposerResidenceAreaCityId) ? "" : getQuotationDetail.proposerResidenceAreaCityId;
                        string personalacciden = getQuotationDetail.insureds0.isPersonalAccidentApplicable;
                        getQuotationDetail.insureds0.isPersonalAccidentApplicable = getQuotationDetail.insureds0.illness == "Yes" ? "true" : "None";


                        if (getQuotationDetail.policyTypeName == "COMPREHENSIVE" || getQuotationDetail.policyTypeName == "COMPREHENSIVEIND")
                        {
                            getQuotationDetail.insureds0.isPersonalAccidentApplicable = personalacciden;
                        }
                        if (getQuotationDetail.policyTypeName == "COMPREHENSIVEIND")
                        {
                            getQuotationDetail.insureds0.isPersonalAccidentApplicable = "true";
                        }
                        getQuotationDetail.insureds0.buyBackPED = getQuotationDetail.insureds0.illness == "Yes" ? 1 : 0;
                        getQuotationDetail.insureds0.illness = getQuotationDetail.insureds0.illness == "Yes" ? getQuotationDetail.insureds0.description : "None";
                        if (getQuotationDetail.policyTypeName == "MCINEW" || !getQuotationDetail.policyTypeName.Contains("COMPREHENSIVE"))
                        {

                            getQuotationDetail.insureds0.isPersonalAccidentApplicable = "";
                        }
                        if (getQuotationDetail.insureds1.name != null)
                        {
                            getQuotationDetail.sumInsuredId = getQuotationDetail.insureds1.sumInsuredId;
                            getQuotationDetail.insureds1.isPersonalAccidentApplicable = getQuotationDetail.insureds1.illness == "Yes" ? "true" : "false";
                            getQuotationDetail.insureds1.buyBackPED = getQuotationDetail.insureds1.illness == "Yes" ? 0 : 0;
                            getQuotationDetail.insureds1.illness = getQuotationDetail.insureds1.illness == "Yes" ? getQuotationDetail.insureds1.description : "None";
                            // getQuotationDetail.insureds1.relationshipId = getQuotationDetail.insureds1.relationshipId != 1 ? 0 : getQuotationDetail.insureds1.relationshipId;
                            if (getQuotationDetail.insureds0.isPersonalAccidentApplicable == "false")
                            {
                                getQuotationDetail.insureds1.isPersonalAccidentApplicable = "true";
                            }
                            else if (getQuotationDetail.insureds0.isPersonalAccidentApplicable == "true")
                            {
                                getQuotationDetail.insureds1.isPersonalAccidentApplicable = "false";
                            }
                            getQuotationDetail.insureds1.relationshipId = getQuotationDetail.insureds1.relationshipId;
                            //getQuotationDetail.insureds1.dob = Convert.ToDateTime(getQuotationDetail.insureds1.dob).ToString("dd-MM-yyyy").Replace("-", "/");

                            getQuotationDetail.insureds1.dob = DateTime.ParseExact(getQuotationDetail.insureds1.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");


                        }
                        if (getQuotationDetail.insureds2.name != null)
                        {
                            getQuotationDetail.sumInsuredId = getQuotationDetail.insureds2.sumInsuredId;
                            getQuotationDetail.insureds2.isPersonalAccidentApplicable = getQuotationDetail.insureds2.illness == "Yes" ? "true" : "false";
                            getQuotationDetail.insureds2.buyBackPED = getQuotationDetail.insureds2.illness == "Yes" ? 0 : 0;
                            getQuotationDetail.insureds2.illness = getQuotationDetail.insureds2.illness == "Yes" ? getQuotationDetail.insureds2.description : "None";
                            // getQuotationDetail.insureds1.relationshipId = getQuotationDetail.insureds1.relationshipId != 1 ? 0 : getQuotationDetail.insureds1.relationshipId;
                            getQuotationDetail.insureds2.relationshipId = getQuotationDetail.insureds2.relationshipId;
                            //getQuotationDetail.insureds2.dob = Convert.ToDateTime(getQuotationDetail.insureds2.dob).ToString("dd-MM-yyyy").Replace("-", "/");

                            getQuotationDetail.insureds2.dob = DateTime.ParseExact(getQuotationDetail.insureds2.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");


                        }
                        if (getQuotationDetail.insureds3.name != null && getQuotationDetail.insureds3.relationshipId != null)
                        {
                            getQuotationDetail.sumInsuredId = getQuotationDetail.insureds3.sumInsuredId;
                            getQuotationDetail.insureds3.isPersonalAccidentApplicable = getQuotationDetail.insureds3.illness == "Yes" ? "true" : "false";
                            getQuotationDetail.insureds3.buyBackPED = getQuotationDetail.insureds3.illness == "Yes" ? 0 : 0;
                            getQuotationDetail.insureds3.illness = getQuotationDetail.insureds3.illness == "Yes" ? getQuotationDetail.insureds3.description : "None";
                            // getQuotationDetail.insureds1.relationshipId = getQuotationDetail.insureds1.relationshipId != 1 ? 0 : getQuotationDetail.insureds1.relationshipId;
                            getQuotationDetail.insureds3.relationshipId = getQuotationDetail.insureds3.relationshipId;
                            //getQuotationDetail.insureds3.dob = Convert.ToDateTime(getQuotationDetail.insureds3.dob).ToString("dd-MM-yyyy").Replace("-", "/");

                            getQuotationDetail.insureds3.dob = DateTime.ParseExact(getQuotationDetail.insureds3.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MMMM dd, yyyy");


                        }
                        getQuotationDetail.insureds0.description = "";
                        getQuotationDetail.insureds1.description = "";
                        getQuotationDetail.insureds2.description = "";
                        getQuotationDetail.insureds3.description = "";

                        postData = JsonConvert.SerializeObject(getQuotationDetail).Replace("null", "\"\"");

                        postData = postData.Replace("\"hospitalCash\":\"\"", "\"hospitalCash\":\"0\"");
                        if (!getQuotationDetail.policyTypeName.Contains("COMPREHENSIVE") || !getQuotationDetail.policyTypeName.Contains("MCINEW"))
                        {

                            postData = postData.Replace(",\"insureds2\":\"\",", ",");
                            postData = postData.Replace(",\"isPersonalAccidentApplicable\":\"\",\"engageManualLabour\":\"\",\"engageWinterSports\":\"\",\"buyBackPED\":0", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":1,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":1,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":1,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":2,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":2,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":2,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":4,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":4,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":4,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":5,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":5,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":5,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":6,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":6,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":6,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":7,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":7,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":7,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":8,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":8,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":8,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":9,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":9,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":9,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds1\":{\"dob\":\"\",\"sumInsuredId\":10,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds2\":{\"dob\":\"\",\"sumInsuredId\":10,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"insureds3\":{\"dob\":\"\",\"sumInsuredId\":10,\"name\":\"\",\"sex\":\"\",\"illness\":\"\",\"description\":\"\",\"relationshipId\":0,\"occupationId\":0,\"hospitalCash\":\"0\",\"height\":0,\"weight\":0}", "");
                            postData = postData.Replace(",\"description\":\"\"", "");
                        }
                        response = requestHandler.Response(baseUrl + "/policy/proposals", postData.Replace("insureds0", "insureds[0]").Replace("insureds1", "insureds[1]").Replace("insureds2", "insureds[2]").Replace("insureds3", "insureds[3]"));

                        break;
                    case "10009":

                        int totalmem = 1; int agespouse = 0;
                        int agechild1 = 0; int totalmem1 = 0; int totalmem2 = 0; int totalmem3 = 0;
                        int agechild2 = 0;
                        DateTime dob = Convert.ToDateTime(getQuotationDetail.insureds0.dob);
                        getQuotationDetail.startOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        getQuotationDetail.endOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        getQuotationDetail.insureds0.dob = DateTime.ParseExact(getQuotationDetail.insureds0.dob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        if (getQuotationDetail.insureds1.dob != null)
                        {
                            getQuotationDetail.insureds1.dob = DateTime.ParseExact(getQuotationDetail.insureds1.dob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                            DateTime dob1 = Convert.ToDateTime(getQuotationDetail.insureds1.dob);
                            TimeSpan tm1 = (DateTime.Now - dob1);
                            agespouse = (tm1.Days / 365);
                            totalmem1 = 1;
                        }
                        else
                        {
                            getQuotationDetail.insureds1.engageManualLabour = "";
                        }
                        if (getQuotationDetail.insureds2.dob != null)
                        {
                            getQuotationDetail.insureds2.dob = DateTime.ParseExact(getQuotationDetail.insureds2.dob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                            DateTime dob2 = Convert.ToDateTime(getQuotationDetail.insureds2.dob);
                            TimeSpan tm2 = (DateTime.Now - dob2);
                            agechild1 = (tm2.Days / 365);
                            totalmem2 = 1;
                        }
                        else
                        {
                            getQuotationDetail.insureds2.engageManualLabour = "";
                        }
                        if (getQuotationDetail.insureds3.dob != null)
                        {
                            getQuotationDetail.insureds3.dob = DateTime.ParseExact(getQuotationDetail.insureds3.dob, "dd-MM-yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                            DateTime dob3 = Convert.ToDateTime(getQuotationDetail.insureds3.dob);
                            TimeSpan tm3 = (DateTime.Now - dob3);
                            agechild2 = (tm3.Days / 365);
                            totalmem3 = 1;
                        }
                        else
                        {
                            getQuotationDetail.insureds3.engageManualLabour = "";
                        }
                        int totalmember = totalmem + totalmem1 + totalmem2 + totalmem3;
                        TimeSpan tm = (DateTime.Now - dob);
                        getQuotationDetail.insureds0.sex = getQuotationDetail.insureds0.sex == "Male" ? "M" : "F";
                        getQuotationDetail.insureds1.sex = getQuotationDetail.insureds1.sex == "Male" ? "M" : "F";
                        getQuotationDetail.insureds2.sex = getQuotationDetail.insureds2.sex == "Male" ? "M" : "F";
                        getQuotationDetail.insureds3.sex = getQuotationDetail.insureds3.sex == "Male" ? "M" : "F";


                        int age = (tm.Days / 365);

                        var zone = "";
                        var zoneid = db.tbl_BajajZone.AsEnumerable().Where(x => x.Zone == getQuotationDetail.proposerAreaCityId).ToList();
                        if (zoneid.Count != 0)
                        {
                            zone = "1";
                        }
                        else
                        {
                            zone = "2";
                        }
                        var result = "";
                        getQuotationDetail.proposerAreaCityId = getQuotationDetail.proposerAreaCityId.ToUpper();
                        var CompanyId = db.tbl_BajajBlockCity.AsEnumerable().Where(x => x.PolicyCity == getQuotationDetail.proposerAreaCityId).ToList();
                        //  string d = CompanyId.ToString();
                        if (CompanyId.Count == 0)
                        {
                            if (getQuotationDetail.policyCategory == "1A")
                            {
                                result = "{ 'userid': '" + BajajUser + "', 'password': '" + BajajPassword + "', 'sourcedtls': { 'username': '', 'departmentcode': '9906', 'productcode': '" + getQuotationDetail.schemeId + "', 'businesstype': 'NB', 'imdcode': '10043080', 'subimdcode': '0', 'modulename': 'WEBSERVICE' }, 'policydtls': { 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'paymentmode': 'CC', 'partnertype': 'P', 'deptcode': '84', 'businesstype': 'NB', 'policyperiod': '1200', 'productcode': '" + getQuotationDetail.schemeId + "', 'scrlocationcode': '9906', 'scrcategory': 'NB' }, 'tycpdetails': { 'partnertype': 'P', 'beforetitle': 'Mr', 'contact1': '" + getQuotationDetail.proposerPhone + "', 'dateofbirth': '" + getQuotationDetail.insureds0.dob + "', 'sex': '" + getQuotationDetail.insureds0.sex + "', 'telephone': '', 'email': '" + getQuotationDetail.proposerEmail + "', 'firstname': '" + getQuotationDetail.insureds0.name + "', 'surname': '', 'middlename': '' }, 'tycpaddrlist': [ { 'postcode': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline1': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline2': '', 'addressline3': '" + getQuotationDetail.proposerAddressOne + "', 'addressline4': '', 'addressline5': 'INDIA', 'state': '' } ], 'channeldtls': { 'imdcode': '10057150', 'subimdcode': '0', 'bankrmecode': '3321', 'bankcustid': 'bankref1', 'loanaccno': 'ABNX121231', 'bankrmename': 'Test Name' }, 'previnsdtls': { 'previnsname': '', 'previnsaddress': '', 'previnspolicyno': '', 'prevpolicyexpirydate': '', 'noofclaims': '0' }, 'hcpdtpreobj': { 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "' }, 'hcpdtmemlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memrelation': '" + getQuotationDetail.nomineeRelationship + "', 'memdob': '" + getQuotationDetail.insureds0.dob + "', 'memage': '" + age + "', 'memgender': '" + getQuotationDetail.insureds0.sex + "', 'memheightcm': '" + getQuotationDetail.insureds0.height + "', 'memweightkg': '" + getQuotationDetail.insureds0.weight + "', 'membmi': '18.4', 'memoccupation': '" + getQuotationDetail.insureds0.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeName + "', 'memnomineerelation': '" + getQuotationDetail.nomineeRelationshipTwo + "', 'mempreexistdisease': '" + getQuotationDetail.insureds0.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }], 'hcpdtmemcovlist': [ { 'memiptreatsi': '" + getQuotationDetail.sumAssured + "', 'memaddflag': 'Y' } ], 'hcpdtpolcovobj': { 'polcovzone': '" + getQuotationDetail.socialStatusUnorganized + "', 'polcovvolntrycp': '" + getQuotationDetail.eiaNumber + "', 'polcovspcndt': 'NA', 'polcovmedcnd': 'N', 'polcov91': 'NA' }, 'hcpdtmemcovaddonlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memaddflag': 'Y' }], 'hcpstagedataobj': { 'busstype': 'NB', 'productcode': '" + getQuotationDetail.schemeId + "', 'subimdcode': '0', 'locationcode': '1501', 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "', 'totalmembernos': '" + totalmember + "', 'partnerpincode': '400706' }, 'rcptlist': [ { } ], 'servicetaxmstobj': { }, 'genpremdtls': { }, 'hcpdpolcaddobj': { }, 'geniclist': [ { } ], 'receiptftcobj': { }, 'gencoinslist': [ { } ], 'scripdtlist': [ { } ], 'endtdtls': { }, 'refdtlist': [ { } ], 'paymenttransobj': { }, 'transactionid': '" + getQuotationDetail.sumInsuredId + "' }";
                            }
                            else if (getQuotationDetail.policyCategory == "2A")
                            {
                                result = "{ 'userid': '" + BajajUser + "', 'password': '" + BajajPassword + "', 'sourcedtls': { 'username': '', 'departmentcode': '9906', 'productcode': '" + getQuotationDetail.schemeId + "', 'businesstype': 'NB', 'imdcode': '10043080', 'subimdcode': '0', 'modulename': 'WEBSERVICE' }, 'policydtls': { 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'paymentmode': 'CC', 'partnertype': 'P', 'deptcode': '84', 'businesstype': 'NB', 'policyperiod': '1200', 'productcode': '" + getQuotationDetail.schemeId + "', 'scrlocationcode': '9906', 'scrcategory': 'NB' }, 'tycpdetails': { 'partnertype': 'P', 'beforetitle': 'Mr', 'contact1': '" + getQuotationDetail.proposerPhone + "', 'dateofbirth': '" + getQuotationDetail.insureds0.dob + "', 'sex': '" + getQuotationDetail.insureds0.sex + "', 'telephone': '', 'email': '" + getQuotationDetail.proposerEmail + "', 'firstname': '" + getQuotationDetail.insureds0.name + "', 'surname': '', 'middlename': '' }, 'tycpaddrlist': [ { 'postcode': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline1': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline2': '', 'addressline3': '" + getQuotationDetail.proposerAddressOne + "', 'addressline4': '', 'addressline5': 'INDIA', 'state': '' } ], 'channeldtls': { 'imdcode': '10057150', 'subimdcode': '0', 'bankrmecode': '3321', 'bankcustid': 'bankref1', 'loanaccno': 'ABNX121231', 'bankrmename': 'Test Name' }, 'previnsdtls': { 'previnsname': '', 'previnsaddress': '', 'previnspolicyno': '', 'prevpolicyexpirydate': '', 'noofclaims': '0' }, 'hcpdtpreobj': { 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "' }, 'hcpdtmemlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memrelation': '" + getQuotationDetail.nomineeRelationship + "', 'memdob': '" + getQuotationDetail.insureds0.dob + "', 'memage': '" + age + "', 'memgender': '" + getQuotationDetail.insureds0.sex + "', 'memheightcm': '" + getQuotationDetail.insureds0.height + "', 'memweightkg': '" + getQuotationDetail.insureds0.weight + "', 'membmi': '18.4', 'memoccupation': '" + getQuotationDetail.insureds0.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeName + "', 'memnomineerelation': '" + getQuotationDetail.nomineeRelationshipTwo + "', 'mempreexistdisease': '" + getQuotationDetail.insureds0.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memrelation': '" + getQuotationDetail.insureds1.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds1.dob + "', 'memage': '" + agespouse + "', 'memgender': '" + getQuotationDetail.insureds1.sex + "', 'memheightcm': '" + getQuotationDetail.insureds1.height + "', 'memweightkg': '" + getQuotationDetail.insureds1.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds1.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeNameTwo + "', 'memnomineerelation': '" + getQuotationDetail.criticalIllness + "', 'mempreexistdisease': '" + getQuotationDetail.insureds1.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }], 'hcpdtmemcovlist': [ { 'memiptreatsi': '" + getQuotationDetail.sumAssured + "', 'memaddflag': 'Y' } ], 'hcpdtpolcovobj': { 'polcovzone': '" + getQuotationDetail.socialStatusUnorganized + "', 'polcovvolntrycp': '" + getQuotationDetail.eiaNumber + "', 'polcovspcndt': 'NA', 'polcovmedcnd': 'N', 'polcov91': 'NA' }, 'hcpdtmemcovaddonlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memaddflag': 'Y' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memaddflag': 'Y' }  ], 'hcpstagedataobj': { 'busstype': 'NB', 'productcode': '" + getQuotationDetail.schemeId + "', 'subimdcode': '0', 'locationcode': '1501', 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "', 'totalmembernos': '" + totalmember + "', 'partnerpincode': '400706' }, 'rcptlist': [ { } ], 'servicetaxmstobj': { }, 'genpremdtls': { }, 'hcpdpolcaddobj': { }, 'geniclist': [ { } ], 'receiptftcobj': { }, 'gencoinslist': [ { } ], 'scripdtlist': [ { } ], 'endtdtls': { }, 'refdtlist': [ { } ], 'paymenttransobj': { }, 'transactionid': '" + getQuotationDetail.sumInsuredId + "' }";
                            }
                            else if (getQuotationDetail.policyCategory == "2A+1C")
                            {
                                result = "{ 'userid': '" + BajajUser + "', 'password': '" + BajajPassword + "', 'sourcedtls': { 'username': '', 'departmentcode': '9906', 'productcode': '" + getQuotationDetail.schemeId + "', 'businesstype': 'NB', 'imdcode': '10043080', 'subimdcode': '0', 'modulename': 'WEBSERVICE' }, 'policydtls': { 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'paymentmode': 'CC', 'partnertype': 'P', 'deptcode': '84', 'businesstype': 'NB', 'policyperiod': '1200', 'productcode': '" + getQuotationDetail.schemeId + "', 'scrlocationcode': '9906', 'scrcategory': 'NB' }, 'tycpdetails': { 'partnertype': 'P', 'beforetitle': 'Mr', 'contact1': '" + getQuotationDetail.proposerPhone + "', 'dateofbirth': '" + getQuotationDetail.insureds0.dob + "', 'sex': '" + getQuotationDetail.insureds0.sex + "', 'telephone': '', 'email': '" + getQuotationDetail.proposerEmail + "', 'firstname': '" + getQuotationDetail.insureds0.name + "', 'surname': '', 'middlename': '' }, 'tycpaddrlist': [ { 'postcode': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline1': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline2': '', 'addressline3': '" + getQuotationDetail.proposerAddressOne + "', 'addressline4': '', 'addressline5': 'INDIA', 'state': '' } ], 'channeldtls': { 'imdcode': '10057150', 'subimdcode': '0', 'bankrmecode': '3321', 'bankcustid': 'bankref1', 'loanaccno': 'ABNX121231', 'bankrmename': 'Test Name' }, 'previnsdtls': { 'previnsname': '', 'previnsaddress': '', 'previnspolicyno': '', 'prevpolicyexpirydate': '', 'noofclaims': '0' }, 'hcpdtpreobj': { 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "' }, 'hcpdtmemlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memrelation': '" + getQuotationDetail.nomineeRelationship + "', 'memdob': '" + getQuotationDetail.insureds0.dob + "', 'memage': '" + age + "', 'memgender': '" + getQuotationDetail.insureds0.sex + "', 'memheightcm': '" + getQuotationDetail.insureds0.height + "', 'memweightkg': '" + getQuotationDetail.insureds0.weight + "', 'membmi': '18.4', 'memoccupation': '" + getQuotationDetail.insureds0.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeName + "', 'memnomineerelation': '" + getQuotationDetail.nomineeRelationshipTwo + "', 'mempreexistdisease': '" + getQuotationDetail.insureds0.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memrelation': '" + getQuotationDetail.insureds1.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds1.dob + "', 'memage': '" + agespouse + "', 'memgender': '" + getQuotationDetail.insureds1.sex + "', 'memheightcm': '" + getQuotationDetail.insureds1.height + "', 'memweightkg': '" + getQuotationDetail.insureds1.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds1.occupationId + "', 'memgrossmonthlyincome': '0',  'memnomineename': '" + getQuotationDetail.nomineeNameTwo + "', 'memnomineerelation': '" + getQuotationDetail.criticalIllness + "', 'mempreexistdisease': '" + getQuotationDetail.insureds1.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' },  { 'membername': '" + getQuotationDetail.insureds2.name + "', 'memrelation': '" + getQuotationDetail.insureds2.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds2.dob + "', 'memage': '" + agechild1 + "', 'memgender': '" + getQuotationDetail.insureds2.sex + "', 'memheightcm': '" + getQuotationDetail.insureds2.height + "', 'memweightkg': '" + getQuotationDetail.insureds2.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds2.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineePercentClaimTwo + "', 'memnomineerelation': '" + getQuotationDetail.previousMedicalInsurance + "', 'mempreexistdisease': '" + getQuotationDetail.insureds2.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }], 'hcpdtmemcovlist': [ { 'memiptreatsi': '" + getQuotationDetail.sumAssured + "', 'memaddflag': 'Y' } ], 'hcpdtpolcovobj': { 'polcovzone': '" + getQuotationDetail.socialStatusUnorganized + "', 'polcovvolntrycp': '" + getQuotationDetail.eiaNumber + "', 'polcovspcndt': 'NA', 'polcovmedcnd': 'N', 'polcov91': 'NA' }, 'hcpdtmemcovaddonlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memaddflag': 'Y' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memaddflag': 'Y' } , { 'membername': '" + getQuotationDetail.insureds2.name + "', 'memaddflag': 'Y' } ], 'hcpstagedataobj': { 'busstype': 'NB', 'productcode': '" + getQuotationDetail.schemeId + "', 'subimdcode': '0', 'locationcode': '1501', 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "', 'totalmembernos': '" + totalmember + "', 'partnerpincode': '400706' }, 'rcptlist': [ { } ], 'servicetaxmstobj': { }, 'genpremdtls': { }, 'hcpdpolcaddobj': { }, 'geniclist': [ { } ], 'receiptftcobj': { }, 'gencoinslist': [ { } ], 'scripdtlist': [ { } ], 'endtdtls': { }, 'refdtlist': [ { } ], 'paymenttransobj': { }, 'transactionid': '" + getQuotationDetail.sumInsuredId + "' }";
                            }
                            else if (getQuotationDetail.policyCategory == "2A+2C")
                            {

                                result = "{ 'userid': '" + BajajUser + "', 'password': '" + BajajPassword + "', 'sourcedtls': { 'username': '', 'departmentcode': '9906', 'productcode': '" + getQuotationDetail.schemeId + "', 'businesstype': 'NB', 'imdcode': '10043080', 'subimdcode': '0', 'modulename': 'WEBSERVICE' }, 'policydtls': { 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'paymentmode': 'CC', 'partnertype': 'P', 'deptcode': '84', 'businesstype': 'NB', 'policyperiod': '1200', 'productcode': '" + getQuotationDetail.schemeId + "', 'scrlocationcode': '9906', 'scrcategory': 'NB' }, 'tycpdetails': { 'partnertype': 'P', 'beforetitle': 'Mr', 'contact1': '" + getQuotationDetail.proposerPhone + "', 'dateofbirth': '" + getQuotationDetail.insureds0.dob + "', 'sex': '" + getQuotationDetail.insureds0.sex + "', 'telephone': '', 'email': '" + getQuotationDetail.proposerEmail + "', 'firstname': '" + getQuotationDetail.insureds0.name + "', 'surname': '', 'middlename': '' }, 'tycpaddrlist': [ { 'postcode': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline1': '" + getQuotationDetail.proposerAreaPinCode + "', 'addressline2': '', 'addressline3': '" + getQuotationDetail.proposerAddressOne + "', 'addressline4': '', 'addressline5': 'INDIA', 'state': '' } ], 'channeldtls': { 'imdcode': '10057150', 'subimdcode': '0', 'bankrmecode': '3321', 'bankcustid': 'bankref1', 'loanaccno': 'ABNX121231', 'bankrmename': 'Test Name' }, 'previnsdtls': { 'previnsname': '', 'previnsaddress': '', 'previnspolicyno': '', 'prevpolicyexpirydate': '', 'noofclaims': '0' }, 'hcpdtpreobj': { 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "' }, 'hcpdtmemlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memrelation': '" + getQuotationDetail.nomineeRelationship + "', 'memdob': '" + getQuotationDetail.insureds0.dob + "', 'memage': '" + age + "', 'memgender': '" + getQuotationDetail.insureds0.sex + "', 'memheightcm': '" + getQuotationDetail.insureds0.height + "', 'memweightkg': '" + getQuotationDetail.insureds0.weight + "', 'membmi': '18.4', 'memoccupation': '" + getQuotationDetail.insureds0.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeName + "', 'memnomineerelation': '" + getQuotationDetail.nomineeRelationshipTwo + "', 'mempreexistdisease': '" + getQuotationDetail.insureds0.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memrelation': '" + getQuotationDetail.insureds1.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds1.dob + "', 'memage': '" + agespouse + "', 'memgender': '" + getQuotationDetail.insureds1.sex + "', 'memheightcm': '" + getQuotationDetail.insureds1.height + "', 'memweightkg': '" + getQuotationDetail.insureds1.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds1.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineeNameTwo + "', 'memnomineerelation': '" + getQuotationDetail.criticalIllness + "', 'mempreexistdisease': '" + getQuotationDetail.insureds1.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' },  { 'membername': '" + getQuotationDetail.insureds2.name + "', 'memrelation': '" + getQuotationDetail.insureds2.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds2.dob + "', 'memage': '" + agechild1 + "', 'memgender': '" + getQuotationDetail.insureds2.sex + "', 'memheightcm': '" + getQuotationDetail.insureds2.height + "', 'memweightkg': '" + getQuotationDetail.insureds2.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds2.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.nomineePercentClaimTwo + "', 'memnomineerelation': '" + getQuotationDetail.previousMedicalInsurance + "', 'mempreexistdisease': '" + getQuotationDetail.insureds2.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' },  { 'membername': '" + getQuotationDetail.insureds3.name + "', 'memrelation': '" + getQuotationDetail.insureds3.engageManualLabour.ToString() + "', 'memdob': '" + getQuotationDetail.insureds3.dob + "', 'memage': '" + agechild2 + "', 'memgender': '" + getQuotationDetail.insureds3.sex + "', 'memheightcm': '" + getQuotationDetail.insureds3.height + "', 'memweightkg': '" + getQuotationDetail.insureds3.weight + "', 'membmi': '18.3', 'memoccupation': '" + getQuotationDetail.insureds3.occupationId + "', 'memgrossmonthlyincome': '0', 'memnomineename': '" + getQuotationDetail.socialStatusBpl + "', 'memnomineerelation': '" + getQuotationDetail.socialStatusDisabled + "', 'mempreexistdisease': '" + getQuotationDetail.insureds3.illness + "', 'memspecialcondition': 'NA', 'memsmkertbco': '0', 'memasthma': '0', 'memcholstrldisordr': '0', 'memheartdisease': '0', 'memhypertension': '0', 'memdiabetes': '0', 'memobesity': '0', 'memcompname': '', 'memprvpolno': '', 'memprvexpdate': '', 'memprvsi': '', 'memaddflag': 'Y', 'bahcpdtmemparam91': 'NA', 'bahcpdtmemparam41': '' }], 'hcpdtmemcovlist': [ { 'memiptreatsi': '" + getQuotationDetail.sumAssured + "', 'memaddflag': 'Y' } ], 'hcpdtpolcovobj': { 'polcovzone': '" + getQuotationDetail.socialStatusUnorganized + "', 'polcovvolntrycp': '" + getQuotationDetail.eiaNumber + "', 'polcovspcndt': 'NA', 'polcovmedcnd': 'N', 'polcov91': 'NA' }, 'hcpdtmemcovaddonlist': [ { 'membername': '" + getQuotationDetail.insureds0.name + "', 'memaddflag': 'Y' }, { 'membername': '" + getQuotationDetail.insureds1.name + "', 'memaddflag': 'Y' } , { 'membername': '" + getQuotationDetail.insureds2.name + "', 'memaddflag': 'Y' } , { 'membername': '" + getQuotationDetail.insureds3.name + "', 'memaddflag': 'Y' } ], 'hcpstagedataobj': { 'busstype': 'NB', 'productcode': '" + getQuotationDetail.schemeId + "', 'subimdcode': '0', 'locationcode': '1501', 'termstartdate': '" + getQuotationDetail.startOn + "', 'termenddate': '" + getQuotationDetail.endOn + "', 'selfcoveredflag': 'Y', 'membercombo': '" + getQuotationDetail.policyCategory.Replace("1A", "1") + "', 'totalmembernos': '" + totalmember + "', 'partnerpincode': '400706' }, 'rcptlist': [ { } ], 'servicetaxmstobj': { }, 'genpremdtls': { }, 'hcpdpolcaddobj': { }, 'geniclist': [ { } ], 'receiptftcobj': { }, 'gencoinslist': [ { } ], 'scripdtlist': [ { } ], 'endtdtls': { }, 'refdtlist': [ { } ], 'paymenttransobj': { }, 'transactionid': '" + getQuotationDetail.sumInsuredId + "' }";
                            }
                            result = result.Replace("'", "\"");
                            response = requestHandler.Response("https://webservices.bajajallianz.com/BagicHealthWebservice/healthissuepolicy", result);


                            if (response["Status"] == "200")
                            {
                                RegistrationDetails health = new RegistrationDetails();
                                var responseString = response["response"].ToString();
                                JObject joResponse = JObject.Parse(responseString);
                                JObject ojObject = (JObject)joResponse["genpremdtlsres"];
                                JObject ojObject2 = (JObject)joResponse["servicetaxmstobj"];
                                var responce = ojObject.ToString();
                                var responce2 = ojObject2.ToString();
                                var bajajresult = JsonConvert.DeserializeObject<Genpremdtlsres>(responce);
                                var bajajrefresult = JsonConvert.DeserializeObject<RootObject>(responseString);
                                var bajajref = JsonConvert.DeserializeObject<Servicetaxmstobj>(responce2);
                                getQuotationDetail.totalPremium = bajajresult.totalpremium;
                                getQuotationDetail.serviceTax = bajajref.addtaxval4;
                                getQuotationDetail.Amount = bajajresult.netpremium;
                                var proposalDetail = new TravelProposalDetail
                                {
                                    BilledDate = result != null ? DateTime.Now : Convert.ToDateTime("01/01/1900"),
                                    FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                                    IsBillPayed = result != null ? true : false,
                                    ReferenceId = bajajrefresult.transactionid != null ? bajajrefresult.transactionid.ToString() : "",
                                    RefType = "Health"

                                };
                                db.TravelProposalDetails.Add(proposalDetail);
                                db.SaveChanges();
                                return proposalDetail.ReferenceId;
                            }
                            else
                            {
                                //handle bad request response
                                return response["response"];
                            }
                        }
                        else
                        {
                            //handle bad request response
                            return response["response"];
                        }
                        //bajajrefresult.transactionid = bajajrefresult.transactionid;
                        //bajajresult.contractid = bajajresult.contractid;
                        //bajajresult.quoterefno = bajajresult.quoterefno;
                        //bajajresult.refno = bajajresult.refno;
                        //bajajresult.scrutinyno = bajajresult.scrutinyno;
                        //bajajresult.policyref = bajajresult.policyref;
                        //bajajresult.netpremium = bajajresult.netpremium;
                        //bajajresult.servicetaxamt = bajajresult.servicetaxamt;
                        //bajajresult.educessamt = bajajresult.educessamt;
                        //bajajresult.finalpremium = bajajresult.finalpremium;
                        //bajajref.taxcode = bajajref.taxcode;
                        //bajajref.taxtype = bajajref.taxtype;
                        //bajajref.addtaxval4 = bajajref.addtaxval4;




                        break;
                    default:

                        break;
                }
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    var result = JsonConvert.DeserializeObject<HealthProposalResponse>(responseString);
                    var proposalDetail = new TravelProposalDetail
                    {
                        BilledDate = result != null ? DateTime.Now : Convert.ToDateTime("01/01/1900"),
                        FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                        IsBillPayed = result != null ? true : false,
                        ReferenceId = result != null ? result.referenceId : "",
                        RefType = "Health"
                    };
                    db.TravelProposalDetails.Add(proposalDetail);
                    db.SaveChanges();
                    Console.WriteLine(response);
                    return proposalDetail.ReferenceId;
                }
                else
                {
                    //handle bad request response
                    return response["response"];
                }
                //  return response;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            return string.Empty;
        }
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
                    // error response handle code
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public Dictionary<string, string> PolicyStatus(string purchaseToken)
        {
            Dictionary<string, string> dicObj = new Dictionary<string, string>();
            try
            {
                var postdata = new { APIKEY = apiKey, SECRETKEY = secretKey, purchaseToken = purchaseToken };
                string postDataJSON = JsonConvert.SerializeObject(postdata);
                var response = requestHandler.PurchaseStatus(postDataJSON, purchaseToken);
                if (response["Status"] == "200")
                {
                    var responseStringPurchaseStatus = response["response"].ToString();
                    ResponsePurchaseToken resultPurchaseToken = JsonConvert.DeserializeObject<ResponsePurchaseToken>(responseStringPurchaseStatus);
                    if (resultPurchaseToken != null)
                    {
                        dicObj.Add("PolicyStatus", resultPurchaseToken.status);
                        dicObj.Add("RefId", resultPurchaseToken.referenceId);
                        var postdataPolicy = new { APIKEY = apiKey, SECRETKEY = secretKey, referenceId = resultPurchaseToken.referenceId };
                        postDataJSON = JsonConvert.SerializeObject(postdataPolicy);
                        response = requestHandler.PolicyStatus(postDataJSON, resultPurchaseToken.referenceId);
                        if (response["Status"] == "200")
                        {
                            var responseString = response["response"].ToString();
                            if (responseString.Contains("Note"))
                            {
                                var result = JsonConvert.DeserializeObject(responseString);
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
                            // Code for handle error response
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

        public List<City> GetCityList(string pinCode)
        {

            var cityList = new List<City>();

            cityList.Add(new City { city_id = 0, city_name = "Select" });
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
                        //cityList.Add(result.city[0].city_id.ToString());
                        //cityList.Add(result.city[0].city_name.ToString());
                        cityList.AddRange(result.city);

                        //var Cities = db.tbl_BlockCity.AsQueryable().AsEnumerable().Where(x => x.PinCode == Convert.ToInt32(pinCode)).Select(x => new destinationList { text = x.BlockCity, value = x.BlockCity }).Distinct().ToList();
                        return cityList;
                        //}
                    }
                    else
                    {
                        var errorVal = responseCityList["response"].ToString();
                        //  cityList.Add(errorVal.Replace("{", "").Replace("}", "").Replace("\"", ""));
                        return cityList;

                    }
                }

            }
            catch (Exception ex)
            {
                return new List<City>();

            }
            return cityList;
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
                //return new List<Area>();

            }
            return areaList;
        }
        //................Policy Status Save...........
        //public long SavePolicy(PolicyStatusDetails srchHealthlIns)
        //{
        //    try
        //    {
        //        Random r = new Random();
        //        int num = Convert.ToInt32(RandomNum());

        //        var tDetails = new tbl_StatusPolicy
        //        {

        //            Refnumber = srchHealthlIns.refrenceNumber,
        //            PolicyNumber = srchHealthlIns.PolicyNumber,
        //            RefID = srchHealthlIns.refID,
        //            Status = srchHealthlIns.Status,
        //            Tag = "true",
        //        };

        //        db.tbl_StatusPolicy.Add(tDetails);
        //        db.SaveChanges();

        //       // var tId = Convert.ToInt64(tDetails.num.tost);
        //      int  tId = 0;
        //        return tId;
             
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return 0;

        //}

    }
}
