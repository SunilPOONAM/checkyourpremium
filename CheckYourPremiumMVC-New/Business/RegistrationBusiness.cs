using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;



namespace Business
{
    public interface IRegistrationBusiness
    {
        List<customSelectList> GetGender();
        List<customSelectList> GetCountry();
        //long saveRegistration(RegistrationDetails Details);
        //long saveLogin(RegistrationDetails Details);
        long saveRegistration(RegistrationDetails Details);
        long saveLogin(RegistrationDetails Details);
        string attamttimeupdate(LoginDetails Details);
        string attamttime(LoginDetails Details);
        string savePassword(LoginDetails details);
        List<RegistrationDetails> GetLoginData(RegistrationDetails objuserlogin);
        List<RegistrationDetails> GetregData(RegistrationDetails objuserlogin);
    }
    public class RegistrationBusiness : IRegistrationBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        public RegistrationBusiness()
        {

            db = new CheckyourpremiumliveEntities();

        }

        public List<customSelectList> GetGender()
        {
            try
            {
                //  var destinationList = db.Travel_Insurance.Where(x => x.Plans == "ABC").Select(x => new { x.Premium, x.SumInsured,x.Travel_id }).OrderByDescending(x => x.Travel_id).ToList();
                var gender = db.tb_GodigitGender.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.Gender.ToString(), text = x.Gender.ToString() }).Distinct().ToList();
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

