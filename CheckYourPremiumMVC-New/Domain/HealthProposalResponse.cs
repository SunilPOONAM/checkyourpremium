using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class HealthProposalResponse
    {
    
       public string referenceId { get; set; }
       public int premium { get; set; }
       public int serviceTax { get; set; }
       public int totalPremium { get; set; }
   }
}
