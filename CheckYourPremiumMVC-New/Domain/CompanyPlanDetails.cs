using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class CompanyPlanDetails
    {
        public int Id { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string PlanInfo { get; set; }
        public string BuyNow { get; set; }
        public string PolicyXURL { get; set; }
        public string Status { get; set; }
        public string PolicyWording { get; set; }
       public string PolicyBoucher { get; set; }
    }
}
