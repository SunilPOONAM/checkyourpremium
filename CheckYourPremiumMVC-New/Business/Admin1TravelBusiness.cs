using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public interface IAdmin1TravelBusiness
    {

        List<SearchTravelInsurance> GetTravelList(SearchTravelInsurance objusertravel);
    }
    public class Admin1TravelBusiness : IAdmin1TravelBusiness
    {
        private readonly CheckyourpremiumliveEntities db;

        public Admin1TravelBusiness()
        {

            db = new CheckyourpremiumliveEntities();

        }
        public List<SearchTravelInsurance> GetTravelList(SearchTravelInsurance objusertravel)
        {
            try
            {
                // List<Reg_tbl> dt = db.Reg_tbl.ToList();

                var data = db.TravelInsuranceSearchBies.AsEnumerable().Select(
                     x => new SearchTravelInsurance
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

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return new List<SearchTravelInsurance>();

        }

    }
}
