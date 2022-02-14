using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
    public class BajajHealthDetailsData : Bajaj_Proposal_Creation
    {
        public int sumInsuredId { get; set; }
        public int schemeId { get; set; }
        public string sumAssured { get; set; }
        public string Amount { get; set; }
        public string serviceTax { get; set; }
        public string totalPremium { get; set; }
        public string NoOfAdults { get; set; }
        public string NoOfChildren { get; set; }
        public string Plan { get; set; }
        [Required]
        public string startOn { get; set; }
        [Required]
        public string endOn { get; set; }
        public string policyCategory { get; set; }
        [Required]
        public string proposerName { get; set; }
        [Required]
        public string proposerEmail { get; set; }
        [Required]
        public string proposerPhone { get; set; }
        [Required]
        public string proposerAddressOne { get; set; }
        [Required]
        public string proposerAddressTwo { get; set; }

        public string proposerAreaId { get; set; }
        [Required]
        public string proposerAreaPinCode { get; set; }
        [Required]
        public string proposerAreaCityId { get; set; }
        public string proposerResidenceAddressOne { get; set; }
        public string proposerResidenceAddressTwo { get; set; }
        public string proposerResidenceAreaId { get; set; }

        public string proposerResidenceAreaPinCode { get; set; }
        public string proposerResidenceAreaCityId { get; set; }
        [Required]
        public string proposerDob { get; set; }
        public string panNumber { get; set; }
        //public string period { get; set; }
        public string gstTypeId { get; set; }
        public string gstIdNumber { get; set; }
        public string aadharNumber { get; set; }
        public string eiaNumber { get; set; }
        public string socialStatus { get; set; }
        public string socialStatusBpl { get; set; }
        public string socialStatusDisabled { get; set; }
        public string socialStatusInformal { get; set; }
        public string socialStatusUnorganized { get; set; }
        public string previousMedicalInsurance { get; set; }
        [Required]
        public string annualIncome { get; set; }
        [Required]
        public string criticalIllness { get; set; }
        public BajajInsured insureds0 { get; set; }
        public BajajInsured insureds1 { get; set; }
        public BajajInsured insureds2 { get; set; }
        public BajajInsured insureds3 { get; set; }
        [Required]
        public string nomineeName { get; set; }
        [Required]
        public string nomineeAge { get; set; }
        [Required]
        public string nomineeRelationship { get; set; }
        [Required]
        public string nomineePercentClaim { get; set; }
        public string appointeeName { get; set; }
        public string appointeeAge { get; set; }
        public string appointeeRelationship { get; set; }
        public string nomineeNameTwo { get; set; }
        public string nomineeAgeTwo { get; set; }
        public string nomineeRelationshipTwo { get; set; }
        public string nomineePercentClaimTwo { get; set; }
        public string appointeeNameTwo { get; set; }
        public string appointeeAgeTwo { get; set; }

        public string appointeeRelationshipTwo { get; set; }
    }
}
