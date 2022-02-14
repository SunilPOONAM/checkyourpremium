using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class QuotationDetail
    {
       public string QuoteNo { get; set; }
       public string OrderNo { get; set; }
       public string Channel { get; set; }
       public string Product { get; set; }
       public string IsMobile { get; set; }
       public string sumAssured { get; set; }
       public string Amount { get; set; }
       public int serviceTax { get; set; }
       public int totalPremium { get; set; }
       public string NoOfAdults { get; set; }
       public string NoOfChildren { get; set; }
       public string Plan{ get; set; }
       public string PlanId { get; set; }
       public string searchId { get; set; }
       public ProposerDetail proposerInfo { get; set; }
       public TravellerDetails InsuredPersonInfo { get; set; }
    }
}
