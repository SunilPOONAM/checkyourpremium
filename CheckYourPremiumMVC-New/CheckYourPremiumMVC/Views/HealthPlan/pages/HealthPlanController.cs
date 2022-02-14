using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Business;
using Domain;
using CheckYourPremiumMVC.Models;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace CheckYourPremiumMVC.Controllers
{
    public class HealthPlanController : Controller
    {
        //
        // GET: /HealthPlan/
        public IHealthBusiness _IHealthBusiness;
        public ITravelBusiness _ITravelBusiness;
        public ICommonBusiness _IProposerBusiness;
        public ICommonBusiness _ITravellerBusiness;

        public List<string> triptype;
        public List<string> relations;
        public List<string> isPassport;
        string secretKey = string.Empty;
        string apiKey = string.Empty;
        public HealthPlanController()
        {
            _IHealthBusiness = new HealthBusiness();
            _ITravelBusiness = new TravelBusiness();
            _IProposerBusiness = new ProposerBusiness();
            _ITravellerBusiness = new TravellerBusiness();
            triptype = new List<string>();
            relations = new List<string>();
            isPassport = new List<string>();
            triptype.Add("Single");
            triptype.Add("Multi");

            isPassport.Add("Yes");
            isPassport.Add("No");
            secretKey = System.Configuration.ConfigurationSettings.AppSettings["SBIClientId"].ToString();
            apiKey = System.Configuration.ConfigurationSettings.AppSettings["SBISecretKey"].ToString();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetQuoteDetail()
        {
            BindDropdown();
            return View();
        }

        public void BindDropdown()
        {
            var destinations = _IHealthBusiness.GetAllCity();
            SelectList destinationsSelectList = new SelectList(destinations, "text", "value");
            ViewData["Cities"] = destinationsSelectList;
            //SelectList listTripType = new SelectList(triptype);
            //ViewData["listTripType"] = listTripType;
        }
        public void BindCoverage()
        {
            var Coverages = _IHealthBusiness.GetAllCoverAge();
            SelectList CoveragesSelectList = new SelectList(Coverages, "text", "value");
            ViewData["Coverage"] = CoveragesSelectList;
            //SelectList listTripType = new SelectList(triptype);
            //ViewData["listTripType"] = listTripType;
        }
        public void BindCheckForYear()
        {
            var CoveragesYear = _IHealthBusiness.GetAllYear();
            SelectList CoveragesYearSelectList = new SelectList(CoveragesYear, "text", "value");
            ViewData["CoverageYear"] = CoveragesYearSelectList;
            //SelectList listTripType = new SelectList(triptype);
            //ViewData["listTripType"] = listTripType;
        }
        //Poonam......................
        public void BindBajajRelation()
        {
            var relation = _IHealthBusiness.GetBajajRelation();
            SelectList selectList = new SelectList(relation, "text", "value");
            ViewData["relation"] = selectList;
        }
        public void BindBajajOccupation()
        {
            var occupation = _IHealthBusiness.GetBajajOccupation();
            SelectList selectList = new SelectList(occupation, "text", "value");
            ViewData["occupation"] = selectList;
        }
        //poonam .......End.....code
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetQuoteDetail(HealthPlanDetails Health)
        {
            try
            {
                //string SumInsurred = "100000.00";
                string SumInsurred = "150000.00";
                if (!ModelState.IsValid)
                {
                    BindDropdown();
                    return View(Health);
                }
                var Tid = _IHealthBusiness.SaveHealthSearch(Health);
                int Child = Convert.ToInt32(Health.son);
                int Adult = Convert.ToInt32(Health.chkspouses);
                int Self = Convert.ToInt32(Health.chkself);
                int ageSelf = Convert.ToInt32(Health.ageSelf);
                if(ageSelf>=60)
                {
                    SumInsurred = "100000.00";
                }
                int ageSopuse = Convert.ToInt32(Health.ageSpouse);
                decimal ageChild1 = Convert.ToDecimal(Health.ageChild1);
                decimal ageChilde2 = Convert.ToDecimal(Health.ageChild2);
                int ageChild3 = Convert.ToInt32(Health.ageBrother);
                int ageChild4 = Convert.ToInt32(Health.ageMother);
                string name = Health.Full_Name;
                string Gender = Health.Gender;
                string MobileNo = Health.MobileNo;
                string city = Health.City;
                //var encoded = Encrypt(Tid.ToString(), "sblw-3hn8-sqoy19").Replace("+", "=").Replace("/", "-");
                var encoded = Encrypt(Tid.ToString()).Replace("+", " ").Replace("/", "@");
                string msg = "Hi%20" + name + "%20, Start your happy days - share, compare, buy protection at Check Your Premium. Term and Condition apply. Visit  https://bit.ly/32XDHmQ";
                string phone = MobileNo;
               //  Hi Dilip, Start your happy days - share, compare, buy protection at Check Your Premium. T&C apply. Visit  https://bit.ly/32XDHmQ
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/vb/apikey.php?apikey=71e6646b9e66e6941b61&senderid=CYPIWA&route=3&number=" + phone + "&message=" + msg + "");
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
                return RedirectToAction("QResult/" + encoded, "HealthPlan", new { @AgeAdult = ageSelf, @Agespouse = ageSopuse, @AgeChild1 = ageChild1, @AgeChild2 = ageChilde2, @Self = Self, @Adult = Adult, @Child = Child, @city = city, @Gender = Gender, @name = name, @MobileNo = MobileNo });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        // poonam............
        //..................Url Encrypt.......................
        //public string Encode(string encodeMe)
        //{
        //    byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
        //    return Convert.ToBase64String(encoded);
        //}

        //public static string Decode(string decodeMe)
        //{
        //    byte[] encoded = Convert.FromBase64String(decodeMe);
        //    return System.Text.Encoding.UTF8.GetString(encoded);
        //}
        //public static string Encrypt(string input, string key)
        //{
        //    byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
        //    TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
        //    tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
        //    tripleDES.Mode = CipherMode.ECB;
        //    tripleDES.Padding = PaddingMode.PKCS7;
        //    ICryptoTransform cTransform = tripleDES.CreateEncryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        //    tripleDES.Clear();
        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //}
        //public static string Decrypt(string input, string key)
        //{
        //    input = input.Replace("-", "/");
        //    byte[] inputArray = Convert.FromBase64String(input);
        //    TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
        //    tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
        //    tripleDES.Mode = CipherMode.ECB;
        //    tripleDES.Padding = PaddingMode.PKCS7;
        //    ICryptoTransform cTransform = tripleDES.CreateDecryptor();
        //    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
        //    tripleDES.Clear();
        //    return UTF8Encoding.UTF8.GetString(resultArray);
        //}  
        //public static string Encrypt(string clearText)
        //{
        //    string EncryptionKey = "PREMCHECKDATAT76677677HGHHJ";
        //    byte[] clearBytes = UTF8Encoding.UTF8.GetBytes(clearText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            clearText = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return clearText;
        //}

        //public static string Decrypt(string cipherText)
        //{
        //    string EncryptionKey = "PREMCHECKDATAT76677677HGHHJ";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}
        //..................................................
        //...........................
     string plainText;
     string passPhrase = "Pas5pr@se";
     string saltValue = "s@1tValue";
     string hashAlgorithm = "MD5";
     int passwordIterations = 2;
     string initVector = "@1B2c3D4e5F6g7H8";
     int keySize = 256;

    //public string Encrypt(string plainText)
    //{
    //    byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //    byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
    //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
    //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
    //    byte[] keyBytes = password.GetBytes(keySize / 8);
    //    RijndaelManaged symmetricKey = new RijndaelManaged();
    //    symmetricKey.Mode = CipherMode.CBC;
    //    ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
    //    MemoryStream memoryStream = new MemoryStream();
    //    CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    //    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
    //    cryptoStream.FlushFinalBlock();
    //    byte[] cipherTextBytes = memoryStream.ToArray();
    //    memoryStream.Close();
    //    cryptoStream.Close();
    //    string cipherText = Convert.ToBase64String(cipherTextBytes);
    //    return cipherText;
    //}

    //public string Decrypt(string cipherText)
    //{
    //    byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //    byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);        
    //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
    //    PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
    //    byte[] keyBytes = password.GetBytes(keySize / 8);
    //    RijndaelManaged symmetricKey = new RijndaelManaged();
    //    symmetricKey.Mode = CipherMode.CBC;
    //    ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
    //    MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
    //    CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
    //    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
    //    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
    //    memoryStream.Close();
    //    cryptoStream.Close();
    //    string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    //    return plainText;
    //}
     public static string Encrypt(string clearText)
     {
         string EncryptionKey = "abc123";
         byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
         using (Aes encryptor = Aes.Create())
         {
             Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
             encryptor.Key = pdb.GetBytes(32);
             encryptor.IV = pdb.GetBytes(16);
             using (MemoryStream ms = new MemoryStream())
             {
                 using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                 {
                     cs.Write(clearBytes, 0, clearBytes.Length);
                     cs.Close();
                 }
                 clearText = Convert.ToBase64String(ms.ToArray());
             }
         }
         return clearText;
     }
     public static string Decrypt(string cipherText)
     {
         string EncryptionKey = "abc123";
         cipherText = cipherText.Replace(" ", "+");
         cipherText = cipherText.Replace("@", "/");
         byte[] cipherBytes = Convert.FromBase64String(cipherText);
         using (Aes encryptor = Aes.Create())
         {
             Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
             encryptor.Key = pdb.GetBytes(32);
             encryptor.IV = pdb.GetBytes(16);
             using (MemoryStream ms = new MemoryStream())
             {
                 using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                 {
                     cs.Write(cipherBytes, 0, cipherBytes.Length);
                     cs.Close();
                 }
                 cipherText = Encoding.Unicode.GetString(ms.ToArray());
             }
         }
         return cipherText;
     }

        //..............
        public ActionResult QResult(string ID, string AgeAdult, string Agespouse, string AgeChild1, string AgeChild2, string Self, string Adult, string Child, string city, string Gender,string name,string MobileNo)
        {
            try
            {
                BindCoverage();
                BindCheckForYear();
                ViewData["son"] = Child;
                ViewData["ageSelf"] = AgeAdult ?? "0";
                ViewData["Agespouse"] = Agespouse ?? "0";
                ViewData["AgeChild1"] = AgeChild1 ?? "0";
                ViewData["AgeChild2"] = AgeChild2 ?? "0";
                ViewData["Self"] = Self;
                ViewData["Spouses"] = Adult;
                ViewData["HID"] = ID;
                ViewData["city"] = city;
                ViewData["Gender"] = Gender;
                ViewData["name"] = name;
                ViewData["phone"] = MobileNo;
                //Login Session Code..........
                //Session["HID3"] = ViewData["HID"].ToString();
                //Session["Spouses3"] = ViewData["Spouses"].ToString();
                //Session["Self3"] = ViewData["Self"].ToString();
                //Session["ageSelf3"] = ViewData["ageSelf"].ToString();
                //Session["son3"] = ViewData["son"].ToString();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        [HttpPost]
        public JsonResult BindPremiumList(int draw, int start, int length, string searchTxt, string AgeAdult, string AgeSpouse, string AgeChild1, string AgeChild2, string Self, string Adult, string Child, string SumInsured, string CoverForYear, string ID, string city, string Gender, string name, string MobileNo)
        {
            JsonTableData jsonData = new JsonTableData();

            try
            {
                string searchKey = Request["search[value]"];
                string order = Request["order[0][column]"];
                string orderby = Request["order[0][dir]"];
                searchTxt = searchKey.Trim().ToLower();
                HealthPlanDetails objSearchHealthInsurance = new HealthPlanDetails();
                View_HealthinsuranceModel objSearchHealthInsurance1 = new View_HealthinsuranceModel();
                objSearchHealthInsurance.ageSelf = AgeAdult;
                objSearchHealthInsurance.ageSpouse = AgeSpouse;
                objSearchHealthInsurance.ageChild1 = AgeChild1;
                objSearchHealthInsurance.ageChild2 = AgeChild2;
                objSearchHealthInsurance.Self = Self;
                objSearchHealthInsurance.son = Child;
                objSearchHealthInsurance.Spouses = Adult;

                objSearchHealthInsurance.SumInsured = SumInsured;
                objSearchHealthInsurance1.SumAssured = SumInsured;
                objSearchHealthInsurance1.ageSelf = AgeAdult;
                objSearchHealthInsurance1.AgeFrom = AgeSpouse;
                objSearchHealthInsurance1.AgeTo = AgeChild1;
                objSearchHealthInsurance1.AgeFromDuration = AgeChild2;
                objSearchHealthInsurance1.Self = Self;
                objSearchHealthInsurance1.son = Child;
                objSearchHealthInsurance1.Spouses = Adult;
                objSearchHealthInsurance1.CoverForYear = CoverForYear;
                objSearchHealthInsurance1.Type = city;
               // var encoded = Decrypt(ID.ToString(), "sblw-3hn8-sqoy19").Replace("=", "+").Replace("-", "/"); ; 
                var encoded = Decrypt(ID.ToString());
                objSearchHealthInsurance1.PlanID = encoded;
                objSearchHealthInsurance.CoverForYear = CoverForYear;
                //objSearchHealthInsurance.ag = stayDays;
                //objSearchHealthInsurance.destination = destination;
                AllCompanyDetails srchproduct = new AllCompanyDetails();
                CompareDetails com = new CompareDetails();
                List<View_HealthinsuranceModel> list = _IHealthBusiness.GetHealthPremiumList(objSearchHealthInsurance);
                List<View_HealthinsuranceModel> lst = _IHealthBusiness.GetPremiumList(objSearchHealthInsurance1);
                List<AllCompanyDetails> list1 = _IHealthBusiness.GetAllCompanyDetails(srchproduct);
                List<CompareDetails> listCompare = _IHealthBusiness.GetCompareDetails(com);
                //(Original Cost x GST%)/100
                
              var commonList = list;
                list.AddRange(lst);
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
                        commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.SumAssured).ToList() : commonList.OrderByDescending(x => x.SumAssured).ToList();
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

                    decimal P = Convert.ToDecimal(item.BeforeServiceTax ?? "0");
                    decimal Premium = P * Convert.ToDecimal((item.Gsttax)) / 100;
                    decimal Premium1 = P + Premium;
                    string pre = Premium1.ToString();
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
                    foreach (var item2 in commonList3)
                    {
                        if (item.companyid == Convert.ToInt32(item2.Company_Id))
                        {
                            if (item.PremiumDesc == item2.Company_Plan)
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
                        if (item.companyid == Convert.ToInt32(item1.Company_Id))
                        {
                            if (item.PremiumDesc == item1.Company_Plan)
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
                    if (item.companyid == 10006)
                    {
                        int starPremium = Convert.ToInt32(item.Premium) * 18 / 100;
                        int premi = Convert.ToInt32(item.Premium) + starPremium;
                        item.Premium = premi.ToString();
                    }
                    //.............compare........................
                    //string comp = "<div style='display:none'><input type='text' class='compare1' value='" + Benefit + "'><input type='text' class='compare2' value='" + Co_Pay1 + "'><input type='text' class='compare3' value='" + Room_Rent1 + "'><input type='text' class='compare4' value='" + OPD + "'><input type='text' class='compare5' value='" + Day_Care_Treatment1 + "'><input type='text' class='compare6' value='" + Medical_Checkup + "'><input type='text' class='compare7' value='" + Pre_Existing_Disease_Covered_After + "'><input type='text' class='compare8' value='" + Domicilliary_Expenses + "'><input type='text' class='compare9' value='" + Organ_Donar_Expenses + "'><input type='text' class='compare10' value='" + Hospital_Cash_Daily_Limit + "'><input type='text' class='compare11' value='" + Maternity_Benefit + "'><input type='text' class='compare12' value='" + New_Born_Baby + "'><input type='text' class='compare13' value='" + Pre_Hospitalization1 + "'><input type='text' class='compare14' value='" + Post_Hospitalization1 + "'><input type='text' class='compare15' value='" + Ambulance_Charges1 + "'><input type='text' class='compare16' value='" + Health_Check_Up1 + "'><input type='text' class='compare17' value='" + Restoration_Benefit1 + "'><input type='text' class='compare18' value='" + Free_Look_Period1 + "'></div>";

                    //.........end........................
                    column1 = "<div class='selectProduct w3-padding' data-title=" + item.PremiumDesc + "  data-id=" + tbl + " > <input type='checkbox' class='addToCompare' value=\"" + i++ + "\"><span class='adCmps'>addToCompare</span><div style='display:none'><input type='text' class='compare1' value='" + Benefit + "'><input type='text' class='compare2' value='" + Co_Pay1 + "'><input type='text' class='compare3' value='" + Room_Rent1 + "'><input type='text' class='compare4' value='" + OPD + "'><input type='text' class='compare5' value='" + Day_Care_Treatment1 + "'><input type='text' class='compare6' value='" + Medical_Checkup + "'><input type='text' class='compare7' value='" + Pre_Existing_Disease_Covered_After + "'><input type='text' class='compare8' value='" + Domicilliary_Expenses + "'><input type='text' class='compare9' value='" + Organ_Donar_Expenses + "'><input type='text' class='compare10' value='" + Hospital_Cash_Daily_Limit + "'><input type='text' class='compare11' value='" + Maternity_Benefit + "'><input type='text' class='compare12' value='" + New_Born_Baby + "'><input type='text' class='compare13' value='" + Pre_Hospitalization1 + "'><input type='text' class='compare14' value='" + Post_Hospitalization1 + "'><input type='text' class='compare15' value='" + Ambulance_Charges1 + "'><input type='text' class='compare16' value='" + Health_Check_Up1 + "'><input type='text' class='compare17' value='" + Restoration_Benefit1 + "'><input type='text' class='compare18' value='" + Free_Look_Period1 + "'><a class='AddD' href='#' onclick='NavigateDetail(" + item.PremiumChartID + ")'>Buy This Plan</a><input type='text' class='PremiumChart' value='" + item.PremiumChartID + "'><input type='text' class='Premiumtotal' value='" + (item.ProductName == "Star Health Insurance" ? item.Premium.ToString() : pre) + "'></div><img src=" + item.Logo + " class='imgFill productImg'/></div><p><button type='button' id='" + btn + "' onclick=HideTable('" + tbl + "');>Details</button></p>";
                    string column2 = "<p>" + item.PremiumDesc.ToUpper() + "<p>";
                    string column3 = "<p>" + item.SumAssured + "<p>";
                    string column4Premium = "<span><i class='fa fa-inr' aria-hidden='true'></i></span> " + (item.ProductName == "Star Health Insurance" ? item.Premium.ToString() : pre) + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.PremiumChartID + ")'>Buy This Plan</a>";

                    List<string> common = new List<string>();
                    common.Add(column1);
                    common.Add(column2);
                    common.Add(column3);
                    // common.Add(column4);
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


            //    jsonData.recordsTotal = commonList.Count;
            //    jsonData.recordsFiltered = commonList.Count;
            //    jsonData.draw = draw;
            //    commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
            //    List<List<string>> DetailList = new List<List<string>>();
            //    string action = string.Empty;


            //    var i = 1;
            //    foreach (var item in commonList)
            //    {

            //        decimal P = Convert.ToDecimal(item.BeforeServiceTax??"0");
            //        decimal Premium = P * Convert.ToDecimal((item.Gsttax)) / 100;
            //        decimal Premium1 = P + Premium;
            //        string pre = Premium1.ToString();
            //        string column1 = "<img src=" + item.Logo + " />";
            //        string column2 = "<p>" + item.PremiumDesc.ToUpper() + "<p>";
            //        string column3 = "<p>" + item.SumAssured + "<p>";

            //        string column4Premium = "<span><i class='fa fa-inr' aria-hidden='true'></i></span> " + (item.ProductName == "Star Health Insurance" ? item.Premium.ToString() : pre) + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.PremiumChartID + ")'>Buy This Plan</a>";
            //        //string col4 = "<button  style='border:none;' onclick='navigateEdit(" + item.EDesigId + ")'  type='submit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button><button onclick='navigatedel(" + item.EDesigId + ")' style='border:none;' type='submit'><i class='fa fa-trash-o' aria-hidden='true'></i></button>";
            //        List<string> common = new List<string>();
            //        common.Add(column1);
            //        common.Add(column2);
            //        common.Add(column3);
            //        common.Add(column4Premium);
            //        DetailList.Add(common);
            //    }
            //    jsonData.data = DetailList;
            //    return new JsonResult()
            //    {
            //        Data = jsonData,
            //        MaxJsonLength = Int32.MaxValue,
            //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    };
            //}
            //catch (Exception ex)
            //{
            //    (new ErrorLog()).Error(ex.ToString(), "");
            //    return Json(null);
            //}
        }
        //        var commonList = list;
        //        list.AddRange(lst);
        //        commonList = list;
        //        if (!string.IsNullOrEmpty(searchTxt))
        //        {
        //            //commonList = commonList.Where(x => x.EDesigName.ToLower().Trim().Contains(searchTxt.ToLower().Trim())).ToList();
        //        }

        //        switch (order)
        //        {
        //            //case "1":
        //            //    commonList = orderby.Equals("asc") ? commonList.OrderBy(x => x.EDesigName).ToList() : commonList.OrderByDescending(x => x.EDesigName).ToList();
        //            //    break;
        //            default:
        //                commonList = orderby.Equals("desc") ? commonList.OrderBy(x => x.SumAssured).ToList() : commonList.OrderByDescending(x => x.SumAssured).ToList();
        //                break;
        //        }


        //        jsonData.recordsTotal = commonList.Count;
        //        jsonData.recordsFiltered = commonList.Count;
        //        jsonData.draw = draw;
        //        commonList = length != -1 ? commonList.Skip(start).Take(length).ToList() : commonList;
        //        List<List<string>> DetailList = new List<List<string>>();
        //        string action = string.Empty;


        //        var i = 1;
        //        foreach (var item in commonList)
        //        {

        //            decimal P = Convert.ToDecimal(item.BeforeServiceTax??"0");
        //            decimal Premium = P * Convert.ToDecimal((item.Gsttax)) / 100;
        //            decimal Premium1 = P + Premium;
        //            string pre = Premium1.ToString();
        //            string column1 = "<img src=" + item.Logo + " />";
        //            string column2 = "<p>" + item.PremiumDesc.ToUpper() + "<p>";
        //            string column3 = "<p>" + item.SumAssured + "<p>";

        //            string column4Premium = "<span><i class='fa fa-inr' aria-hidden='true'></i></span> " + (item.ProductName == "Star Health Insurance" ? item.Premium.ToString() : pre) + "<br src='#'><a class='AddD' href='#' onclick='NavigateDetail(" + item.PremiumChartID + ")'>Buy This Plan</a>";
        //            //string col4 = "<button  style='border:none;' onclick='navigateEdit(" + item.EDesigId + ")'  type='submit'><i class='fa fa-pencil-square-o' aria-hidden='true'></i></button><button onclick='navigatedel(" + item.EDesigId + ")' style='border:none;' type='submit'><i class='fa fa-trash-o' aria-hidden='true'></i></button>";
        //            List<string> common = new List<string>();
        //            common.Add(column1);
        //            common.Add(column2);
        //            common.Add(column3);
        //            common.Add(column4Premium);

        //            DetailList.Add(common);


        //        }
        //        jsonData.data = DetailList;
        //        return new JsonResult()
        //        {
        //            Data = jsonData,
        //            MaxJsonLength = Int32.MaxValue,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        (new ErrorLog()).Error(ex.ToString(), "");
        //        return Json(null);
        //    }
        //}

        //public ActionResult GetQuoteDetails(string Id, string tid)
        //{
        //    if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
        //    {
        //        return RedirectToAction("GetQuote");
        //    }
        //    SelectList lstRealtion = new SelectList(relations);
        //    ViewData["relations"] = lstRealtion;
        //    SelectList lstPassport = new SelectList(isPassport);
        //    ViewData["lstPassport"] = lstPassport;
        //    var quotationDetail = _IHealthBusiness.GetQuotationDetail(Id, tid);
        //    return View(quotationDetail);
        //    return View();
        //}
        public void BindDesease()
        {
            var desease = _ITravelBusiness.GetIllness();
            SelectList selectList = new SelectList(desease, "text", "value");
            ViewData["desease"] = selectList;
        }
        public void BindRelations(string planId)
        {

            var relations = _IHealthBusiness.GetRelations(planId);
            SelectList selectList = new SelectList(relations, "text", "value");
            ///  SelectList selectList = new SelectList(relations);
            if (planId != "0")
            {
                ViewData["relations"] = selectList;
            }
            else
            {
                ViewData["nomineeRelations"] = selectList;
            }

        }
        public void BindOccupation(string planId)
        {
            var occupationList = _IHealthBusiness.GetOccupation(planId);
            SelectList selectList = new SelectList(occupationList, "text", "value");
            ///  SelectList selectList = new SelectList(relations);
            ViewData["occupationList"] = selectList;

        }
        public void BindAssigneeRelation(string type, Int32 planId)
        {

            var CoverAgeAmount = _IHealthBusiness.GetAssigneeNominee(type, planId);
            SelectList CoverAgeAmountSelectList = new SelectList(CoverAgeAmount, "text", "value");
            ViewData["assigneerelations"] = CoverAgeAmountSelectList;

        }
        public ActionResult GetHealthQDetais(string Id, string tid)
        
        {
            try
            {
                var encoded = Decrypt(tid); //  poonam 
                //// Login Session Code.....
                //if (Session["UserName"] == null)
                //{
                //    Session["IdNum3"] = Id;

                //}

                //if (Session["UserName"] != null)
                //{

                //    HealthPlanDetails Health = new HealthPlanDetails();
                //    if (Id == null)
                //    {
                //        string Tid = Session["HID3"].ToString();
                //        tid = Tid;
                //        Id = Session["IdNum23"].ToString();
                //        Response.Cookies["page"].Value = "";
                //    }
                //}
                //else
                //{


                //    Response.Cookies["page"].Value = "GetHealthQDetais";
                //    Response.Cookies["page"].Expires = DateTime.Now.AddHours(1);
                //    return RedirectToAction("LoginDetails", "RegistrationLogin");
                //}
                ////End....................
                if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(tid))
                {
                    return RedirectToAction("GetQuote");
                }
                BindDesease();
                BindRelations(Id);
                BindOccupation(Id);
                BindRelations("0");
                BindAssigneeRelation("health", Convert.ToInt32(Id));
                //SelectList lstRealtion = new SelectList(relations);
                //ViewData["relations"] = lstRealtion;
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                var quotationDetail = _IHealthBusiness.GetQuotationDetail(Id, encoded);
                return View(quotationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }
      //  [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GetHealthQDetais(RegistrationDetails healthQuatationDetail)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    BindDesease();


                //    SelectList lstRealtion = new SelectList(relations);
                //    ViewData["relations"] = lstRealtion;
                //    SelectList lstPassport = new SelectList(isPassport);
                //    ViewData["lstPassport"] = lstPassport;
                //    return View(healthQuatationDetail);
                //}

                _IProposerBusiness.SaveInfo(healthQuatationDetail);
                _ITravellerBusiness.SaveInfo(healthQuatationDetail);

                string res = _IHealthBusiness.SubmitProposal(healthQuatationDetail);
                // For City and area
                ViewBag.city = healthQuatationDetail.proposerAreaCityId;
                ViewBag.area = healthQuatationDetail.proposerAreaId;
                ViewBag.residancecity = healthQuatationDetail.proposerResidenceAreaCityId;
                ViewBag.residancearea = healthQuatationDetail.proposerResidenceAreaId;
                ViewBag.pincode = healthQuatationDetail.proposerAreaPinCode;
                ViewBag.residancepincode = healthQuatationDetail.proposerResidenceAreaPinCode;
                BindDesease();

                //Response.Write("<script>alert('"+res+"')<script>");
                BindRelations(healthQuatationDetail.planId.ToString());
                BindOccupation(healthQuatationDetail.planId.ToString());
                BindRelations("0");
                BindAssigneeRelation("health", Convert.ToInt32(healthQuatationDetail.planId));
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                if (!string.IsNullOrEmpty(res) && !res.Contains("error"))
                {
                    ViewBag.SecretKey = secretKey;
                    ViewBag.APIKey = apiKey;
                    ViewBag.referenceId = res;
                   
                   

                  
                    
                    ///return RedirectToAction("ProposalStatus", "HealthPlan", new { @refid = res });
                    //////...........................SMS Sender...............................
                    //string url = "http://checkyourpremium.com//HealthPlan/QResult/" + encoded + "?AgeAdult=" + AgeAdult + "&Agespouse=" + AgeSpouse + "&AgeChild1=" + AgeChild1 + "&AgeChild2=" + AgeChild2 + "&Self=" + Self + "&Adult=" + Adult + "&Child=" + Child + "&city=" + city + "&Gender=" + Gender + "&name=" + name + "";
                    //////.....................send Sms

                    //string msg = "Hi%20" + name + "%20your Policy is ready to URl %20" + url;
                    //string phone = MobileNo;

                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/vb/apikey.php?apikey=71e6646b9e66e6941b61&senderid=CYPIWA&route=3&number=" + phone + "&message=" + msg + "");
                    //// HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/http-api.php?username=nitink&password=bulksms@123&senderid=CYPIWA& route=1&number=" + phone + "&message=" + msg + "");

                    //String responseString = String.Empty;
                    //try
                    //{
                    //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    //    {
                    //        using (Stream objStream = response.GetResponseStream())
                    //        {
                    //            using (StreamReader objReader = new StreamReader(objStream))
                    //            {
                    //                responseString = objReader.ReadToEnd();
                    //                objReader.Close();
                    //            }
                    //            objStream.Flush();
                    //            objStream.Close();
                    //        }
                    //        //lblErrormsg.Text = "Msg Sent";
                    //        response.Close();

                    //    }
                    //}
                    //catch (Exception e)
                    //{

                    //}
                    //.............................
                }
                else
                {
                    ViewData["Error"] = res.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");

                }
                return View(healthQuatationDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        public ActionResult GetHealthBajajDetais(string Id, string tid, string QuoteNo)
        {
            try
            {
                BindDesease();
                var decodetid = Decrypt(tid.ToString()); ;
                int decid = Convert.ToInt32(decodetid);
                BindRelations("0");
                BindAssigneeRelation("health", Convert.ToInt32(Id));
                //SelectList lstRealtion = new SelectList(relations);
                //ViewData["relations"] = lstRealtion;
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                BindBajajRelation();//ppppppppppppppppppppp
                BindBajajOccupation();//pppppppppppppppppp

                var quotationDetail = _IHealthBusiness.GetQuotationBsjajDetail(Id, decodetid, QuoteNo);
                try
                {
                    return View(quotationDetail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        //[ValidateInput(false)]
        [ValidateAntiForgeryToken]
         [HttpPost]
        public ActionResult GetHealthBajajDetais(RegistrationDetails healthQuatationDetail)
        {
            try
            {
                BindBajajRelation();//ppppppppppppppppppppp
                BindBajajOccupation();//pppppppppppppppppp
                _IProposerBusiness.SaveInfo(healthQuatationDetail);
                _ITravellerBusiness.SaveInfo(healthQuatationDetail);

                string res = _IHealthBusiness.SubmitProposal(healthQuatationDetail);

                BindDesease();


                BindRelations(healthQuatationDetail.planId.ToString());
                BindOccupation(healthQuatationDetail.planId.ToString());
                BindRelations("0");
                BindAssigneeRelation("health", Convert.ToInt32(healthQuatationDetail.planId));
                SelectList lstPassport = new SelectList(isPassport);
                ViewData["lstPassport"] = lstPassport;
                if (!string.IsNullOrEmpty(res) && !res.Contains("error"))
                {
                    ViewBag.SecretKey = secretKey;
                    ViewBag.APIKey = apiKey;
                    ViewBag.referenceId = res;
                    ///return RedirectToAction("ProposalStatus", "HealthPlan", new { @refid = res });
                }
                else
                {
                    ViewData["Error"] = res.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "");

                }
                BindBajajRelation();//ppppppppppppppppppppp
                BindBajajOccupation();//pppppppppppppppppp
 
                return View(healthQuatationDetail);
                ////.....................send Sms

                //string msg = "Hi%20" + name + "%20your Policy is ready to URl %20" + url;
                //string phone = MobileNo;

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/vb/apikey.php?apikey=71e6646b9e66e6941b61&senderid=CYPIWA&route=3&number=" + phone + "&message=" + msg + "");
                //// HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://sms.smsinsta.in/http-api.php?username=nitink&password=bulksms@123&senderid=CYPIWA& route=1&number=" + phone + "&message=" + msg + "");

                //String responseString = String.Empty;
                //try
                //{
                //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //    {
                //        using (Stream objStream = response.GetResponseStream())
                //        {
                //            using (StreamReader objReader = new StreamReader(objStream))
                //            {
                //                responseString = objReader.ReadToEnd();
                //                objReader.Close();
                //            }
                //            objStream.Flush();
                //            objStream.Close();
                //        }
                //        //lblErrormsg.Text = "Msg Sent";
                //        response.Close();

                //    }
                //}
                //catch (Exception e)
                //{

                //}
                //.............................
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();

        }

       
        public ActionResult ProposalStatus(string refid)
        {
            ViewBag.SecretKey = secretKey;
            ViewBag.APIKey = apiKey;
            ViewBag.referenceId = refid;
            return View();
        }
        
       // [HttpPost]
        public ActionResult GenerateToken(string refId)
        {
            var redirectToken = _IHealthBusiness.GenerateToken(refId);
            // var redirectToken = "60b393827113566ce99e5160373c5331";
            ViewBag.RedirectToken = redirectToken;
            return Json(new { token = redirectToken }, JsonRequestBehavior.AllowGet);
        }
        //[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult GenerateTokenBajaj(string refId)
        {
            //var redirectToken = _IHealthBusiness.GenerateToken(refId);
            // var redirectToken = "60b393827113566ce99e5160373c5331";
            ViewBag.RedirectToken = refId;
            return Json(new { token = refId }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPaymentStatus(string purchaseToken)
        {
            try
            {
                PolicyStatusDetails policystatus = new PolicyStatusDetails();
                if (!string.IsNullOrEmpty(purchaseToken))
                {
                    var policyStatus = _ITravelBusiness.PolicyStatus(purchaseToken);
                    if (policyStatus.Count == 4)
                    {
                        ViewBag.ReferenceNumber = (policyStatus["RefNumber"] ?? "").ToString();
                        ViewBag.PolicyNumber = policyStatus["PolicyNumber"].ToString();
                        ViewBag.RefId = policyStatus["RefId"].ToString();
                        ViewBag.PolicyStatus = policyStatus["PolicyStatus"].ToString();
                        policystatus.refrenceNumber = ViewBag.ReferenceNumber;
                        policystatus.PolicyNumber = ViewBag.PolicyNumber;
                        policystatus.refID = ViewBag.RefId;
                        policystatus.PolicyNumber = ViewBag.PolicyStatus;
                    //    var Tid = _IHealthBusiness.SavePolicy(policystatus);
                       // var Tid = _IHealthBusiness.SaveHealthSearch(Health);
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
            //try
            //{


                var response = _ITravelBusiness.GetDocument(refid);
                return File(response, "application/pdf");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return View();

        }

        public ActionResult GetCity(string pinCode)
        {
            try
            {
                var response = _IHealthBusiness.GetCityList(pinCode);
                // var result = response != null && !response[0].Contains("error") ? new { data = response, status = "success" } : new { data = response, status = "error" };
                //return Json(result, JsonRequestBehavior.AllowGet);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("");

        }
        public ActionResult GetArea(string pinCode, string cityId)
        {
            try
            {

                var response = _IHealthBusiness.GetArea(pinCode, cityId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("");
        }


        public ActionResult GetCityBajaj(string pinCode)
        {

            try
            {
                var relations = _IHealthBusiness.GetBlockCity(pinCode);
                if (relations.Count == 0)
                {
                    var response = _IHealthBusiness.GetCityList(pinCode);
                    // var result = response != null && !response[0].Contains("error") ? new { data = response, status = "success" } : new { data = response, status = "error" };
                    //return Json(result, JsonRequestBehavior.AllowGet);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("");
           
        }
        public ActionResult GetAreaBajaj(string pinCode, string cityId)
        {
            try
            {

                var response = _IHealthBusiness.GetArea(pinCode, cityId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json("");
        }

        public ActionResult GetQuoteSBI()
        {

            return View();
        }
        public ActionResult GetBajajPaymentStatus()
        {
            return View();
        }
    }
}
