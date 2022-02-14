using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
   public class GetICICIResponseDetails
    {
        public string FullName { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }
        //[DataType(DataType.Date)]
        //  [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //  public DateTime? DOB { get; set; }
        ////  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Required]
        public string DOB { get; set; }

        public string Earning { get; set; }
        public string MaritialStatus { get; set; }


        public string PolicyIndiaFirst { get; set; }
        public string SumAssured { get; set; }
        public int PolicyTerm { get; set; }
        public int PPT { get; set; }
        public string Frequency { get; set; }
        public string BHB_Ind { get; set; }
        public string TUB_Ind { get; set; }
        public string PWB_Ind { get; set; }
        public string ADB_Ind { get; set; }
        public string CI_Ind { get; set; }
        public string PD_Ind { get; set; }
        public string HCB_ind { get; set; }
        public string DSA_ind { get; set; }
        public int ADB { get; set; }
        public int ATPD { get; set; }
        public int CI { get; set; }
        public int HCB { get; set; }
        public string CIC_Ind { get; set; }
        public int CIC_SumAssured { get; set; }
        public string CIC_ClaimOption { get; set; }
        public int CIC_PolicyTerm { get; set; }
        public int TopupRate { get; set; }
        public int TotalBenefit { get; set; }
        public string PolicyOption { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }
        public string SpouseDOB { get; set; }
        public string SpouseGender { get; set; }
        public string SpouseAge { get; set; }
        public string SpouseTobbacoUser { get; set; }
        public string AdditionalBenefit { get; set; }
        public string TopUpBenefitPercentage { get; set; }
        public string PayoutOption { get; set; }
        public string PayoutMonths { get; set; }
        public string PayoutPercentageLumpsum { get; set; }
        public string PayoutPercentageLevelIncome { get; set; }
        public string PayoutPercentageIncreasingIncome { get; set; }
        public string TransID { get; set; }
        //.................Responce 
        public long SearchId { get; set; }
        public string ANNUAL_GROSS_PREMIUM { get; set; }
        public string INSTALL_PREMIUM_BEFORETAX { get; set; }
        public string INSTALL_PREMIUM_TAXINCLUSIVE { get; set; }
        public string INSTALL_PREMIUM_TAXINCLUSIVE1 { get; set; }
        public string SUM_ASSURED { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Smoke { get; set; }

        public string Income { get; set; }


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
        public string City { get; set; }
        public string State { get; set; }
        public string hdnZindagiPlusdata { get; set; }
        // For ICICI

        public string father_Name { get; set; }
        public string Mother_name { get; set; }
        public string spouse_name { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public string NameofOrg { get; set; }
        public string Url { get; set; }
        public string TransactionId { get; set; }
        public string responseRemarks { get; set; }
        
    }
}
