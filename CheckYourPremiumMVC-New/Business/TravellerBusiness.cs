using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Repository;
using System.Globalization;
namespace Business
{
    public interface ICommonBusiness
    {
        Int64 SaveInfo(RegistrationDetails getQuotationDetail);
        Int64 SaveInfo(GetQuotationDetail getQuotationDetail);

    }
    public class TravellerBusiness : ICommonBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private RequestHandler requestHandler = new RequestHandler();
        public TravellerBusiness()
        {
            db = new CheckyourpremiumliveEntities();
        }
        public Int64 SaveInfo(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                var travellerInfo = new TravellerDetail
                {
                    TravellerName = getQuotationDetail.insuredPersonName,
                    DOB = DateTime.ParseExact(getQuotationDetail.dOB, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                    Gender = getQuotationDetail.gender,
                    Illness = getQuotationDetail.Illness,
                    IsPassport = getQuotationDetail.IsIndianPassport == "Yes" ? true : false,
                    //MaritalStatus = getQuotationDetail.maritalStatus,
                    NomineeName = getQuotationDetail.NomineeName,
                    PassportExpiry = DateTime.ParseExact(getQuotationDetail.PassportExpiry, "dd/MM/yyyy", CultureInfo.InvariantCulture),

                    PassportNo = getQuotationDetail.passportNo,

                    Relation = getQuotationDetail.Relation,
                    visaType = getQuotationDetail.visaType,
                    searchType = "Travel"

                };
                db.TravellerDetails.Add(travellerInfo);
                db.SaveChanges();
                return travellerInfo.TravellerId;

            }
            catch (Exception ex)
            {


            }

            return 0;
        }
        public Int64 SaveInfo(RegistrationDetails getQuotationDetail)
        {
            try
            {
                var travellerInfo = new TravellerDetail
                {
                    TravellerName = getQuotationDetail.insureds0.name,
                    DOB = DateTime.ParseExact(getQuotationDetail.insureds0.dob, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                    Gender = getQuotationDetail.insureds0.sex,
                    Illness = getQuotationDetail.insureds0.illness,
                    NomineeName = getQuotationDetail.nomineeName,
                    Relation = getQuotationDetail.nomineeRelationship,
                    searchType="Health"
                    


                };
                db.TravellerDetails.Add(travellerInfo);
                db.SaveChanges();
                return travellerInfo.TravellerId;

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

    }
}
