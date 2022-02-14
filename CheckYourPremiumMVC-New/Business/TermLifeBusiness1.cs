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
        List<GetIndiaFirstResponceDetail> GetPremiumList(GetIndiaFirstResponceDetail srchTrvlIns);
        List<GetIndiaFirstResponceDetail> GetPremiumListHDFC(GetIndiaFirstResponceDetail srchTrvlIns);
        List<GetIndiaFirstResponceDetail> GetPremiumEdelWises(GetIndiaFirstResponceDetail srchTremIns);
        List<GetIndiaFirstResponceDetail> GetPremiumEdelWisesResponce(GetIndiaFirstResponceDetail srchTremIns);
        long SaveTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns);
        //  ZPModel GetQuotationDetail(string planId, string TId);
        string GetQuotationDetailIndiaFirst(GetIndiaFirstResponceDetail DataChangeEvt);
        ZPModel GetQuotationDetail(string planId, string TId, string POLICY_TERM, string Frequency, string State);
        ZPModel GetQuotationDetail(ZPModel zpmodel);
    }
    public class TermLifeBusiness : ITermLifeBusiness
    {
        private readonly CheckyrpremiumEntities db;
        private string baseUrl; string secretKey = string.Empty;
        string apiKey = string.Empty;
        private RequestHandler requestHandler = new RequestHandler();
        public TermLifeBusiness()
        {

            db = new CheckyrpremiumEntities();
            baseUrl = System.Configuration.ConfigurationSettings.AppSettings["baseURL"].ToString();
            //secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
            //apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        }
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
                                             }).ToList();
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

        public long SaveResponceIndiaFirstTermiLife(GetIndiaFirstResponceDetail Responcedetail)
        {

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
                CustID = Responcedetail.CustID,
                CoverAge = "1",

            };
            db.tbl_GetPremiumResponce.Add(tDetails);
            db.SaveChanges();
            var tid = tDetails.ID;
            return Convert.ToInt64(tid);
        }
        public long SaveTermLifeSearch(GetIndiaFirstResponceDetail srchTrvlIns)
        {
            try
            {
                srchTrvlIns.sumAssured = "10000000";
                srchTrvlIns.POLICY_TERM = "40";
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
                            PayoutOption = "https://uat.indiafirstlife.com/buy-online-insurance/term-plan/?src=CYP",
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
                            CustID = view_Insurance.CustID,
                            PayoutOption = " https://onlineinsuranceuat.hdfclife.com/thirdparty?appnum='" + view_Insurance.CustID + "'&mob='" + travelInsuranceSearchBies.Mobile + "'&dob='" + Convert.ToDateTime(travelInsuranceSearchBies.DOB).ToString("dd/MM/yyyy").Replace("-", "/") + "'&email='" + travelInsuranceSearchBies.Email + "'&mobcd=91",
                            Company = view_Insurance.CompanyID.ToString()
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
    }
}
