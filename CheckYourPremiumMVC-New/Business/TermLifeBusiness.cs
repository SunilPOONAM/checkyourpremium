using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;
using Domain;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Web.Mvc;

namespace Business
{
    public interface ITermLifeBusiness
    {

        List<destinationList> GetAllCity(string state);
        List<customSelectList> GetSuminsured();//pppppppppppppppppppppppppp.......
        List<customSelectList> BindRiskOptions();//pppppppppppp
        List<customSelectList> BindcoverOptions();
        List<customSelectList> GetPolicyTerm();
        List<customSelectList> GetFrequency();
        List<customSelectList> MarialStatus();
        List<customSelectList> GetGender();
        List<customSelectList> BindState();
        List<customSelectList> PremiumType();
        //ICICI Bind Data
        List<customSelectList> Organaisation_Name();
        List<customSelectList> Occupation();
        List<customSelectList> GetICICIState();
        List<customSelectList> GetCountry();
        List<customSelectList> GetNationality();
        List<customSelectList> GetICICIMaritalStatus();
        List<customSelectList> GetEducation();
        List<customSelectList> GetTrusteeType();
        //For Kotak
        List<destinationList> GetKotakplan(string ptype);
        Kotak GetQuotationDetailKotak(Kotak zpmodel);
        //For Compare
        List<AllCompanyDetails> GetAllCompanyDetails(AllCompanyDetails objuserlhealth);
        List<Compare_Term> GetCompareDetails(Compare_Term objuserlhealth);
        List<GetIndiaFirstResponceDetail> GetPremiumList(GetIndiaFirstResponceDetail srchTrvlIns);
        List<GetIndiaFirstResponceDetail> GetPremiumListHDFC(GetIndiaFirstResponceDetail srchTrvlIns);
        List<GetIndiaFirstResponceDetail> GetPremiumEdelWises(GetIndiaFirstResponceDetail srchTremIns);
        List<GetIndiaFirstResponceDetail> GetPremiumEdelWisesResponce(GetIndiaFirstResponceDetail srchTremIns);
        List<GetIndiaFirstResponceDetail> GetPremiumListICICI(GetIndiaFirstResponceDetail srchTrvlIns);//pppppppp
        List<GetIndiaFirstResponceDetail> GetPremiumListKotak(GetIndiaFirstResponceDetail srchTrvlIns);//pppppp
        GetICICIResponseDetails SubmitProposalICICI(GetICICIResponseDetails zpmodel);
        GetICICIResponseDetails GetQuotationICICIDetail(string planId, string TId, string POLICY_TERM, string Frequency, string State);
        long SaveTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns);

