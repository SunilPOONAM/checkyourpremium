using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
    public class GetQuotationDetail
    {
        public string QuoteNo { get; set; }
        public string OrderNo { get; set; }
        public string Channel { get; set; }
        public string Product { get; set; }
        public string IsMobile { get; set; }
        public string sumAssured { get; set; }
        public string Amount { get; set; }
        public string serviceTax { get; set; }
        public string totalPremium { get; set; }
        public string NoOfAdults { get; set; }
        public string NoOfChildren { get; set; }
        public string Plan { get; set; }
        public string PlanId { get; set; }
        public string searchId { get; set; }
        public long searchId1 { get; set; }
        //---------Proposar detail
        [Required]
        public string proposerName { get; set; }
        [Required(ErrorMessage = "Please Fill  Traveler DOB")]

        public string proposerdOB { get; set; }
        [Required(ErrorMessage = "Please Fill Mobile No")]
        public string proposerage { get; set; }
        [Required(ErrorMessage = "Please Select Gender")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Please Fill  Email")]
        public string pgender { get; set; }
        [Required(ErrorMessage = "Please Fill Your PINCODE")]
        public string email { get; set; }
        [Required]
        public string pinCode { get; set; }
        [Required]

        public string city { get; set; }
        [Required]
        public string address { get; set; }
        public string address2 { get; set; }
        [Required]
        public string landmark { get; set; }
        [Required]
        public string alternateNo { get; set; }
        [Required]
        public string placeOfVisit { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool travelDeclaration { get; set; }
        [Required]
        public string travelPurposeId { get; set; }
        [Required]
        public string physicianName { get; set; }
        [Required]
        public string physicianContactNumber { get; set; }
        [Required]
        public string travelstartdate { get; set; }
        [Required]
        public string travelenddate { get; set; }
        [Required]
        public string gstTypeId { get; set; }
        [Required]
        public string gstIdNumber { get; set; }
        public string eiaNumber { get; set; }
        //------------ Insurer Detail
        [Required(ErrorMessage = "Please Fill  Person  Name")]
        public string insuredPersonName { get; set; }
        [Required(ErrorMessage = "Please Fill  DOB")]
        public string dOB { get; set; }
        [Required]
        public string age { get; set; }
        public string maritalStatus { get; set; }
        [Required(ErrorMessage = "Please Select  Gender")]
        public string gender { get; set; }
        [Required(ErrorMessage = "Please Select  Passport Type")]
        public string IsIndianPassport { get; set; }
        [Required(ErrorMessage = "Please Select  Passport Number")]
        public string passportNo { get; set; }
        public string NomineeName { get; set; }
        public string Relation { get; set; }
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "Please Fill Passport Expiry Date")]
        public string PassportExpiry { get; set; }
        [Required(ErrorMessage = "Please Select None")]
        public string Illness { get; set; }
        [Required(ErrorMessage = "Please Select Visa Type")]
        public string visaType { get; set; }
        [Required(ErrorMessage = "Please Fill Assignee Name")]
        public string assigneeName { get; set; }
        [Required(ErrorMessage = "Please Fill AssigneeRelation Id")]
        public string assigneeRelationshipId { get; set; }

        public string prefix { get; set; }
        public string occupation { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string insuredPrefix { get; set; }
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Index { get; set; }
        public string UserName { get; set; }

        public string Route { get; set; }
        public string Contract { get; set; }
        public string proposerAreaId { get; set; }
        public string proposerAreaCityId { get; set; }
        public string proposerResidenceAreaCityId { get; set; }
        public string bookingId { get; set; }
        public string policyPremium { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string ugst { get; set; }
        public string igst { get; set; }

        public string policyNetPremium { get; set; }

        public string invoiceNumber { get; set; }

        public string code { get; set; }
        public string message { get; set; }
        public string paxId { get; set; }

        public string policyID { get; set; }
        public string transactionDate { get; set; }

        public string policyStatus { get; set; }
        public string applicationId { get; set; }
        public string paymentLink { get; set; }

        public string policyPremiumd { get; set; }
        public string LogoImage { get; set; }

        public string Plans { get; set; }

        public int CompanyId { get; set; }
        public int Travel_id { get; set; }

        public string pakegecode { get; set; }
        public string paymentid { get; set; }
        public string Godigitgender { get; set; }
        public string GodigitPgender { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public string proposerResidenceAreaId { get; set; }
        public string staydays { get; set; }

        //  public insuredperson insured0 { get; set; }
        public string insured0insuredPersonName { get; set; }
        public string insured0dOB { get; set; }
        public string age1 { get; set; }
        public string age2 { get; set; }
        public string age3 { get; set; }
        public string insured0age { get; set; }
        public string insured0maritalStatus { get; set; }
        public string insured0gender { get; set; }
        public string insured0IsIndianPassport { get; set; }
        public string insured0passportNo { get; set; }
        public string insured0NomineeName { get; set; }
        public string insured0Relation { get; set; }
        public string insured0MaritalStatus { get; set; }
        public string insured0PassportExpiry { get; set; }
        public string insured0Illness { get; set; }
        public string insured0visaType { get; set; }
        public string insured0assigneeName { get; set; }
        public string insured0assigneeRelationshipId { get; set; }
        public string insured0travelPurposeId { get; set; }
        //   public insuredperson insured1 { get; set; }
        public string insured1insuredPersonName { get; set; }
        public string insured1dOB { get; set; }
        public string insured1age { get; set; }
        public string insured1maritalStatus { get; set; }
        public string insured1gender { get; set; }
        public string insured1IsIndianPassport { get; set; }
        public string insured1passportNo { get; set; }
        public string insured1NomineeName { get; set; }
        public string insured1Relation { get; set; }
        public string insured1MaritalStatus { get; set; }
        public string insured1PassportExpiry { get; set; }
        public string insured1Illness { get; set; }
        public string insured1visaType { get; set; }
        public string insured1assigneeName { get; set; }
        public string insured1assigneeRelationshipId { get; set; }
        public string insured1travelPurposeId { get; set; }

        // public insuredperson insured2 { get; set; }
        public string insured2insuredPersonName { get; set; }
        public string insured2dOB { get; set; }
        public string insured2age { get; set; }
        public string insured2maritalStatus { get; set; }
        public string insured2gender { get; set; }
        public string insured2IsIndianPassport { get; set; }
        public string insured2passportNo { get; set; }
        public string insured2NomineeName { get; set; }
        public string insured2Relation { get; set; }
        public string insured2MaritalStatus { get; set; }
        public string insured2PassportExpiry { get; set; }
        public string insured2Illness { get; set; }
        public string insured2visaType { get; set; }
        public string insured2assigneeName { get; set; }
        public string insured2assigneeRelationshipId { get; set; }
        public string insured2travelPurposeId { get; set; }

    }

    //public class insuredperson
    //{
    //    public string insured0insuredPersonName { get; set; }
    //    public string insured0dOB { get; set; }
    //    public string insured0age { get; set; }
    //    public string insured0maritalStatus { get; set; }
    //    public string insured0gender { get; set; }
    //    public string insured0IsIndianPassport { get; set; }
    //    public string insured0passportNo { get; set; }
    //    public string insured0NomineeName { get; set; }
    //    public string insured0Relation { get; set; }
    //    public string insured0MaritalStatus { get; set; }
    //    public string insured0PassportExpiry { get; set; }
    //    public string insured0Illness { get; set; }
    //    public string insured0visaType { get; set; }
    //    public string insured0assigneeName { get; set; }
    //    public string insured0assigneeRelationshipId { get; set; }

    //}
}
