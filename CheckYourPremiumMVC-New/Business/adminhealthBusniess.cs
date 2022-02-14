using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{

    public interface IadminhealthBusniess
    {

        List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth);
        List<HealthPlanDetails> GetSearchList(HealthPlanDetails objuserlhealth);
    }
    public class adminhealthBusniess : IadminhealthBusniess
    {
        private readonly CheckyourpremiumliveEntities db;

        public adminhealthBusniess()
        {

            db = new CheckyourpremiumliveEntities();

        }

        public List<HealthPlanDetails> GetHealthList(HealthPlanDetails objuserlhealth)
        {
            try
            {

                var Policy = db.Health_Insurance_Record_Saved.Select(x => new HealthPlanDetails()
                {
                    Gender = x.Gender,
                    Self = x.Self,
                    Spouses = x.Adult,
                    son = x.Child,
                    City = x.City,
                    Full_Name = x.Name,
                    MobileNo = x.MobileNo,
                    // income = x.Income,
                    // PremiuminServiceTax = x.ServiceTax
                }).ToList().OrderByDescending(p => p.Full_Name).ToList();
                return Policy;

            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }


            return new List<HealthPlanDetails>();

        }

        public List<HealthPlanDetails> GetSearchList(HealthPlanDetails objuserlhealth)
        {
            try
            {

                var Policy = db.Health_Insurance_Record_Saved.Where(x => x.Name == objuserlhealth.Full_Name).Select(x => new HealthPlanDetails()
                {
                    Gender = x.Gender,
                    Self = x.Self,
                    Spouses = x.Adult,
                    son = x.Child,
                    City = x.City,
                    Full_Name = x.Name,
                    MobileNo = x.MobileNo,
                    // income = x.Income,
                    // PremiuminServiceTax = x.ServiceTax
                }).ToList().OrderByDescending(p => p.Full_Name).ToList();
                return Policy;

            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }


            return new List<HealthPlanDetails>();

        }
    }
}
