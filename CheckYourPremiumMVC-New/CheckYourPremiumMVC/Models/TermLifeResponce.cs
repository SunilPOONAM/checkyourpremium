using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckYourPremiumMVC.Models
{
    public class TermLifeResponce
    {
        public string pdfName { get; set; }
        public string premium { get; set; }
        public string totalPremium { get; set; }
        public string accidentalDeathPremium { get; set; }
        public string permanentDisabilityPremium { get; set; }
        public string criticalIllnessPremium { get; set; }
        public string premiumWaiver { get; set; }
        public string betterHalfPremium { get; set; }
        public string basePremium { get; set; }
        public string hcbPremium { get; set; }
        public string error { get; set; }
        public string Company { get; set; }
        public string sumAssured { get; set; }
        public string Action { get; set; }
        public string CustID { get; set; }
    }
}