        long UpdateTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns);
        //  ZPModel GetQuotationDetail(string planId, string TId);
        string GetQuotationDetailIndiaFirst(GetIndiaFirstResponceDetail DataChangeEvt);
        ZPModel GetQuotationDetail(string planId, string TId, string POLICY_TERM, string Frequency, string State);
        ZPModel GetQuotationDetail(ZPModel zpmodel);
    }
    public class TermLifeBusiness : ITermLifeBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private string baseUrl; string secretKey = string.Empty;
        string apiKey = string.Empty;
        private string KotakUrl;
        private RequestHandler requestHandler = new RequestHandler();
        public TermLifeBusiness()
        {

            db = new CheckyourpremiumliveEntities();
            baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseURL"].ToString();
            KotakUrl = System.Configuration.ConfigurationSettings.AppSettings["KotakUrl"].ToString();
            //secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            //apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        }
        //ppppppppppppppppp
        //Bind ICICI Data
        public List<customSelectList> Occupation()
        {
            try
            {

                var gender = db.ICICI_Occupation_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Occupation.ToString(), text = x.value.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> Organaisation_Name()
        {
            try
            {

                var gender = db.ICICI_Organisation_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Company_Name.ToString(), text = x.Value.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetCountry()
        {
            try
            {

                var gender = db.ICICI_Country_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Value.ToString(), text = x.Country_Name.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetICICIState()
        {
            try
            {

                var gender = db.ICICI_State_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Value.ToString(), text = x.State_Name.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetNationality()
        {
            try
            {

                var gender = db.ICICI__NATIONALITY__tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.value.ToString(), text = x.NATIONALITY.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetICICIMaritalStatus()
        {
            try
            {

                var gender = db.ICICI_MARITAL_STATUS_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.MARITAL_STATUS.ToString(), text = x.Value.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetEducation()
        {
            try
            {

                var gender = db.ICICI_Education_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Education.ToString(), text = x.Value.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetTrusteeType()
        {
            try
            {

                var gender = db.ICICI_Trustee_Type_tbl.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Value.ToString(), text = x.Trustee_Type.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        //Bind ICICI Data
        public List<customSelectList> MarialStatus()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var state = db.tbl_MaterialStatus.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Material.ToString(), text = x.Material.ToString() }).Distinct().ToList();
                return state;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetSuminsured()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var suminsured = db.tbl_EdelWise_Suminsured.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Sumisurredvalue.ToString(), text = x.Suminsured.ToString() }).Distinct().ToList();
                return suminsured;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> PremiumType()
        {
            try
            {
                var suminsured = db.tbl_PremiumType.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Premium_Type.ToString(), text = x.Premium_Type.ToString() }).Distinct().ToList();
                return suminsured;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> BindState()
        {
            try
            {
                var state = db.tbl_IndiaFirstCityData.Select(x => x.State)
                                             .Distinct()
                                             .Select(x =>
                                             new customSelectList
                                             {
                                                 text = x,
                                                 value = x
                                             }).OrderBy(x => x.text).ToList();
                return state;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> GetPolicyTerm()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var policyterm = db.tbl_FrequncyType.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.FrequncyType.ToString(), text = x.FrequncyValue.ToString() }).Distinct().ToList();
                return policyterm;
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
        public List<customSelectList> GetFrequency()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var policyterm = db.tbl_CoverFrequency.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.CoverFrequency.ToString(), text = x.CoverFrequency.ToString() }).Distinct().ToList();
                return policyterm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        //Poonam ..............................
        public List<customSelectList> BindRiskOptions()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var riskoption = db.RiskCover_options.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Options.ToString(), text = x.RiskCoverOptions.ToString() }).Distinct().ToList();
                return riskoption;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public List<customSelectList> BindcoverOptions()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var riskoption = db.tbl_ProductPlan_Data.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.PlanName.ToString(), text = x.PlanName.ToString() }).Distinct().ToList();
                return riskoption;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        //End..........code.......
        public string GetQuotationDetailIndiaFirst(GetIndiaFirstResponceDetail srchTrvlIns)
        {

            int ageperson = Convert.ToInt32(srchTrvlIns.Age);
            int Policyterm = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
            int totaldata = ageperson + Policyterm;
            try
            {
                if (srchTrvlIns.Income == "Upto 3 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "230000";
                }
                else if (srchTrvlIns.Income == "3-5 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "300000";
                }
                else if (srchTrvlIns.Income == "5-7 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "500000";
                }
                else if (srchTrvlIns.Income == "7-10 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "700000";
                }
                else if (srchTrvlIns.Income == "10-15 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "1000000";
                }
                else if (srchTrvlIns.Income == "15 Lacs+ Per Annum")
                {
                    srchTrvlIns.Income = "1500000";
                }
                int age = Convert.ToInt32(srchTrvlIns.Age);
                srchTrvlIns.CustID = srchTrvlIns.CustID != null ? srchTrvlIns.CustID : "1";
                if (srchTrvlIns.CustID == "1         ")
                {
                    srchTrvlIns.CustID = "1";
                }
                else if (srchTrvlIns.CustID == "3         ") { srchTrvlIns.CustID = "3"; }
                else if (srchTrvlIns.CustID == "4         ") { srchTrvlIns.CustID = "4"; }
                else if (srchTrvlIns.CustID == "6         ") { srchTrvlIns.CustID = "6"; }
                else if (srchTrvlIns.CustID == "7         ") { srchTrvlIns.CustID = "7"; }
                else if (srchTrvlIns.CustID == "8         ") { srchTrvlIns.CustID = "8"; }
                else if (srchTrvlIns.CustID == "5         ") { srchTrvlIns.CustID = "5"; }
                var result = SoapRequestHandler.SubmitSoapRequest(srchTrvlIns.Age, srchTrvlIns.Gender, srchTrvlIns.Smoke, srchTrvlIns.fullName, srchTrvlIns.DOB, srchTrvlIns.Email, srchTrvlIns.Phone, srchTrvlIns.sumAssured, srchTrvlIns.POLICY_TERM, srchTrvlIns.Frequency, srchTrvlIns.city, srchTrvlIns.PemiumType, srchTrvlIns.CustID, srchTrvlIns.Income);
                int term = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
                XDocument docc = XDocument.Parse(result);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(result);
                XmlNode xn = xd.DocumentElement;
                int sumasurred = Convert.ToInt32(srchTrvlIns.sumAssured);
                if (sumasurred < 5000000)
                {
                    int i = Convert.ToInt32(srchTrvlIns.CustID);

                    XmlNode rest = xn.SelectSingleNode("//*[@id='annual_prem']"); //you should give exact condition
                    XmlNode res2 = xn.SelectSingleNode("//*[@id='res4']"); //you should give exact condition
                    XmlNode res3 = xn.SelectSingleNode("//*[@id='annual_prem']"); //you should give exact condition
                    XmlNode res4 = xn.SelectSingleNode("//*[@id='sum_assured']");
                    XmlNode res5 = xn.SelectSingleNode("//*[@id='prem_pay_term']");
                    GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                    if (res3.InnerText != "0.0")
                    {
                        if (res3.InnerText != "")
                        {
                            Responcedetail.ANNUAL_GROSS_PREMIUM = rest.InnerText;
                            Responcedetail.INSTALL_PREMIUM_BEFORETAX =
                                //decimal Perimum = Convert.ToDecimal(result3.InnerText) / term;
                                //decimal Perimum3 = Convert.ToDecimal(res3.InnerText);
                                //int pre = Convert.ToInt32(Perimum3);
                            Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = res3.InnerText;
                            Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE1 = res3.InnerText;
                            Responcedetail.Logo = "/Logo/IndiaFirstLogo.png";
                            //.....Responce
                            decimal sum = Convert.ToDecimal(res4.InnerText);
                            int sum1 = Convert.ToInt32(sum);
                            Responcedetail.SUM_ASSURED = sum1.ToString();
                            Responcedetail.Company = "10007";
                            Responcedetail.PlanName = "Anytime plan";
                            Responcedetail.LogoName = "Anytime plan";
                            Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);
                            List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
                            lst.Add(Responcedetail);
                            return srchTrvlIns.ToString();
                        }

                    }
                }
                else
                {
                    // int i = Convert.ToInt32(srchTrvlIns.CustID);
                    if (totaldata > 60 && srchTrvlIns.CustID == "4")
                    {
                        return string.Empty;
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "5")
                    {
                        return string.Empty;
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "6")
                    {
                        return string.Empty;
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "7")
                    {
                        return string.Empty;
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "8")
                    {
                        return string.Empty;
                    }
                    XmlNode resultt = xn.SelectSingleNode("//*[@id='RISK" + srchTrvlIns.CustID + "_ANNUAL_GROSS_PREMIUM']"); //you should give exact condition
                    XmlNode result2 = xn.SelectSingleNode("//*[@id='RISK" + srchTrvlIns.CustID + "_INSTALL_PREMIUM_BEFORETAX']"); //you should give exact condition
                    XmlNode result3 = xn.SelectSingleNode("//*[@id='RISK" + srchTrvlIns.CustID + "_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    XmlNode result4 = xn.SelectSingleNode("//*[@id='SUM_ASSURED']");
                    XmlNode result5 = xn.SelectSingleNode("//*[@id='POLICY_TERM']");
                    GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                    if (result3.InnerText != "0.0")
                    {
                        if (result3.InnerText != "")
                        {
                            Responcedetail.ANNUAL_GROSS_PREMIUM = resultt.InnerText;
                            Responcedetail.INSTALL_PREMIUM_BEFORETAX = result2.InnerText;
                            //decimal Perimum = Convert.ToDecimal(result3.InnerText) / term;
                            decimal Perimum = Convert.ToDecimal(result3.InnerText);
                            int pre = Convert.ToInt32(Perimum);
                            srchTrvlIns.totalPremium = pre.ToString();
                            Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE1 = result3.InnerText;
                            Responcedetail.Logo = "/Logo/IndiaFirstLogo.png";           //.....Responce
                            decimal sum = Convert.ToDecimal(result4.InnerText);
                            int sum1 = Convert.ToInt32(sum);
                            Responcedetail.SUM_ASSURED = sum1.ToString();
                            Responcedetail.Company = "10007";
                            Responcedetail.PlanName = "Life Benefit";
                            Responcedetail.LogoName = "Online E-term";
                            Responcedetail.CustID = srchTrvlIns.SearchId.ToString();
                            Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);

                            return srchTrvlIns.ToString();
                        }
                    }

                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return string.Empty;

        }
        public List<GetIndiaFirstResponceDetail> GetPremiumList(GetIndiaFirstResponceDetail srchTrvlIns)
        {
            try
            {
                if (srchTrvlIns.Income == "Upto 3 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "230000";
                }
                else if (srchTrvlIns.Income == "3-5 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "300000";
                }
                else if (srchTrvlIns.Income == "5-7 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "500000";
                }
                else if (srchTrvlIns.Income == "7-10 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "700000";
                }
                else if (srchTrvlIns.Income == "10-15 Lacs Per Annum")
                {
                    srchTrvlIns.Income = "1000000";
                }
                else if (srchTrvlIns.Income == "15 Lacs+ Per Annum")
                {
                    srchTrvlIns.Income = "1500000";
                }
                int age = Convert.ToInt32(srchTrvlIns.Age);
                srchTrvlIns.CustID = srchTrvlIns.CustID != null ? srchTrvlIns.CustID : "1";
                if (srchTrvlIns.CustID == "1")
                {
                    srchTrvlIns.CustID = "1";
                }
                else if (srchTrvlIns.CustID == "3         ") { srchTrvlIns.CustID = "3"; }
                else if (srchTrvlIns.CustID == "4         ") { srchTrvlIns.CustID = "4"; }
                else if (srchTrvlIns.CustID == "6         ") { srchTrvlIns.CustID = "6"; }
                else if (srchTrvlIns.CustID == "7         ") { srchTrvlIns.CustID = "7"; }
                else if (srchTrvlIns.CustID == "8         ") { srchTrvlIns.CustID = "8"; }
                else if (srchTrvlIns.CustID == "5         ") { srchTrvlIns.CustID = "5"; }
                var result = SoapRequestHandler.SubmitSoapRequest(srchTrvlIns.Age, srchTrvlIns.Gender, srchTrvlIns.Smoke, srchTrvlIns.fullName, srchTrvlIns.DOB, srchTrvlIns.Email, srchTrvlIns.Phone, srchTrvlIns.sumAssured, srchTrvlIns.POLICY_TERM, srchTrvlIns.Frequency, srchTrvlIns.city, srchTrvlIns.PemiumType, srchTrvlIns.CustID, srchTrvlIns.Income);
                int term = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
                XDocument docc = XDocument.Parse(result);
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(result);
                XmlNode xn = xd.DocumentElement;
                int sumasurred = Convert.ToInt32(srchTrvlIns.sumAssured);
                if (sumasurred < 5000000)
                {
                    if (age > 60)
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }

                    int i = Convert.ToInt32(srchTrvlIns.CustID);

                    XmlNode rest = xn.SelectSingleNode("//*[@id='annual_prem']"); //you should give exact condition
                    XmlNode res2 = xn.SelectSingleNode("//*[@id='res4']"); //you should give exact condition
                    XmlNode res3 = xn.SelectSingleNode("//*[@id='instal_prem_with_ST']"); //you should give exact condition
                    XmlNode res4 = xn.SelectSingleNode("//*[@id='sum_assured']");
                    XmlNode res5 = xn.SelectSingleNode("//*[@id='prem_pay_term']");
                    GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                    if (res3.InnerText != "0.0")
                    {
                        if (res3.InnerText != "")
                        {
                            Responcedetail.ANNUAL_GROSS_PREMIUM = rest.InnerText;
                            Responcedetail.INSTALL_PREMIUM_BEFORETAX =
                                //decimal Perimum = Convert.ToDecimal(result3.InnerText) / term;
                                //decimal Perimum3 = Convert.ToDecimal(res3.InnerText);
                                //int pre = Convert.ToInt32(Perimum3);
                            Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = res3.InnerText;
                            Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE1 = res3.InnerText;
                            Responcedetail.Logo = "/Logo/IndiaFirstLogo.png";
                            //.....Responce
                            decimal sum = Convert.ToDecimal(res4.InnerText);
                            int sum1 = Convert.ToInt32(sum);
                            Responcedetail.SUM_ASSURED = sum1.ToString();
                            Responcedetail.Company = "10007";
                            Responcedetail.PlanName = "Anytime plan";
                            Responcedetail.LogoName = "Anytime plan";
                            Responcedetail.pdfName = srchTrvlIns.PemiumType;
                            Responcedetail.CustID = srchTrvlIns.CustID;
                            Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);
                            List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
                            lst.Add(Responcedetail);
                            return lst;
                        }

                    }
                }
                else
                {
                    //////////////////////
                    //XmlNode resdata1 = xn.SelectSingleNode("//*[@id='RISK1_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata2 = xn.SelectSingleNode("//*[@id='RISK3_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata3 = xn.SelectSingleNode("//*[@id='RISK4_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata4 = xn.SelectSingleNode("//*[@id='RISK5_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata5 = xn.SelectSingleNode("//*[@id='RISK6INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata6 = xn.SelectSingleNode("//*[@id='RISK7_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //XmlNode resdata7 = xn.SelectSingleNode("//*[@id='RISK8_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                    //if(resdata1.InnerText!="0.0")
                    //{ }
                    //else if (resdata1.InnerText != "0.0")
                    //{ }
                    //else if ( resdata2.InnerText != "0.0")
                    //{ }
                    //else if (resdata3.InnerText != "0.0")
                    //{ }
                    //else if (resdata4.InnerText != "0.0")
                    //{ }
                    //else if (resdata5.InnerText != "0.0")
                    //{ }
                    //else if (resdata6.InnerText != "0.0")
                    //{ }
                    //else if (resdata7.InnerText != "0.0")
                    //{ }
                    //XmlNode resultt = xn.SelectSingleNode("");
                    //XmlNode result2 = xn.SelectSingleNode("");
                    //XmlNode result3 = xn.SelectSingleNode("");
                    //XmlNode result4 = xn.SelectSingleNode("");
                    //XmlNode result5 = xn.SelectSingleNode("");
                    int ageperson = Convert.ToInt32(srchTrvlIns.Age);
                    int Policyterm = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
                    int totaldata = ageperson + Policyterm;
                    int i = Convert.ToInt32(srchTrvlIns.CustID);

                    GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                    if (totaldata > 80 && srchTrvlIns.CustID == "1")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    if (totaldata > 60 && srchTrvlIns.CustID == "4")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    if (age < 20 && srchTrvlIns.CustID == "4")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "5")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "6")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "7")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    else if (totaldata > 65 && srchTrvlIns.CustID == "8")
                    {
                        return new List<GetIndiaFirstResponceDetail>();
                    }
                    for (i = 1; i <= 8; i++)
                    {
                        //    //////////////////////
                        if (i != 2)
                        {
                            XmlNode resultt = xn.SelectSingleNode("//*[@id='RISK" + i + "_ANNUAL_GROSS_PREMIUM']"); //you should give exact condition
                            XmlNode result2 = xn.SelectSingleNode("//*[@id='RISK" + i + "_INSTALL_PREMIUM_BEFORETAX']"); //you should give exact condition
                            XmlNode result3 = xn.SelectSingleNode("//*[@id='RISK" + i + "_INSTALL_PREMIUM_TAXINCLUSIVE']"); //you should give exact condition
                            XmlNode result4 = xn.SelectSingleNode("//*[@id='SUM_ASSURED']");
                            XmlNode result5 = xn.SelectSingleNode("//*[@id='POLICY_TERM']");



                            if (result3.InnerText != "0.0")
                            {
                                if (result3.InnerText != "")
                                {
                                    if (i == 1) { Responcedetail.PlanName = "Life Benefit"; }
                                    if (i == 3) { Responcedetail.PlanName = "Income Benefit"; }
                                    if (i == 4) { Responcedetail.PlanName = "Income Plus Benefit"; }
                                    if (i == 5) { Responcedetail.PlanName = "Income Replacement Benefit"; }
                                    if (i == 6) { Responcedetail.PlanName = "Disability Shield Benefit"; }
                                    if (i == 7) { Responcedetail.PlanName = "Critical Illness Protector Benefit"; }
                                    if (i == 8) { Responcedetail.PlanName = "Comprehensive Benefit"; }
                                    Responcedetail.ANNUAL_GROSS_PREMIUM = resultt.InnerText;
                                    Responcedetail.INSTALL_PREMIUM_BEFORETAX = result2.InnerText;
                                    //decimal Perimum = Convert.ToDecimal(result3.InnerText) / term;
                                    decimal Perimum = Convert.ToDecimal(result3.InnerText);
                                    int pre = Convert.ToInt32(Perimum);
                                    Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = pre.ToString();
                                    Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE1 = result3.InnerText;
                                    Responcedetail.Logo = "/Logo/IndiaFirstLogo.png";
                                    //.....Responce
                                    decimal sum = Convert.ToDecimal(result4.InnerText);
                                    int sum1 = Convert.ToInt32(sum);
                                    Responcedetail.SUM_ASSURED = sum1.ToString();
                                    Responcedetail.Company = "10007";
                                    Responcedetail.PlanName = Responcedetail.PlanName;
                                    Responcedetail.LogoName = "Online E-term";
                                    Responcedetail.pdfName = srchTrvlIns.PemiumType;
                                    Responcedetail.CustID = srchTrvlIns.CustID;
                                    Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);
                                    List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
                                    lst.Add(Responcedetail);
                                    return lst;
                                }
                            }
                        }
                    }

                }
                return new List<GetIndiaFirstResponceDetail>();

            }
            catch (Exception ex)
            {
                return new List<GetIndiaFirstResponceDetail>();
            }
            return new List<GetIndiaFirstResponceDetail>();
        }
        public List<GetIndiaFirstResponceDetail> GetPremiumListHDFC(GetIndiaFirstResponceDetail srchTrvlIns)
        {

            try
            {
                //................HDFC......................
                var HDFCResult = SoapRequestHandler.SubmitSoapRequestForHDFC(srchTrvlIns.premiumWaiver, srchTrvlIns.Age, srchTrvlIns.Gender, srchTrvlIns.Smoke, srchTrvlIns.fullName, srchTrvlIns.DOB, srchTrvlIns.Email, srchTrvlIns.Phone, srchTrvlIns.sumAssured, srchTrvlIns.POLICY_TERM, srchTrvlIns.Frequency, srchTrvlIns.city);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(HDFCResult);
                var responseString = doc.InnerText;

                var xDoc = XDocument.Parse(HDFCResult);
                var msg = xDoc.Descendants("status").Single();
                string message = msg.Value;
                if (message == "Failed")
                {
                    var m = xDoc.Descendants("msg").Single();
                    srchTrvlIns.error = m.Value;
                    List<GetIndiaFirstResponceDetail> lst1 = new List<GetIndiaFirstResponceDetail>();
                    lst1.Add(srchTrvlIns);
                    return lst1;

                }
                else if (message == "Success")
                {
                    var responc = (responseString).Replace("Successpindianbank1C2P3D_Life_Option", ""); //..........................
                    int index = responc.LastIndexOf("FULL");
                    if (index > 0)
                        responc = responc.Substring(0, index);
                    int length = responc.Length;


                    var result = JsonConvert.DeserializeObject<RootObject2>(responc);
                    if (responseString != "")
                    {
                        //...................

                        var status = xDoc.Descendants("appno").Single();
                        srchTrvlIns.CustID = status.Value;
                        // with Descendents

                        // var resultHDFC = JsonConvert.DeserializeObject<RootObject2>(responseString);
                        srchTrvlIns.INSTALL_PREMIUM_TAXINCLUSIVE = result.totAnnPremium.ToString();
                        srchTrvlIns.SUM_ASSURED = srchTrvlIns.sumAssured;
                        srchTrvlIns.Company = "10010";
                        srchTrvlIns.PlanName = "HDFC";
                        srchTrvlIns.Logo = "/Logo/HDFC.png";
                        //ppppppppppppppp
                        srchTrvlIns.accidentalDeathPremium = "";
                        srchTrvlIns.basePremium = "";
                        srchTrvlIns.betterHalfPremium = "";
                        srchTrvlIns.criticalIllnessPremium = "";
                        srchTrvlIns.error = "";
                        srchTrvlIns.hcbPremium = "";
                        srchTrvlIns.pdfName = "";
                        srchTrvlIns.permanentDisabilityPremium = "";
                        srchTrvlIns.premium = "";
                        srchTrvlIns.premiumWaiver = "";
                        srchTrvlIns.totalPremium = result.totAnnPremium.ToString();
                        srchTrvlIns.CustID = srchTrvlIns.CustID;
                        srchTrvlIns.SearchId = SaveResponceIndiaFirstTermiLife(srchTrvlIns);

                        //ppppppppppppp
                        List<GetIndiaFirstResponceDetail> lst1 = new List<GetIndiaFirstResponceDetail>();
                        lst1.Add(srchTrvlIns);
                        return lst1;
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<GetIndiaFirstResponceDetail>();
            }
            return new List<GetIndiaFirstResponceDetail>();
        }
        public List<GetIndiaFirstResponceDetail> GetPremiumEdelWises(GetIndiaFirstResponceDetail srchTremIns)
        {
            if (srchTremIns.Smoke == "true")
            {
                srchTremIns.Smoke = "Yes";
            }
            else
            {
                srchTremIns.Smoke = "No";
            }
            var customer = "{ 'product': 'Zindagiplus', 'fullName':  '" + srchTremIns.fullName + "', 'cliGender': '" + srchTremIns.Gender + "', 'age':'" + srchTremIns.Age + "', 'cliDOB': '" + srchTremIns.DOB + "', 'maturityAge': '0', 'frequency': '" + srchTremIns.Frequency + "', 'smoke': '" + srchTremIns.Smoke + "', 'policyTerm': '" + srchTremIns.POLICY_TERM + "', 'premiumPayingTerm': '" + srchTremIns.POLICY_TERM + "', 'sumAssured': '" + srchTremIns.sumAssured + "', 'staff': 'No', 'workSiteFlag': 'No', 'index': '0', 'ADB': '100000', 'ATPD': '100000', 'CI': '100000', 'HCB': '100000', 'Term': '', 'WOP': 'No', 'pdf': 'No', 'childAge': '', 'childGender': '', 'maturityOption': '', 'flexibleBenefitYear': '', 'postponement': '', 'largeCap': '', 'top250': '', 'bond': '', 'moneyMarket': '', 'PEBased': '', 'managed': '', 'claimsOption': '', 'LAProposerSame': 'false',   'LAFullName': '" + srchTremIns.fullName + "', 'LAEmail': '" + srchTremIns.Email + "','LAMarriedStatus':'single', 'LANumber': '" + srchTremIns.Phone + "', 'LATobacco': '" + srchTremIns.Smoke + "', 'LAAge': '" + srchTremIns.Age + "', 'LADOB': '" + srchTremIns.DOB + "', 'investmentStrategy': '', 'LAGender': '" + srchTremIns.Gender + "', 'risingStar': 'No', 'policyOption': 'Life Cover with Level Sum Assured', 'distributionChannel': 'Corporate Agent', 'betterHalfBenefit': 'No', 'spouseFirstName': '', 'spouseLastName': ' ', 'spouseDOB': '', 'spouseGender': '', 'spouseAge': '', 'spouseTobaccoUser': '', 'additionalBenefit': '', 'topUpBenefitPercentage': '', 'payoutOption': 'LumpSum', 'payoutMonths': '', 'payoutPercentageLumpsum': '100', 'payoutPercentageLevelIncome': '0', 'payoutPercentageIncreasingIncome': '0', 'tranId': '' }";
            var response = requestHandler.Response(baseUrl, customer);
            List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
            try
            {
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    var result = JsonConvert.DeserializeObject<EdleWise>(responseString);
                    srchTremIns.INSTALL_PREMIUM_TAXINCLUSIVE = result.premium;
                    srchTremIns.SUM_ASSURED = srchTremIns.sumAssured;
                    srchTremIns.Company = "1";
                    srchTremIns.PlanName = "Zindgi Plus";
                    srchTremIns.Logo = "/Logo/logoEdelWise.gif";
                    srchTremIns.accidentalDeathPremium = result.accidentalDeathPremium;
                    srchTremIns.basePremium = result.basePremium;
                    srchTremIns.betterHalfPremium = result.betterHalfPremium;
                    srchTremIns.criticalIllnessPremium = result.criticalIllnessPremium;
                    srchTremIns.error = result.error;
                    srchTremIns.hcbPremium = result.hcbPremium;
                    srchTremIns.pdfName = result.pdfName;
                    srchTremIns.permanentDisabilityPremium = result.permanentDisabilityPremium;
                    srchTremIns.premium = result.premium;
                    srchTremIns.premiumWaiver = result.premiumWaiver;
                    srchTremIns.totalPremium = result.totalPremium;
                    srchTremIns.CustID = srchTremIns.CustID;
                    srchTremIns.SearchId = SaveResponceIndiaFirstTermiLife(srchTremIns);
                    lst.Add(srchTremIns);
                    //Save Detail..........
                    //var tDetails = new tbl_GetPremiumResponce
                    //{



                    //};
                    //db.tbl_GetPremiumResponce.Add(tDetails);
                    //db.SaveChanges();
                    //var tid = tDetails.ID;

                    //.....................
                }

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lst;
        }

        public long SaveResponceIndiaFirstTermiLife(GetIndiaFirstResponceDetail Responcedetail)
        {
            try
            {
                string id = RandomNum();
                var tDetails = new tbl_GetPremiumResponce
                {

                    accidentalDeathPremium = Responcedetail.accidentalDeathPremium,
                    basePremium = Responcedetail.basePremium,
                    betterHalfPremium = Responcedetail.betterHalfPremium,
                    criticalIllnessPremium = Responcedetail.criticalIllnessPremium,
                    error = Responcedetail.error,
                    hcbPremium = Responcedetail.hcbPremium,
                    pdfName = Responcedetail.pdfName,
                    permanentDisabilityPremium = Responcedetail.permanentDisabilityPremium,
                    premium = Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE,
                    premiumWaiver = Responcedetail.premiumWaiver,
                    sumAssured = Responcedetail.SUM_ASSURED,
                    totalPremium = Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE1,
                    Company = Convert.ToInt32(Responcedetail.Company),
                    PlanName = Responcedetail.PlanName,
                    // CustID = Convert.ToInt32(Responcedetail.CustID),
                    CustID = Convert.ToInt32(id),
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
        public string RandomNum()
        {
            string quoteNo = string.Empty;
            try
            {

                quoteNo = (new Random()).Next(1000000000).ToString();
                var result = db.tbl_GetPremiumData.SingleOrDefault(x => x.CustID == quoteNo);
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

        public long SaveTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns)
        {
            try
            {
                string id = RandomNum();
                int term = 80 - Convert.ToInt32(srchTrvlIns.Age);
                srchTrvlIns.sumAssured = "10000000";
                if (term > 40)
                {
                    srchTrvlIns.POLICY_TERM = "40";
                }
                else
                {
                    srchTrvlIns.POLICY_TERM = term.ToString();
                }
                srchTrvlIns.Frequency = "Yearly";
                var tDetails = new tbl_GetPremiumData
                {

                    Age = srchTrvlIns.Age,
                    Name = srchTrvlIns.fullName,
                    Income = srchTrvlIns.Income,
                    Mobile = srchTrvlIns.Phone,
                    DOB = DateTime.ParseExact(srchTrvlIns.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    //   DOB = srchTrvlIns.DOB.ToString(),
                    Email = srchTrvlIns.Email,
                    Smoke = srchTrvlIns.Smoke,
                    Gender = srchTrvlIns.Gender,
                    SumInsurred = Convert.ToDecimal(srchTrvlIns.sumAssured),
                    Term = Convert.ToInt32(srchTrvlIns.POLICY_TERM),
                    frequency = srchTrvlIns.Frequency,
                    createDate = DateTime.Now,
                    CustID = id

                };
                db.tbl_GetPremiumData.Add(tDetails);
                db.SaveChanges();
                var tId = tDetails.ID;

                return tId;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;

        }

        public ZPModel GetQuotationDetail(string planId, string TId, string POLICY_TERM, string Frequency, string State)
        {
            ZPModel quotationDetail = new ZPModel();

            try
            {

                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                var view_Insurance = db.View_TermLifeInsurance.AsEnumerable().Where(x =>
                    x.ID == _planId).FirstOrDefault();
                var travelInsuranceSearchBies = db.tbl_GetPremiumData.AsEnumerable().Where(x => x.ID == _TId).FirstOrDefault();

                switch (view_Insurance.CompanyID)
                {

                    case 1:
                        if (travelInsuranceSearchBies.Smoke == "true")
                        {
                            travelInsuranceSearchBies.Smoke = "Yes";
                        }
                        else
                        {
                            travelInsuranceSearchBies.Smoke = "No";
                        }
                        var ResponceData1 = @"[
  {
    'ZPModel': {
      'FullName': '" + travelInsuranceSearchBies.Name + @"',
      'Gender': '" + travelInsuranceSearchBies.Gender + @"',
      'Age': " + travelInsuranceSearchBies.Age + @",
      'DOB': '" + Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + @"',
      'Smoke': '" + travelInsuranceSearchBies.Smoke + @"',
      'Earning': '" + travelInsuranceSearchBies.Income + @"',
      'MaritialStatus': 'Single',
      'Phone': '" + travelInsuranceSearchBies.Mobile + @"',
      'Email': '" + travelInsuranceSearchBies.Email + @"',
      'SumAssured': '" + view_Insurance.sumAssured + @"',
      'PolicyTerm': " + POLICY_TERM + @",
      'PPT': " + POLICY_TERM + @",
      'Frequency': '" + Frequency + @"',
      'BHB_Ind': 'Yes',
      'TUB_Ind': 'No',
      'PWB_Ind': 'No',
      'ADB_Ind': 'No',
      'CI_Ind': 'Yes',
      'PD_Ind': 'No',
      'HCB_ind': 'Yes',
      'DSA_ind': 'No',
      'ADB': 100000,
      'ATPD': 100000,
      'CI': 1200000,
      'HCB': 250000,
      'CIC_Ind': 'Yes',
      'CIC_SumAssured': 1200000,
      'CIC_ClaimOption': 'Multi',
      'CIC_PolicyTerm': 30,
      'TopupRate': 10,
      'TotalBenefit': 2,
      'PolicyOption': 'Life Cover with Level Sum Assured',
      'SpouseFirstName': '',
      'SpouseLastName': '',
      'SpouseDOB': '',
      'SpouseGender': '',
      'SpouseAge':0,
      'SpouseTobbacoUser': '',
      'AdditionalBenefit': 'Top-up Benefit',
      'TopUpBenefitPercentage': '10%',
      'PayoutOption': 'Lumpsum',
      'PayoutMonths': '',
      'PayoutPercentageLumpsum': '100%',
      'PayoutPercentageLevelIncome': '',
      'PayoutPercentageIncreasingIncome': '',
      'TransID': ''
    }
  }
]
            ";
                        EdelWisesRequest edel = new EdelWisesRequest();
                        quotationDetail = new ZPModel
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            BHB_Ind = "Yes",
                            TUB_Ind = "No",
                            PWB_Ind = "No",
                            ADB_Ind = "No",
                            PD_Ind = "No",
                            HCB_ind = "Yes",
                            DSA_ind = "No",
                            ADB = 100000,
                            ATPD = 100000,
                            CI = 1200000,
                            HCB = 250000,
                            CIC_Ind = "Yes",
                            CIC_SumAssured = 1200000,
                            CIC_ClaimOption = "Multi",
                            CIC_PolicyTerm = 30,
                            TopupRate = 10,
                            TotalBenefit = 2,
                            PolicyOption = "Life Cover with Level Sum Assured",
                            SpouseFirstName = "",
                            SpouseLastName = "",
                            SpouseDOB = "",
                            SpouseGender = "",
                            SpouseAge = quotationDetail.SpouseAge,
                            SpouseTobbacoUser = "",
                            AdditionalBenefit = "Top-up Benefit",
                            TopUpBenefitPercentage = "10%",
                            PayoutOption = "LUMSUM",
                            PayoutMonths = "",
                            PayoutPercentageLumpsum = "100%",
                            PayoutPercentageLevelIncome = "",
                            PayoutPercentageIncreasingIncome = "",
                            TransID = ""

                        };


                        string postdata = JsonConvert.SerializeObject(quotationDetail);

                        quotationDetail.hdnZindagiPlusdata = ResponceData1.Replace("'", "\"");

                        break;
                    case 10007:
                        quotationDetail = new ZPModel
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            //  PayoutOption = "https://uat.indiafirstlife.com/buy-online-insurance/term-plan/?src=CYP",
                            PayoutOption = "https://indiafirstlife.com/buy-online-insurance/term-plan/?src=CYP",
                            Company = view_Insurance.CompanyID.ToString(),
                            pdfName = view_Insurance.pdfName,
                            State = quotationDetail.State
                        };
                        break;
                    case 10010:
                        quotationDetail = new ZPModel
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            CustID = view_Insurance.CustID.ToString(),
                            PayoutOption = " https://onlineinsuranceuat.hdfclife.com/thirdparty?appnum='" + view_Insurance.CustID + "'&mob='" + travelInsuranceSearchBies.Mobile + "'&dob='" + Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + "'&email='" + travelInsuranceSearchBies.Email + "'&mobcd=91",
                            Company = view_Insurance.CompanyID.ToString()
                        };
                        break;
                    case 10014:

                        quotationDetail = new ZPModel
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            Company = view_Insurance.CompanyID.ToString(),
                            pdfName = view_Insurance.pdfName,
                            State = quotationDetail.State
                        };

                        break;
                    default:
                        break;

                }

                return quotationDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }


        public ZPModel GetQuotationDetail(ZPModel zpmodel)
        {
            ZPModel quotationDetail = new ZPModel();

            try
            {
                if (zpmodel.Company != "10007")
                {
                    if (zpmodel.Smoke == "true") { zpmodel.Smoke = "Yes"; }
                    else { zpmodel.Smoke = "No"; }
                    if (zpmodel.SpouseAge == ",") { zpmodel.SpouseAge = "0"; } else { zpmodel.SpouseAge = zpmodel.SpouseAge; }
                    var ResponceData1 = @"[
  {
    'ZPModel': {
      'FullName': '" + zpmodel.FullName + @"',
      'Gender': '" + zpmodel.Gender + @"',
      'Age': " + zpmodel.Age + @",
      'DOB': '" + Convert.ToDateTime(zpmodel.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + @"',
      'Smoke': '" + zpmodel.Smoke + @"',
      'Earning': 'Above 20 Lakhs',
      'MaritialStatus':'" + zpmodel.MaritialStatus.Replace(",", "") + @"',
      'Phone': '" + zpmodel.Phone + @"',
      'Email': '" + zpmodel.Email + @"',
      'SumAssured': '" + zpmodel.sumAssured + @"',
      'PolicyTerm': " + zpmodel.PolicyTerm + @",
      'PPT': " + zpmodel.PolicyTerm + @",
      'Frequency': '" + zpmodel.Frequency + @"',
      'BHB_Ind': 'Yes',
      'TUB_Ind': 'No',
      'PWB_Ind': 'No',
      'ADB_Ind': 'No',
      'CI_Ind': 'No',
      'PD_Ind': 'No',
      'HCB_ind': 'Yes',
      'DSA_ind': 'No',
      'ADB': 100000,
      'ATPD': 100000,
      'CI': 1200000,
      'HCB': 250000,
      'CIC_Ind': 'No',
      'CIC_SumAssured': 1200000,
      'CIC_ClaimOption': 'Single',
      'CIC_PolicyTerm': 30,
      'TopupRate': 10,
      'TotalBenefit': 2,
      'PolicyOption': 'Life Cover with Level Sum Assured',
      'SpouseFirstName': '" + zpmodel.SpouseFirstName.Replace(",", "") + @"',
      'SpouseLastName': '" + zpmodel.SpouseLastName.Replace(",", "") + @"',
      'SpouseDOB': '" + zpmodel.SpouseDOB.Replace(",", "") + @"',
      'SpouseGender': '" + zpmodel.SpouseGender.Replace(",", "") + @"',
      'SpouseAge':" + zpmodel.SpouseAge.Replace(",", "") + @",
      'SpouseTobbacoUser': '" + zpmodel.SpouseTobbacoUser.Replace(",", "") + @"',
      'AdditionalBenefit': 'Top-up Benefit',
      'TopUpBenefitPercentage': '10%',
      'PayoutOption': 'Lumpsum',
      'PayoutMonths': '',
      'PayoutPercentageLumpsum': '100%',
      'PayoutPercentageLevelIncome': '',
      'PayoutPercentageIncreasingIncome': '',
      'TransID': ''
    }
  }
]
            ";
                    zpmodel.hdnZindagiPlusdata = ResponceData1;
                    zpmodel.hdnZindagiPlusdata = ResponceData1.Replace("'", "\"");
                    //  zpmodel.hdnZindagiPlusdata = ResponceData1.Replace(",,", "\",");


                    return zpmodel;
                }
                quotationDetail.Company = zpmodel.Company;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }

        public List<GetIndiaFirstResponceDetail> GetPremiumEdelWisesResponce(GetIndiaFirstResponceDetail srchTremIns)
        {

            if (srchTremIns.Smoke == "true")
            {
                srchTremIns.Smoke = "Yes";
            }
            else
            {
                srchTremIns.Smoke = "No";
            }
            var customer = "{ 'product': 'Zindagiplus', 'fullName':  '" + srchTremIns.fullName + "', 'cliGender': '" + srchTremIns.Gender + "', 'age':'" + srchTremIns.Age + "', 'cliDOB': '" + srchTremIns.DOB + "', 'maturityAge': '0', 'frequency': '" + srchTremIns.Frequency + "', 'smoke': '" + srchTremIns.Smoke + "', 'policyTerm': '36', 'premiumPayingTerm': '36', 'sumAssured': '10000000', 'staff': 'No', 'workSiteFlag': 'No', 'index': '0', 'ADB': '0', 'ATPD': '0', 'CI': '0', 'HCB': '0', 'Term': '', 'WOP': 'No', 'pdf': 'No', 'childAge': '', 'childGender': '', 'maturityOption': '', 'flexibleBenefitYear': '', 'postponement': '', 'largeCap': '', 'top250': '', 'bond': '', 'moneyMarket': '', 'PEBased': '', 'managed': '', 'claimsOption': '', 'LAProposerSame': 'false', 'LAFullName': '" + srchTremIns.fullName + "', 'LAEmail': '" + srchTremIns.Email + "', 'LANumber': '" + srchTremIns.Phone + "', 'LATobacco': '" + srchTremIns.Smoke + "', 'LAAge': '" + srchTremIns.Age + "', 'LADOB': '" + srchTremIns.DOB + "', 'investmentStrategy': '', 'LAGender': '" + srchTremIns.Gender + "', 'risingStar': 'No', 'policyOption': 'Life Cover with Level Sum Assured', 'distributionChannel': 'Corporate Agent', 'betterHalfBenefit': 'No', 'spouseFirstName': '', 'spouseLastName': ' ', 'spouseDOB': '', 'spouseGender': '', 'spouseAge': '', 'spouseTobaccoUser': 'No', 'additionalBenefit': '', 'topUpBenefitPercentage': '', 'payoutOption': 'LumpSum', 'payoutMonths': '', 'payoutPercentageLumpsum': '100', 'payoutPercentageLevelIncome': '0', 'payoutPercentageIncreasingIncome': '0', 'tranId': '' }";
            var response = requestHandler.Response(baseUrl, customer);
            List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
            try
            {
                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    var result = JsonConvert.DeserializeObject<EdleWise>(responseString);
                    srchTremIns.INSTALL_PREMIUM_TAXINCLUSIVE = result.premium;
                    srchTremIns.SUM_ASSURED = "10000000";
                    srchTremIns.Company = "1";
                    srchTremIns.PlanName = "Zindgi Plus";
                    srchTremIns.Logo = "/Logo/logoEdelWise.gif";
                    srchTremIns.accidentalDeathPremium = result.accidentalDeathPremium;
                    srchTremIns.basePremium = result.basePremium;
                    srchTremIns.betterHalfPremium = result.betterHalfPremium;
                    srchTremIns.criticalIllnessPremium = result.criticalIllnessPremium;
                    srchTremIns.error = result.error;
                    srchTremIns.hcbPremium = result.hcbPremium;
                    srchTremIns.pdfName = result.pdfName;
                    srchTremIns.permanentDisabilityPremium = result.permanentDisabilityPremium;
                    srchTremIns.premium = result.premium;
                    srchTremIns.premiumWaiver = result.premiumWaiver;
                    srchTremIns.totalPremium = result.totalPremium;
                    srchTremIns.CustID = "";



                    srchTremIns.SearchId = SaveResponceIndiaFirstTermiLife(srchTremIns);
                    lst.Add(srchTremIns);
                    //Save Detail..........
                    //var tDetails = new tbl_GetPremiumResponce
                    //{



                    //};
                    //db.tbl_GetPremiumResponce.Add(tDetails);
                    //db.SaveChanges();
                    //var tid = tDetails.ID;

                    //.....................
                }

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lst;
        }

        public List<destinationList> GetAllCity(string state)
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var Coverage = db.tbl_IndiaFirstCityData.AsQueryable().AsEnumerable().Where(x => x.State == state).Select(x => new { City = x.City.ToString() }).Distinct().ToList().OrderBy(x => x.City).Select(x => new destinationList { text = x.City, value = x.City }).ToList();

                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();

        }
        public List<AllCompanyDetails> GetAllCompanyDetails(AllCompanyDetails objuserlhealth)
        {
            try
            {


                var data = db.All_Company_Details.AsEnumerable().Select(
                     p => new AllCompanyDetails
                     //var data = db.All_Company_Details.Where(x => x.Company_Id == objuserlhealth.Company_Id)
                     //.Select(x => new AllCompanyDetails
                     {
                         Id = p.Id,
                         Benefit = p.Benefit,
                         Co_Pay = p.Co_Pay,
                         Company_Id = p.Company_Id,
                         Company_Plan = p.Company_Plan,
                         Day_Care_Treatment = p.Day_Care_Treatment,
                         Post_Hospitalization = p.Post_Hospitalization,
                         Pre_Hospitalization = p.Pre_Hospitalization,
                         Restoration_Benefit = p.Restoration_Benefit,
                         Room_Rent = p.Room_Rent,
                         Ambulance_Charges = p.Ambulance_Charges,
                         Status = p.Status,

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
        public List<Compare_Term> GetCompareDetails(Compare_Term objuserlhealth)
        {
            try
            {


                var data = db.Compare_TermLife.AsEnumerable().Select(
                 p => new Compare_Term

                 {

                     Company_Id = p.Company_Id,
                     Company = p.Company,
                     Plan_Name = p.Plan_Name,
                     Minimum_Entry_Age = p.Minimum_Entry_Age,
                     Maximum_Entry_Age = p.Maximum_Entry_Age,
                     Cover_Upto = p.Cover_Upto,
                     Premium_Payment_Option = p.Premium_Payment_Option,
                     Payment_Payment_Mode = p.Payment_Payment_Mode,
                     Minimum_Sum_Assured = p.Minimum_Sum_Assured,
                     Medical_Test_Required = p.Medical_Test_Required,
                   

                 }).ToList();

                return data;

            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<Compare_Term>();

        }
        //ICICI ppppppppppppppppppppppp
        public List<GetIndiaFirstResponceDetail> GetPremiumListICICI(GetIndiaFirstResponceDetail srchTrvlIns)
        {

            try
            {
                //................ICICI......................
                var ICICIResult = SoapRequestHandler.SubmitSoapRequestForICICI(srchTrvlIns.Age, srchTrvlIns.Gender, srchTrvlIns.Smoke, srchTrvlIns.fullName, srchTrvlIns.DOB, srchTrvlIns.Email, srchTrvlIns.Phone, srchTrvlIns.sumAssured, srchTrvlIns.POLICY_TERM, srchTrvlIns.Frequency, srchTrvlIns.city, srchTrvlIns.PemiumType, srchTrvlIns.CustID, srchTrvlIns.Income);

                //................................................
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(ICICIResult);
                XmlElement elem = (XmlElement)xd.DocumentElement.FirstChild;
                //  XmlElement directoryElement = xd.InnerXml;
                XmlNode xn = xd.DocumentElement;
                string valueee = elem.InnerXml;
                XDocument docc = XDocument.Parse(valueee);
                XNamespace ns = "http://tempuri.org/";
                List<XElement> responses1 = docc.Descendants(ns + "EBIResponse").ToList();
                List<XElement> responses2 = docc.Descendants(ns + "PremiumSummary").ToList();

                GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                for (int intc = 0; intc <= responses2.Count - 1; intc++)
                {

                    Responcedetail.basePremium = responses2[intc].Element(ns + "AnnualPremium").Value;
                    string PremiumPaymentTerm = responses2[intc].Element(ns + "PremiumPaymentTerm").Value.ToString();
                    string PremiumPaymentOption = responses2[intc].Element(ns + "PremiumPaymentOption").Value.ToString();
                    string DeathBenefitOption = responses2[intc].Element(ns + "DeathBenefitOption").Value.ToString();
                    Responcedetail.POLICY_TERM = responses2[intc].Element(ns + "Term").Value.ToString();
                    //Responcedetail.Suminsured = responses2[intc].Element(ns + "sumAssured").Value.ToString();
                    Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = responses2[intc].Element(ns + "TotalPremium").Value.ToString();


                    //..............................................

                    Responcedetail.Logo = "/Logo/ICICI.png";
                    //.....Responce
                    Responcedetail.SUM_ASSURED = srchTrvlIns.SUM_ASSURED;
                    Responcedetail.Company = "10011";
                    Responcedetail.PlanName = "ICICI";
                    Responcedetail.LogoName = "ICICI Logo";
                    Responcedetail.pdfName = Responcedetail.PemiumType;
                    Responcedetail.SUM_ASSURED = srchTrvlIns.sumAssured;
                    //ppppppppppppppp
                    Responcedetail.accidentalDeathPremium = "";
                    Responcedetail.betterHalfPremium = "";
                    Responcedetail.criticalIllnessPremium = "";
                    Responcedetail.error = "";
                    Responcedetail.hcbPremium = "";
                    Responcedetail.permanentDisabilityPremium = "";
                    Responcedetail.premium = "";
                    Responcedetail.premiumWaiver = "";
                    Responcedetail.CustID = srchTrvlIns.CustID;
                    Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);
                    List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
                    lst.Add(Responcedetail);
                    return lst;

                }

            }
            catch (Exception ex)
            {
                return new List<GetIndiaFirstResponceDetail>();
            }
            return new List<GetIndiaFirstResponceDetail>();


        }
        public GetICICIResponseDetails GetQuotationICICIDetail(string planId, string TId, string POLICY_TERM, string Frequency, string State)
        {
            GetICICIResponseDetails quotationDetail = new GetICICIResponseDetails();
            //Term_ProposalCreation termproposal = new Term_ProposalCreation();

            try
            {

                Int64 _planId = Convert.ToInt64(planId);
                Int64 _TId = Convert.ToInt64(TId);
                var view_Insurance = db.View_TermLifeInsurance.AsEnumerable().Where(x =>
                    x.ID == _planId).FirstOrDefault();
                var travelInsuranceSearchBies = db.tbl_GetPremiumData.AsEnumerable().Where(x => x.ID == _TId).FirstOrDefault();
                switch (view_Insurance.CompanyID)
                {

                    case 1:
                        if (travelInsuranceSearchBies.Smoke == "true")
                        {
                            travelInsuranceSearchBies.Smoke = "Yes";
                        }
                        else
                        {
                            travelInsuranceSearchBies.Smoke = "No";
                        }
                        var ResponceData1 = @"[
  {
    'ZPModel': {
      'FullName': '" + travelInsuranceSearchBies.Name + @"',
      'Gender': '" + travelInsuranceSearchBies.Gender + @"',
      'Age': " + travelInsuranceSearchBies.Age + @",
      'DOB': '" + Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + @"',
      'Smoke': '" + travelInsuranceSearchBies.Smoke + @"',
      'Earning': '" + travelInsuranceSearchBies.Income + @"',
      'MaritialStatus': 'Single',
      'Phone': '" + travelInsuranceSearchBies.Mobile + @"',
      'Email': '" + travelInsuranceSearchBies.Email + @"',
      'SumAssured': '" + view_Insurance.sumAssured + @"',
      'PolicyTerm': " + POLICY_TERM + @",
      'PPT': " + POLICY_TERM + @",
      'Frequency': '" + Frequency + @"',
      'BHB_Ind': 'Yes',
      'TUB_Ind': 'No',
      'PWB_Ind': 'No',
      'ADB_Ind': 'No',
      'CI_Ind': 'Yes',
      'PD_Ind': 'No',
      'HCB_ind': 'Yes',
      'DSA_ind': 'No',
      'ADB': 100000,
      'ATPD': 100000,
      'CI': 1200000,
      'HCB': 250000,
      'CIC_Ind': 'Yes',
      'CIC_SumAssured': 1200000,
      'CIC_ClaimOption': 'Multi',
      'CIC_PolicyTerm': 30,
      'TopupRate': 10,
      'TotalBenefit': 2,
      'PolicyOption': 'Life Cover with Level Sum Assured',
      'SpouseFirstName': '',
      'SpouseLastName': '',
      'SpouseDOB': '',
      'SpouseGender': '',
      'SpouseAge':0,
      'SpouseTobbacoUser': '',
      'AdditionalBenefit': 'Top-up Benefit',
      'TopUpBenefitPercentage': '10%',
      'PayoutOption': 'Lumpsum',
      'PayoutMonths': '',
      'PayoutPercentageLumpsum': '100%',
      'PayoutPercentageLevelIncome': '',
      'PayoutPercentageIncreasingIncome': '',
      'TransID': ''
    }
  }
]
            ";
                        EdelWisesRequest edel = new EdelWisesRequest();
                        quotationDetail = new GetICICIResponseDetails
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            BHB_Ind = "Yes",
                            TUB_Ind = "No",
                            PWB_Ind = "No",
                            ADB_Ind = "No",
                            PD_Ind = "No",
                            HCB_ind = "Yes",
                            DSA_ind = "No",
                            ADB = 100000,
                            ATPD = 100000,
                            CI = 1200000,
                            HCB = 250000,
                            CIC_Ind = "Yes",
                            CIC_SumAssured = 1200000,
                            CIC_ClaimOption = "Multi",
                            CIC_PolicyTerm = 30,
                            TopupRate = 10,
                            TotalBenefit = 2,
                            PolicyOption = "Life Cover with Level Sum Assured",
                            SpouseFirstName = "",
                            SpouseLastName = "",
                            SpouseDOB = "",
                            SpouseGender = "",
                            SpouseAge = quotationDetail.SpouseAge,
                            SpouseTobbacoUser = "",
                            AdditionalBenefit = "Top-up Benefit",
                            TopUpBenefitPercentage = "10%",
                            PayoutOption = "LUMSUM",
                            PayoutMonths = "",
                            PayoutPercentageLumpsum = "100%",
                            PayoutPercentageLevelIncome = "",
                            PayoutPercentageIncreasingIncome = "",
                            TransID = ""

                        };


                        string postdata = JsonConvert.SerializeObject(quotationDetail);

                        quotationDetail.hdnZindagiPlusdata = ResponceData1.Replace("'", "\"");

                        break;


                    case 10011:
                        //termproposal = new Term_ProposalCreation
                        //{
                        //    PlanId=view_Insurance.CustID,

                        //};
                        //string postData = JsonConvert.SerializeObject(termproposal);
                        //   var response = requestHandler.Response("https://iciciprulife-preprod.apigee.net/v1/partnerintegrator?apikey=uZe2ivHTNbXZuAv3rtkKEAUGtLCNG2Wm", postData.Replace("/","/"));
                        //   if (response["Status"] == "200")
                        //   {
                        //       var responseString = response["response"].ToString();
                        //       var result = JsonConvert.DeserializeObject<StartTravelPremiumAPIResponse>(responseString);


                        quotationDetail = new GetICICIResponseDetails
                        {
                            FullName = travelInsuranceSearchBies.Name,
                            Gender = travelInsuranceSearchBies.Gender,
                            Age = Convert.ToInt32(travelInsuranceSearchBies.Age),
                            DOB = Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/"),
                            Smoke = travelInsuranceSearchBies.Smoke,
                            Earning = travelInsuranceSearchBies.Income,
                            MaritialStatus = "",
                            Phone = travelInsuranceSearchBies.Mobile,
                            Email = travelInsuranceSearchBies.Email,
                            sumAssured = view_Insurance.sumAssured.ToString(),
                            PolicyTerm = Convert.ToInt32(POLICY_TERM),
                            PPT = Convert.ToInt32(POLICY_TERM),
                            totalPremium = view_Insurance.premium,
                            PlanName = view_Insurance.PlanName,
                            Frequency = Frequency,
                            Company = view_Insurance.CompanyID.ToString(),
                            pdfName = view_Insurance.pdfName,
                            State = quotationDetail.State
                        };

                        break;
                    default:
                        break;

                }

                return quotationDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }

        public GetICICIResponseDetails SubmitProposalICICI(GetICICIResponseDetails zpmodel)
        {
            GetICICIResponseDetails quotationDetail = new GetICICIResponseDetails();

            string fullN = zpmodel.FullName + "";
            var names = fullN.Split(' ');
            string lastName = "";
            string firstName = "";
            if (names.Length == 1)
            {
                firstName = names[0];
                lastName = "test";
            }
            else
            {
                firstName = names[0];
                lastName = names[1];
            }
            if (zpmodel.Smoke == "Yes")
            {
                zpmodel.Smoke = "1";
            }
            else
            {
                zpmodel.Smoke = "0";
            }
            string date = DateTime.ParseExact(zpmodel.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMMM-yyyy");
            //For Marital Status
            var mrts = "";
            if (zpmodel.MaritialStatus == "Single")
            {
                mrts = "697";
            }
            else if (zpmodel.MaritialStatus == "Married")
            {
                mrts = "696";
            }
            else if (zpmodel.MaritialStatus == "Divorced")
            {
                mrts = "695";
            }
            else
            {
                mrts = "694";
            }
            try
            {
                var result = "{ 'appNo': '', 'advisorCode': '00124001', 'source': 'DUMMYPAYOFLBOL', 'sourceKey': 'DUMMYPAYOFLBOLKEY', 'salesDataReqd': 'N', 'dependentFlag': 'Y', 'sourceTransactionId': '01600043', 'sourceOfFund': 'Salary', 'sourceOfFundDesc': '', 'buyersPinCode': '600002', 'sellersPinCode': '400097', 'clientId': '', 'uidId': '480', 'proposerInfos': { 'frstNm': '" + firstName + "', 'lstNm': '" + lastName + "', 'mrtlSts': '" + mrts + "', 'dob': '" + date + "', 'gender': '" + zpmodel.Gender + "', 'isStaff': '0', 'mobNo': '" + zpmodel.Phone + "', 'orgType': 'PRIVATE LTD', 'orgTypeDesc': '', 'relationWithLa': 'Spouse', 'sharePortfolio': 'Yes', 'desig': '', 'fathersName': '" + zpmodel.father_Name + "', 'mothersName': '" + zpmodel.Mother_name + "', 'spouseName': '" + zpmodel.spouse_name + "', 'ckycNumber': '12345678901234', 'kycDoc': { 'idPrf': 'PAN', 'addPrf': 'Passport', 'agePrf': 'ANM', 'itPrf': 'pancard', 'incomePrf': 'ITRETN', 'lddIdOthrDesc': '222222', 'lddIdNumber': 'DADPK3599E', 'lddIdExpiryDate': '11/21/2022' }, 'comunctnAddress': { 'pincode': '600002', 'landmark': 'NEAR FORUM MALL', 'state': '37', 'line1': 'NO.82, N.H.B. COLONY', 'line3': 'PUDUPET', 'city': 'CHENNAI', 'country': 'India', 'line2': 'GOYATHOPPU STREET' }, 'nri': { 'passportNo': '', 'dateOfArrivingIndia': '', 'dateOfLeavingIndia': '', 'durationOfStayInYear': '', 'durationOfStayInMonth': '', 'nameOfEmployer': '', 'travelDetails': '', 'bankType': '', 'countryOfResidence': '', 'nriTINIssuingCountry': '', 'tinNum': '', 'tinNum2': '', 'tinNum3': '', 'nriBirthCountry': '', 'placeOfBirthNRI': '', 'countryOfResidence2': '', 'countryOfResidence3': '', 'nriOtherResCountry': '', 'countryOfNationality': '', 'nriTaxResidentUS': '', 'purposeOfStay': '' }, 'education': '680', 'yrInService': '10', 'prmntAddress': { 'pincode': '600002', 'landmark': 'NEAR FORUM MALL', 'state': '37', 'line1': 'NO.82, N.H.B. COLONY', 'line3': 'PUDUPET', 'city': 'CHENNAI', 'country': 'India', 'line2': 'GOYATHOPPU STREET' }, 'lifeStageDetails': { 'calculator': '', 'goals': '', 'lifeStage': '', 'investmentPreferance': '', 'subGoals': '' }, 'occ': 'SPVT', 'occDesc': '', 'myProf': '', 'indsTypeQuestion' : 'No', 'indsType': '', 'indsTypeDesc': 'No', 'nameOfOrg': 'Indian Bank', 'nameOfOrgDesc' : '', 'objective': 'Both', 'objectiveOthers' : '', 'annIncme': '500000', 'panNo': 'BMIPS2819G', 'aadharCardNo': '', 'photoSubmitted': 'Yes', 'nationality': 'Indian', 'email': 'JAYSUDAA.G@GMAIL.COM', 'pltclyExpsd': 'No', 'landLnNo': '', 'stdNo': '', 'rural_lan': '', 'rural_policyno': '', 'rural_policyamt': '', 'rstSts': '' }, 'healthDetails': [{ 'answer1': '5', 'code': 'HQ01', 'answer3': '', 'answer2': '5 feet 3 inches', 'answer4': '' }, { 'answer1': '3', 'code': 'HQ02', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': '60', 'code': 'HQ03', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ05', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ06', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ07', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ09', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ125', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ144', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ165', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ166', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer15': 'No', 'answer1': 'No', 'code': 'HQ167', 'answer3': 'No', 'answer2': 'No', 'answer4': 'No', 'answer5': 'No', 'answer6': 'No', 'answer7': 'No', 'answer8': 'No', 'answer9': 'No', 'answer10': 'No', 'answer11': 'No', 'answer12': 'No', 'answer13': 'No', 'answer14': 'No' }, { 'answer15': 'No', 'answer1': 'No', 'code': 'HQ188', 'answer3': 'No', 'answer2': 'No', 'answer4': 'No', 'answer5': 'No', 'answer6': 'No', 'answer7': 'No', 'answer8': 'No', 'answer9': 'No', 'answer10': 'No', 'answer11': 'No', 'answer12': 'No', 'answer13': 'No', 'answer14': 'No' }, { 'answer1': 'No', 'code': 'HQ21', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ24', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'No', 'code': 'HQ168', 'answer3': '', 'answer2': '', 'answer4': '' }, { 'answer1': 'Yes', 'code': 'HQ61', 'answer3': '', 'answer2': '', 'answer4': '' }], 'productSelection': { 'CIBenefit': '0', 'premiumPayingFrequency': 'Yearly', 'salesChannel':'20', 'policyTerm': '7', 'premiumPayingTerm': '7', 'sumAssured': '5000000', 'productType': 'TRADITIONAL', 'productName': 'ICICI Pru iProtect Smart', 'productId': 'T51', 'LifeCoverOption': 'Life', 'lumpsumPercentage' : '0', 'premiumpaymentoption': 'Regular Pay', 'option': 'Lump-Sum', 'tobacco': '" + zpmodel.Smoke + "', 'aDHB': 0}, 'mwpaOpted': 'No', 'mwpaBenefit': '', 'nomineeInfos': { 'apnteDtls': { 'frstNm': '', 'relationship': '', 'gender': '', 'dob': '', 'lstNm': '' }, 'frstNm': 'JAYALAKSHMI S', 'relationship': 'Wife', 'gender': 'Female', 'dob': '04/19/1982', 'lstNm': 'Swami' }, 'beneficiaryDtls': [{ 'frstNm': '', 'lstNm': '', 'relationship': '', 'dob': '', 'shareOfBenefit': '', 'isMinor': '' }, { 'frstNm': '', 'lstNm': '', 'relationship': '', 'dob': '', 'shareOfBenefit': '', 'isMinor': '' }], 'trusteeDtls': [{ 'name': 'Nivedha', 'dob': '18-JUL-1995', 'trusteeType': 'Entity', 'address': 'Tuty', 'city': 'Thoothukudi', 'pincode': '628003', 'state': '37', 'panNo': 'FCAPS5230K', 'mobileNo': '9999999999', 'email': 'bbb@gmail.com', 'salutation' : 'Ms' }], 'lifeAssrdInfos': { 'frstNm': 'Alice', 'lstNm': 'Doe', 'dob': '16-April-1980', 'mrtlSts': '696', 'gender': 'Male', 'pinCode': '600002', 'relationship': 'Spouse', 'orgType': 'PRIVATE LTD', 'orgTypeDesc': '', 'mobNo': '9840466886', 'landLnNo': '', 'stdNo': '', 'nationality': 'Indian', 'aadharCardNo': '', 'education': '680', 'kycDoc': { 'idPrf': 'Passport', 'addPrf': 'Bank Letter', 'agePrf': 'PAN', 'itPrf': 'pancard' }, 'nri': { 'passportNo': '', 'dateOfArrivingIndia': '', 'dateOfLeavingIndia': '', 'durationOfStayInYear': '', 'durationOfStayInMonth': '', 'nameOfEmployer': '', 'travelDetails': '', 'bankType': '', 'countryOfResidence': '', 'nriTINIssuingCountry': '', 'tinNum': '', 'tinNum2': '', 'tinNum3': '', 'nriBirthCountry': '', 'placeOfBirthNRI': '', 'countryOfResidence2': '', 'countryOfResidence3': '', 'nriOtherResCountry': '', 'countryOfNationality': '', 'nriTaxResidentUS': '', 'purposeOfStay': '' }, 'desig': '', 'occ': 'SPVT', 'occDesc': '', 'myProf': '', 'nameOfOrg': 'Indian Bank', 'nameOfOrgDesc' : '', 'rstSts': '', 'annIncme': '500000' }, 'eiaDetails': { 'isEIAOpted': 'No', 'eiaInsuranceRepository': 'NDML', 'preferredInsuranceRepository': '', 'convertICICIToEIA': '', 'EIAAccountNumber': '' }, 'insuranceInfos' : [ { 'sum' : 22893, 'cmpany' : 'Birla Sun Life Insurance' }, { 'sum' : 228930, 'cmpany' : 'Kotak Life Insurance' } ], 'advisorSalesDetails': { 'channelType': 'BR', 'cusBankAccNo': '', 'bankName': 'TTIN', 'needRiskProfile': '', 'csrLimCode': '5032212', 'cafosCode': '999999999', 'oppId': '', 'fscCode': '01286361', 'spCode': '', 'bankBrnch': 'IE00', 'source': '0TIN', 'lanNo': '', 'selectedTab': 'broker_ci', 'subChannel': '' }, 'paymentData': { 'payModeOfDeposit': 'Cheque', 'payCheckDdNo': '001047', 'payAmount': '52251', 'payBankName': 'CITY UNION BANK', 'paySourceFund': 'Salary', 'payIsProposer': 'Yes', 'payCrtDate_str': '05-10-2016 17:27:23', 'payCrtdBy': '', 'payIfscCode': 'ICIC0000001', 'payInstruDate_str': '05-01-2016 00:00:00', 'payMicrNo': '600054003', 'payAccountNo': '001001000276870', 'payAccountHolderName': 'SUDHAKAR G', 'paySourceOthers': '', 'paymentStatus': 'Success', 'payCustFname': 'SUDHAKAR G', 'payCustLname': 'SWAMI', 'payFinalPremiumAmnt': '52251', 'payPolicyName': 'ICICI Pru Savings Suraksha', 'paySourceFlag': 'W', 'payTransStartTime_str': '05-10-2016 17:27:23', 'payPreimumFreq': '12', 'payFailReason': '', 'payGatewayTransId': '343636346', 'payPartPayMentId': '', 'payResponseCode': '', 'payResponseRemark': '', 'paySIOpted': '', 'paySIStatus': '', 'payTransEndTime_str': '', 'payUpdateDate_str': '', 'payUpdateBy': '', 'isChequeAns': '', 'paymentType': 'SLBL', 'ecsFlag': 'N', 'payAccountType': 'Saving' } }";
                result = result.Replace("'", "\"");
                var response = requestHandler.Response("https://iciciprulife-preprod.apigee.net/v1/partnerintegrator?apikey=uZe2ivHTNbXZuAv3rtkKEAUGtLCNG2Wm", result);
                if (response["Status"] == "200")
                {
                    zpmodel.responseRemarks = "Success";
                    var re1 = response["response"].ToString();
                    var result2 = JsonConvert.DeserializeObject<ICICIResponceDetails>(re1);
                    zpmodel.Url = result2.URL;
                    zpmodel.TransactionId = result2.transID;
                    zpmodel.totalPremium = result2.modalPremium;
                    //zpmodel.basePremium= result2.transID;
                }
                return zpmodel;
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }
        public long UpdateTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns)
        {
            int tid = 0;
            int company = Convert.ToInt32(srchTrvlIns.Company);
            var p = db.tbl_GetPremiumResponce.Where(x => x.CustID.ToString() == srchTrvlIns.CustID && x.Company == company).FirstOrDefault(); //the targeted record of the database
            if (p != null)
            {
                p.premiumWaiver = "true";
                db.SaveChanges();

            }
            return tid;
        }
        //For Kotak
        public List<GetIndiaFirstResponceDetail> GetPremiumListKotak(GetIndiaFirstResponceDetail srchTrvlIns)
        {
            int age = Convert.ToInt32(srchTrvlIns.Age);

            string premiumtype = "";
            string smoke = "";
            string gender = "";
            string fullN = srchTrvlIns.fullName + "";
            var names = fullN.Split(' ');
            string lastName = "";
            string firstName = "";
            string planname = "";
            string payoutOption = "";
            string Coverage1 = "";
            // srchTrvlIns.POLICY_TERM = (75 - age);
            // int POLICY_TERM= (75 - age);
            // POLICY_TERM = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
            int POLICY_TERM = Convert.ToInt32(srchTrvlIns.POLICY_TERM);
            if (POLICY_TERM <= 40)
            {
                Coverage1 = "Regular";
            }
            else
            {
                Coverage1 = "CoverageTill75";
            }
            if (srchTrvlIns.PlanName == null) { planname = "Life"; } else if (srchTrvlIns.PlanName == "") { planname = "Life"; } else { planname = srchTrvlIns.PlanName; }
            if (srchTrvlIns.Payout_option == null) { payoutOption = "Immediate Payout"; } else { payoutOption = srchTrvlIns.Payout_option; }
            if (names.Length == 1)
            { firstName = names[0]; lastName = "test"; }
            else { firstName = names[0]; lastName = names[1]; }
            string ppt = "";
            if (srchTrvlIns.PemiumType == "") { premiumtype = "Regular Pay"; ppt = ppt = POLICY_TERM.ToString(); }
            else if (srchTrvlIns.PemiumType == "Regular") { premiumtype = "Regular Pay"; ppt = POLICY_TERM.ToString(); }
            else if (srchTrvlIns.PemiumType == "Single") { premiumtype = "Single Premium"; srchTrvlIns.Frequency = "Single Premium"; ppt = "1"; }
            else if (srchTrvlIns.PemiumType == "Limited Pay") { premiumtype = "Limited Pay"; ppt = srchTrvlIns.pdfName; }
            else { premiumtype = "Regular Pay"; }
            if (srchTrvlIns.Smoke == "No" || srchTrvlIns.Smoke == "N") { smoke = "N"; }
            else { smoke = "Y"; }
            if (srchTrvlIns.Gender == "Male") { gender = "M"; } else { gender = "F"; }
            if (srchTrvlIns.Frequency == "Yearly") { srchTrvlIns.Frequency = "Annually"; } else if (srchTrvlIns.Frequency == "Monthly") { srchTrvlIns.Frequency = "Monthly"; }
            string date = DateTime.ParseExact(srchTrvlIns.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
            try
            {
                var result22 = "{ 'GetQuoteRequest': { 'Source': 'CHECKYOURPREMIUM', 'AgentID': '60642121', 'Coveage': '" + Coverage1 + "', 'DOB': '" + date + "', 'ExistingCustomer': 'N', '" + firstName + "': 'wqew', 'Gender': '" + gender + "', 'IsBackdated': 'N', 'LastName': '" + lastName + "', 'PaymentMode': '" + premiumtype + "', 'PayoutOption': '" + payoutOption + "', 'PlanOption': '" + planname + "', 'PolicyNo': '', 'PolicyTerm': '" + POLICY_TERM + "', 'PremiumPaymentTerm': '" + ppt + "', 'ResidencyStatus': 'IND', 'SourceName': 'KOTAK', 'State': '" + srchTrvlIns.city + "', 'SumAssured': '" + srchTrvlIns.sumAssured + "', 'TobaccoUser': '" + smoke + "', 'frequency': '" + srchTrvlIns.Frequency + "' }, 'riderData': { 'RiderFlag': 'Y', 'RiderName': 'CriticalIllnessBenefit', 'RiderSumAssured': '100000' } }";


                result22 = result22.Replace("'", "\"");
                var response = requestHandler.Response(KotakUrl, result22);

                if (response["Status"] == "200")
                {
                    var responseString = response["response"].ToString();
                    string json = (responseString).ToString();
                    var result = JsonConvert.DeserializeObject<Root>(json);
                    string d = result.GetQuotesResponse.Response;
                    XDocument docc = XDocument.Parse(d);
                    XNamespace ns = "";
                    List<XElement> responses1 = docc.Descendants(ns + "InsuredDetails").ToList();
                    List<XElement> responses2 = docc.Descendants(ns + "BaseCover").ToList();
                    List<XElement> responses3 = docc.Descendants(ns + "Premium").ToList();
                    var datt = responses3[1];
                    ;
                    GetIndiaFirstResponceDetail Responcedetail = new GetIndiaFirstResponceDetail();
                    for (int intc = 0; intc <= responses2.Count - 1; intc++)
                    {
                        Responcedetail.basePremium = responses2[intc].Element(ns + "Prem").Value;
                        string PremiumPaymentTerm = responses2[intc].Element(ns + "Term").Value.ToString();
                        string PremiumPaymentOption = responses2[intc].Element(ns + "PremiumPaymentOption").Value.ToString();
                        //string DeathBenefitOption = responses2[intc].Element(ns + "DeathBenefitOption").Value.ToString();
                        Responcedetail.POLICY_TERM = responses2[intc].Element(ns + "Term").Value.ToString();
                        Responcedetail.Suminsured = responses2[intc].Element(ns + "SumAssured").Value.ToString();
                        Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = datt.Element(ns + "BasicModalPremiumIncTax").Value.ToString();
                        //Responcedetail.INSTALL_PREMIUM_TAXINCLUSIVE = responses2[intc].Element(ns + "Prem").Value.ToString();

                    }

                    Responcedetail.Logo = "/Logo/Kotak.png";
                    //.....Responce
                    // Responcedetail.SUM_ASSURED = srchTrvlIns.SUM_ASSURED;
                    Responcedetail.Company = "10014";
                    Responcedetail.PlanName = "Kotak Life";
                    Responcedetail.Payout_option = "Kotak Logo";
                    Responcedetail.CustID = srchTrvlIns.CustID;
                    Responcedetail.pdfName = Responcedetail.PemiumType;
                    Responcedetail.SUM_ASSURED = srchTrvlIns.sumAssured;
                    //ppppppppppppppp
                    Responcedetail.accidentalDeathPremium = "";
                    Responcedetail.betterHalfPremium = "";
                    Responcedetail.criticalIllnessPremium = "";
                    Responcedetail.error = "";
                    Responcedetail.hcbPremium = "";
                    Responcedetail.permanentDisabilityPremium = "";
                    Responcedetail.premium = "";
                    Responcedetail.premiumWaiver = "";

                    Responcedetail.SearchId = SaveResponceIndiaFirstTermiLife(Responcedetail);
                    List<GetIndiaFirstResponceDetail> lst = new List<GetIndiaFirstResponceDetail>();
                    lst.Add(Responcedetail);
                    return lst;

                    //}

                }
            }
            catch (Exception ex)
            {
                return new List<GetIndiaFirstResponceDetail>();
            }
            return new List<GetIndiaFirstResponceDetail>();


        }
        public Kotak GetQuotationDetailKotak(Kotak zpmodel)
        {
            Kotak quotationDetail = new Kotak();

            try
            {
                if (zpmodel.Company != "10014")
                {
                    if (zpmodel.Smoke == "true") { zpmodel.Smoke = "Yes"; }
                    else { zpmodel.Smoke = "No"; }
                    if (zpmodel.SpouseAge == ",") { zpmodel.SpouseAge = "0"; } else { zpmodel.SpouseAge = zpmodel.SpouseAge; }
                    var ResponceData1 = @"[
  {
    'ZPModel': {
      'FullName': '" + zpmodel.FullName + @"',
      'Gender': '" + zpmodel.Gender + @"',
      'Age': " + zpmodel.Age + @",
      'DOB': '" + Convert.ToDateTime(zpmodel.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + @"',
      'Smoke': '" + zpmodel.Smoke + @"',
      'Earning': 'Above 20 Lakhs',
      'MaritialStatus':'" + zpmodel.MaritialStatus.Replace(",", "") + @"',
      'Phone': '" + zpmodel.Phone + @"',
      'Email': '" + zpmodel.Email + @"',
      'SumAssured': '" + zpmodel.sumAssured + @"',
      'PolicyTerm': " + zpmodel.PolicyTerm + @",
      'PPT': " + zpmodel.PolicyTerm + @",
      'Frequency': '" + zpmodel.Frequency + @"',
      'BHB_Ind': 'Yes',
      'TUB_Ind': 'No',
      'PWB_Ind': 'No',
      'ADB_Ind': 'No',
      'CI_Ind': 'No',
      'PD_Ind': 'No',
      'HCB_ind': 'Yes',
      'DSA_ind': 'No',
      'ADB': 100000,
      'ATPD': 100000,
      'CI': 1200000,
      'HCB': 250000,
      'CIC_Ind': 'No',
      'CIC_SumAssured': 1200000,
      'CIC_ClaimOption': 'Single',
      'CIC_PolicyTerm': 30,
      'TopupRate': 10,
      'TotalBenefit': 2,
      'PolicyOption': 'Life Cover with Level Sum Assured',
      'SpouseFirstName': '" + zpmodel.SpouseFirstName.Replace(",", "") + @"',
      'SpouseLastName': '" + zpmodel.SpouseLastName.Replace(",", "") + @"',
      'SpouseDOB': '" + zpmodel.SpouseDOB.Replace(",", "") + @"',
      'SpouseGender': '" + zpmodel.SpouseGender.Replace(",", "") + @"',
      'SpouseAge':" + zpmodel.SpouseAge.Replace(",", "") + @",
      'SpouseTobbacoUser': '" + zpmodel.SpouseTobbacoUser.Replace(",", "") + @"',
      'AdditionalBenefit': 'Top-up Benefit',
      'TopUpBenefitPercentage': '10%',
      'PayoutOption': 'Lumpsum',
      'PayoutMonths': '',
      'PayoutPercentageLumpsum': '100%',
      'PayoutPercentageLevelIncome': '',
      'PayoutPercentageIncreasingIncome': '',
      'TransID': ''
    }
  }
]
            ";
                    zpmodel.hdnZindagiPlusdata = ResponceData1;
                    zpmodel.hdnZindagiPlusdata = ResponceData1.Replace("'", "\"");
                    //  zpmodel.hdnZindagiPlusdata = ResponceData1.Replace(",,", "\",");


                    return zpmodel;
                }
                else
                {
                    // 23-Jan-1987

                    quotationDetail.Company = zpmodel.Company;
                    quotationDetail.DOB = DateTime.ParseExact(zpmodel.DOB, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                    return quotationDetail;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return quotationDetail;

        }
        public List<destinationList> GetKotakplan(string ptype)
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var Coverage = db.Kotak_Plan_tbl.AsQueryable().AsEnumerable().Where(x => x.Premium_type == ptype).Select(x => new { PlanName = x.PlanName.ToString() }).Distinct().ToList().OrderBy(x => x.PlanName).Select(x => new destinationList { text = x.PlanName, value = x.PlanName }).ToList();

                return Coverage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<destinationList>();

        }
    }
}