                var gender = db.tbl_travelCountry.AsQueryable().AsEnumerable().Select(x => new customSelectList { value = x.County.ToString(), text = x.County.ToString() }).Distinct().ToList();
                return gender;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<customSelectList>();
        }
        public long saveRegistration(RegistrationDetails Details)
        {
            var tdetails = new Reg_tbl
            {
                FirstName = Details.FirstName,
                LastName = Details.LastName,
                EmailId = Details.EmailId,
                 Dob = DateTime.ParseExact(Details.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                //Dob = Convert.ToDateTime(Details.DOB),
              // Dob="1998-01-07 00:00:00.000",
                Gender = Details.Gender,
                MobileNo = Details.MobileNo,
                Address = Details.Address,
                Country = Details.Country,
                Password = Details.Password,
                ConfirmPassword = Details.ConfirmPassword,
                Picture = ""

            };
            db.Reg_tbl.Add(tdetails);
            db.SaveChanges();
            var tid = Details.Id;
            return tid;
        }
        public long saveLogin(RegistrationDetails Details)
        {
            DateTime date=new DateTime();
            string dt="1998-01-01 00:00:00.000";
            var tDetails = new LoginReg_tbl
            {
                EmailId = Details.EmailId,
                MobileNo = Details.MobileNo,
                UserType = "Costumer",
                Password = Details.Password,
                UserName=Details.FirstName+" "+Details.LastName,
                AlltemtTime=0,
                timer=0,
                time=Convert.ToDateTime(dt)
              
            };
            db.LoginReg_tbl.Add(tDetails);
            db.SaveChanges();
            var Tid = Details.Id;
            return Tid;
        }
        //public List<RegistrationDetails> GetLoginData(RegistrationDetails objuserlogin)
        //{
        //    try {
         
        //       var loginuser = db.LoginReg_tbl.AsEnumerable().Where(x =>
        //            x.Password == objuserlogin.Password && x.EmailId == objuserlogin.EmailId 
        //           ).FirstOrDefault();
                 
        //        var registration=db.Reg_tbl.AsEnumerable().Where(x =>
        //             x.Password == objuserlogin.Password && x.EmailId == objuserlogin.EmailId 
        //             ).FirstOrDefault();
        //        objuserlogin.FirstName = registration.FirstName;
        //        objuserlogin.LastName = registration.LastName;
                
               
        //    }
        //    catch (Exception ex)
        //    { }
        //    return new List<RegistrationDetails>();
            
        //}
        //Poonam ......................................................
        public List<RegistrationDetails> GetLoginData(RegistrationDetails objuserlogin)
        {
            try
            {

               
                var registration = db.LoginReg_tbl.AsEnumerable().Where(x =>
                     x.Password == objuserlogin.Password && x.EmailId == objuserlogin.EmailId || x.MobileNo == objuserlogin.EmailId).Select(
                    x => new RegistrationDetails
                    {
                        EmailId = x.EmailId,
                        MobileNo = x.MobileNo,
                        Password = x.Password,
                       FirstName=x.UserName,
                       attempttime=Convert.ToInt32(x.AlltemtTime),
                        countsec=Convert.ToInt32(x.timer),
                        datecount=x.time.ToString()

                      
                        //MobileNo=x.MobileNo
                    }).ToList();

                return registration;
            }
            catch (Exception ex)
            { }


            return new List<RegistrationDetails>();

        }
        public List<RegistrationDetails> GetregData(RegistrationDetails objuserlogin)
        {
            try
            {


                var registration = db.LoginReg_tbl.AsEnumerable().Where(x =>
                   x.EmailId == objuserlogin.EmailId || x.MobileNo == objuserlogin.EmailId).Select(
                    x => new RegistrationDetails
                    {
                        EmailId = x.EmailId,
                        MobileNo = x.MobileNo,
                        Password = x.Password,
                        FirstName = x.UserName,
                        attempttime = Convert.ToInt32(x.AlltemtTime),
                        countsec = Convert.ToInt32(x.timer),
                        datecount = x.time.ToString()


                        //MobileNo=x.MobileNo
                    }).ToList();

                return registration;
            }
            catch (Exception ex)
            { }


            return new List<RegistrationDetails>();

        }
        //14 march
        public string savePassword(LoginDetails details)
        {
            int marks = GetPasswordStrength(details.NewPassword);
            string status = "";
            switch (marks)
            {
                case 1:
                    status = "Very Week";
                    break;
                case 2:
                    status = "Week";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                default:
                    break;
            }
            if (marks >= 5)
            {
                var result = db.LoginReg_tbl.SingleOrDefault((x => x.Password == details.Password && x.EmailId == details.EmailId));
                if (result != null)
                {
                    result.Password = details.NewPassword;
                    db.SaveChanges();
                    return result.ToString();
                }
                var tid = details.UserId;
                return "";
            }
            return status;
        }
        //14 march
        private int GetPasswordStrength(string password)
        {
            int Marks = 0;
            // here we will check password strength
            if (password.Length < 6)
            {
                // Very Week
                return 1;
            }
            else
            {
                Marks = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                // 2    week
                Marks++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                // 3    medium
                Marks++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                //4     strong
                Marks++;
            }
            if (Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            {
                //5     very strong
                Marks++;
            }
            return Marks;

        }
        //ppppp End.........................
        public string attamttimeupdate(LoginDetails Details)
        {
            try
            {

                var result = db.LoginReg_tbl.SingleOrDefault(x => x.EmailId == Details.EmailId);
                int timeatt = Convert.ToInt32(result.AlltemtTime) + 1;
              //  result.AlltemtTime = timeatt;
                if (result != null)
                {
                    result.AlltemtTime = 0;
                        db.SaveChanges();
                        return result.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public string attamttime(LoginDetails Details)
        {
            try
            {

                var result = db.LoginReg_tbl.SingleOrDefault(x =>x.EmailId == Details.EmailId);
                int timeatt = Convert.ToInt32(result.AlltemtTime) + 1;
                result.AlltemtTime = timeatt;
                if (result != null)
                {
                    if (timeatt > 5)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        result.AlltemtTime = timeatt;
                        if (timeatt == 5)
                        {
                            result.timer = 30;
                            string dt = DateTime.Now.ToString();
                            result.time = Convert.ToDateTime(dt);
                        }
                        db.SaveChanges();
                        return result.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
             return "";
        }

    }

}
