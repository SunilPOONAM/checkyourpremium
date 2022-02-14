using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public  class View_TravelInsuranceModel
    {
        public string CompanyName { get; set; }
        public long Travel_id { get; set; }
        public Nullable<int> Age_From { get; set; }
        public Nullable<int> Age_To { get; set; }
        public string Plans { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> SumInsured { get; set; }
        public string CoverType { get; set; }
        public string TripType { get; set; }
        public Nullable<int> No_Of_Memebers { get; set; }
        public Nullable<int> No_Of_Child { get; set; }
        public Nullable<int> TermFrom { get; set; }
        public Nullable<int> TermTo { get; set; }
        public Nullable<decimal> Premium { get; set; }
        public Nullable<long> Plan_ID { get; set; }
        public Nullable<long> PCode { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string Destination { get; set; }
        public Nullable<long> destinationId { get; set; }
        public string LogoImage { get; set; }
        public string ageSelf { get; set; }
        public string stayDays { get; set; }
        public string destination { get; set; }
        public string tripType { get; set; }
        public string travellerName { get; set; }
       
        public string City { get; set; }
      
        public string Phone { get; set; }
      
        public string Email { get; set; }

        public string tripStartDate { get; set; }

        public string tripEndDate { get; set; }
        public long SearchId { get; set; }
       
        public string ageSpouse { get; set; }
        public string ageChild1 { get; set; }
        public string ageChild2 { get; set; }

        public string ageBrother { get; set; }
        public string ageMother { get; set; }
        public string ageFather { get; set; }
         public string policyPremiumd { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string ugst { get; set; }
        public string igst { get; set; }
        public string invoiceNumber { get; set; }
        
        public string code { get; set; }
         public string message { get; set; }
         public string paxId { get; set; }
        
         public string policyID { get; set; }
         public string transactionDate { get; set; }
         public string bookingId { get; set; }
         public string policyStatus { get; set; }
         public string applicationId { get; set; }
         public string paymentLink { get; set; }
         public string pakegecode { get; set; }
         public string pakageName { get; set; }
         public string Orderno { get; set; }
         public string QuatoNo { get; set; }
         
    }
}
