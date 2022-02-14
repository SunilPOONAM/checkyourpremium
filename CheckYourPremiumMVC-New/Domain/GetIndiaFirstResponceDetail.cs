using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
 public   class GetIndiaFirstResponceDetail
    {
     public long SearchId { get; set; }
     public string ANNUAL_GROSS_PREMIUM { get; set; }
     public string INSTALL_PREMIUM_BEFORETAX { get; set; }
     public string INSTALL_PREMIUM_TAXINCLUSIVE { get; set; }
     public string INSTALL_PREMIUM_TAXINCLUSIVE1 { get; set; }
     public string SUM_ASSURED { get; set; }
     [Required]
     public string fullName { get; set; }
       [Required]
     public string Phone { get; set; }
       [Required]
     public string Email { get; set; }
       [Required]
     public string Smoke { get; set; }
       [Required]
     public string Age { get; set; }
       [Required]
     public string Income { get; set; }
       [Required]
     public string Gender { get; set; }
       [Required]
    public string DOB { get; set; }
     public string CustID { get; set; }
     //..Responce
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
     public string PlanName { get; set; }
     public string POLICY_TERM { get; set; }
     public string Logo { get; set; }
     public string Frequency { get; set; }
     public string Suminsured { get; set; }
     public string city { get; set; }
     public string Payout_option { get; set; }
     public string PemiumType { get; set; }
     public string LogoName { get; set; }
    }

}
