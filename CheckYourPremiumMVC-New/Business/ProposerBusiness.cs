using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Business
{


    public class ProposerBusiness : ICommonBusiness
    {
        private readonly CheckyourpremiumliveEntities db;
        private RequestHandler requestHandler = new RequestHandler();
        public ProposerBusiness()
        {
            db = new CheckyourpremiumliveEntities();
        }
        public Int64 SaveInfo(GetQuotationDetail getQuotationDetail)
        {
            try
            {
                var proposerInfo = new Repository.ProposerDetail
                {
                    ProposerName = getQuotationDetail.proposerName,
                    proposerDob = DateTime.ParseExact(getQuotationDetail.proposerdOB, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                    travelStartOn = DateTime.ParseExact(getQuotationDetail.travelstartdate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    travelPurposeId = Convert.ToInt32(getQuotationDetail.travelPurposeId),
                    travelEndOn = DateTime.ParseExact(getQuotationDetail.travelenddate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    proposerPhone = getQuotationDetail.mobile,
                    proposerEmail = getQuotationDetail.NomineeName,
                    proposerAreaId = 5,
                    proposerAddressOne = getQuotationDetail.address,
                    PlanId = Convert.ToInt64(getQuotationDetail.PlanId),
                    placeOfVisit = getQuotationDetail.placeOfVisit,

                    physicianName = getQuotationDetail.physicianName,
                    physicianContactNumber = getQuotationDetail.physicianContactNumber,
                    gstTypeId = getQuotationDetail.gstTypeId,
                    gstIdNumber = getQuotationDetail.gstIdNumber,
                    eiaNumber = getQuotationDetail.eiaNumber,
                    SearchType="Travel"

                };
                db.ProposerDetails.Add(proposerInfo);
                db.SaveChanges();
                return proposerInfo.ProposerId;

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
                var proposerInfo = new Repository.ProposerDetail
                {
                    ProposerName = getQuotationDetail.proposerName,
                    proposerDob = DateTime.ParseExact(getQuotationDetail.proposerDob, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FkSearchId = Convert.ToInt64(getQuotationDetail.searchId),
                    travelStartOn = DateTime.ParseExact(getQuotationDetail.startOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
               //     travelPurposeId = Convert.ToInt32(getQuotationDetail.travelPurposeId),
                    travelEndOn = DateTime.ParseExact(getQuotationDetail.endOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    proposerPhone = getQuotationDetail.proposerPhone,
                    proposerEmail = getQuotationDetail.proposerEmail,
                    proposerAreaId = 5,
                    proposerAddressOne = getQuotationDetail.proposerAddressOne,
                 //   PlanId = Convert.ToInt64(getQuotationDetail.PlanId),
                  //  placeOfVisit = getQuotationDetail.placeOfVisit,

                  ///  physicianName = getQuotationDetail.physicianName,
                //    physicianContactNumber = getQuotationDetail.physicianContactNumber,
                    gstTypeId = getQuotationDetail.gstTypeId,
                    gstIdNumber = getQuotationDetail.gstIdNumber,
                    eiaNumber = getQuotationDetail.eiaNumber,
                    SearchType="Health"

                };
                db.ProposerDetails.Add(proposerInfo);
                db.SaveChanges();
                return proposerInfo.ProposerId;

            }
            catch (Exception ex)
            {


            }

            return 0;
        }

    }
}
