using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class HealthQuatationDetail
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
        public string Dureation { get; set; }
        public string PlanID { get; set; }
        public HealthProposerDetail proposerInfo { get; set; }
        public HealthDetails InsuredPersonInfo { get; set; }
    }
}
