using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
  public  class ICICIResponceDetails
    {
        public string responseCode { get; set; }
        public string BREDecision { get; set; }
        public string responseRemarks { get; set; }
        public string modalPremium { get; set; }
        public string BREAction { get; set; }
        public string baseCounterOffer { get; set; }
        public string lifeOption { get; set; }
        public string transID { get; set; }
        public string annualPremiumWithTax { get; set; }
        public string ciCounterOffer { get; set; }
        public string URL { get; set; }
        public string adbrCounterOffer { get; set; }
        public string isMedical { get; set; }
    }
}
