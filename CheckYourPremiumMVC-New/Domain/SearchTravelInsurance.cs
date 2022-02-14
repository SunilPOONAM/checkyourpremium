using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class SearchTravelInsurance
    {
        public string destination { get; set; }

        [Required]
        public string ageSelf { get; set; }
        public string ageSpouse{ get; set; }
        public string ageChild1 { get; set; }
        public string ageChild2 { get; set; }

        public string ageBrother{ get; set; }
        public string ageMother{ get; set; }
        public string ageFather{ get; set; }
          [Required]
        public string tripStartDate { get; set; }
          [Required]
        public string tripEndDate { get; set; }

        public string coverAmount { get; set; }
        public string stayDays{ get; set; }
          [Required]
        public string travellerName { get; set; }
          [Required]
        public string City { get; set; }
          [Required]
        public string Phone { get; set; }
          [Required]
        public string Email{ get; set; }
          public string tripType { get; set; }
          public string SumInsured { get; set; }
          public string PlanName { get; set; }
          public long SearchId { get; set; }

          public string ANNUAL_GROSS_PREMIUM { get; set; }
          public string INSTALL_PREMIUM_BEFORETAX { get; set; }
          public string INSTALL_PREMIUM_TAXINCLUSIVE { get; set; }
          public string INSTALL_PREMIUM_TAXINCLUSIVE1 { get; set; }
          public string SUM_ASSURED { get; set; }
          public string accidentalDeathPremium { get; set; }
          public string basePremium { get; set; }
          public string betterHalfPremium { get; set; }
          public string criticalIllnessPremium { get; set; }
          public string error { get; set; }
          public string hcbPremium { get; set; }
          public string pdfName { get; set; }
          public string permanentDisabilityPremium { get; set; }
          public string premium { get; set; }
          public string premiumWaiver { get; set; }
          public string sumAssured { get; set; }
          public string totalPremium { get; set; }
          public string Company { get; set; }

          public string POLICY_TERM { get; set; }
          public string Logo { get; set; }
          public string Frequency { get; set; }
          public string CustID { get; set; }
          public string Loading { get; set; }
    }
}
