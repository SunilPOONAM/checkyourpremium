using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository;
using Domain;

namespace Business
{
    public interface ICustomerBusiness
    {
        List<ZPModel> GetSearchList(ZPModel srchHealthlIns);
        List<HealthPlanDetails> GetSearchListHealth(HealthPlanDetails objuserlhealth);
        List<SearchTravelInsurance> GetTravelList(SearchTravelInsurance objusertravel);
        List<BuyInfoDetails> GetMotorList(BuyInfoDetails objusertravel);
    }
    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        public CustomerBusiness()
        {

            db = new CheckyourpremiumliveEntities();

        }
        public List<ZPModel> GetSearchList(ZPModel srchHealthlIns)
        {
            try
            {

                var d = db.tbl_GetPremiumData.Join(db.tbl_GetPremiumResponce, s => s.ID, sa => sa.CustID,
     (s, sa) => new { service = s, asgnmt = sa })
.Where(ssa => ssa.service.Mobile == srchHealthlIns.Phone && ssa.asgnmt.premiumWaiver=="true")
.Select(s => new ZPModel
{
    FullName = s.service.Name,
    Phone = s.service.Mobile,
    Email = s.service.Email,
    totalPremium = s.asgnmt.totalPremium,
    Gender = s.service.Gender,
   POLICY_TERM = s.service.Term.ToString(),
   DSA_ind=s.service.createDate.ToString(),
  
}).ToList();
                //var Policy = db.tbl_GetPremiumData.AsEnumerable().Where(x => x.Mobile == srchHealthlIns.Phone).Select(x => new ZPModel
                // {
                //     FullName = x.Name,
                //     //  DOB = Convert.ToDateTime(x.DOB).ToString("dd-mm-yyyy"),
                //     Phone = x.Mobile,
                //     Email = x.Email,
                //     // City = x.city,
                //     Income = x.Income,
                //     Gender = x.Gender,
                //     //SumAssured = x.SumInsurred.ToString(),
                //     POLICY_TERM = x.Term.ToString(),
                //     // Frequency = x.frequency
                // }).ToList();
                return d;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error(ex.Message.ToString(), "");
            }
            return new List<ZPModel>();

        }
        // For Health
        public List<HealthPlanDetails> GetSearchListHealth(HealthPlanDetails objuserlhealth)
        {
            try
            {
                var d = db.Health_Insurance_Record_Saved.Join(db.TravelProposalDetails, s => s.tid, sa => sa.FkSearchId,
   (s, sa) => new { service = s, asgnmt = sa })
.Where(ssa => ssa.service.MobileNo == objuserlhealth.MobileNo && ssa.asgnmt.RefType == "Health")
.Select(s => new HealthPlanDetails
{
    Full_Name = s.service.Name,
    MobileNo = s.service.MobileNo,
    City = s.service.City,
    Gender = s.service.Gender,
    income = s.asgnmt.ReferenceId,
    PremiuminServiceTax = s.asgnmt.IsBillPayed.ToString(),
    CoverForYear=s.asgnmt.BilledDate.ToString()
   
}).ToList().OrderByDescending(p => p.Full_Name).ToList();

                //var Policy = db.Health_Insurance_Record_Saved.Where(x => x.MobileNo == objuserlhealth.MobileNo).Select(x => new HealthPlanDetails()
                //{
                //    Gender = x.Gender,
                //    Self = x.Self,
                //    Spouses = x.Adult,
                //    son = x.Child,
                //    City = x.City,
                //    Full_Name = x.Name,
                //    MobileNo = x.MobileNo,
                //    // income = x.Income,
                //    // PremiuminServiceTax = x.ServiceTax
                //}).ToList().OrderByDescending(p => p.Full_Name).ToList();
                return d;

            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }


            return new List<HealthPlanDetails>();

        }
        public List<SearchTravelInsurance> GetTravelList(SearchTravelInsurance objusertravel)
        {
            try
            {
                // List<Reg_tbl> dt = db.Reg_tbl.ToList();
                var Policy = db.TravelInsuranceSearchBies.AsEnumerable().Where(x => x.contactno == objusertravel.Phone).Select(x => new SearchTravelInsurance

                {
                    travellerName = x.TravellerName,
                    ageSelf = x.age.ToString(),
                    City = x.city,
                    Phone = x.contactno,
                    Email = x.emailid,
                    destination = x.destination,
                    tripStartDate = Convert.ToDateTime(x.startDate).ToString("dd-MM-yyyy"),
                    tripEndDate = Convert.ToDateTime(x.endDate).ToString("dd-MM-yyyy"),
                    ageSpouse = x.ageSpouse.ToString(),
                    ageChild1 = x.ageChild1.ToString(),
                    ageChild2 = x.ageChild2.ToString(),
                    ageBrother = x.ageBrother.ToString(),
                    ageMother = x.ageMother.ToString(),
                    ageFather = x.ageFather.ToString()




                }).ToList();

                return Policy;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return new List<SearchTravelInsurance>();

        }
        public List<BuyInfoDetails> GetMotorList(BuyInfoDetails objusertravel)
        {
            try
            {
                // List<Reg_tbl> dt = db.Reg_tbl.ToList();
                var Policy = db.tbl_BuyInfoData.AsEnumerable().Where(x => x.Phone == objusertravel.Phone).Select(x => new BuyInfoDetails

                {
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Status = x.Status,
                   

                }).ToList();

                return Policy;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return new List<BuyInfoDetails>();

        }
    }
}
