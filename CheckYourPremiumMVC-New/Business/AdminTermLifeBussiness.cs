using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public interface IAdminTermLifeBussiness
    {
        List<ZPModel> GetHealthPremiumList(ZPModel srchHealthIns);
        List<ZPModel> GetSearchList(ZPModel srchHealthIns);
    }
    public class AdminTermLifeBussiness : IAdminTermLifeBussiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private string baseUrl; string secretKey = string.Empty; string apiKey = string.Empty;
        private string bajajurl;
        string BajajUser = string.Empty;
        string BajajPassword = string.Empty;
        private RequestHandler requestHandler = new RequestHandler();
        public AdminTermLifeBussiness()
        {
            db = new CheckyourpremiumliveEntities();
        }
        public List<ZPModel> GetHealthPremiumList(ZPModel srchHealthlIns)
        {
            try
            {
                var Policy = db.tbl_GetPremiumData.AsEnumerable().Select(x => new ZPModel
                {
                    FullName = x.Name,
                    //  DOB = Convert.ToDateTime(x.DOB).ToString("dd-mm-yyyy"),
                    Phone = x.Mobile,
                    Email = x.Email,
                    // City = x.city,
                    Income = x.Income,
                    Gender = x.Gender,
                    //SumAssured = x.SumInsurred.ToString(),
                    POLICY_TERM = x.Term.ToString(),
                    // Frequency = x.frequency
                }).ToList();



                return Policy;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error(ex.Message.ToString(), "");
            }
            return new List<ZPModel>();

        }
        public List<ZPModel> GetSearchList(ZPModel srchHealthlIns)
        {
            try
            {
                var Policy = db.tbl_GetPremiumData.AsEnumerable().Where(x => x.Name == srchHealthlIns.FullName).Select(x => new ZPModel
                {
                    FullName = x.Name,
                    //  DOB = Convert.ToDateTime(x.DOB).ToString("dd-mm-yyyy"),
                    Phone = x.Mobile,
                    Email = x.Email,
                    // City = x.city,
                    Income = x.Income,
                    Gender = x.Gender,
                    //SumAssured = x.SumInsurred.ToString(),
                    POLICY_TERM = x.Term.ToString(),
                    // Frequency = x.frequency
                }).ToList();



                return Policy;
            }
            catch (Exception ex)
            {
                (new ErrorLog()).Error(ex.Message.ToString(), "");
            }
            return new List<ZPModel>();

        }

    }
}
