using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class QuoteDetails
    {
        public PolicyInfo policyInfo { get; set; }
        public string locationAddressDlt { get; set; }
        public string insuredId { get; set; }
        public string quoteNo { get; set; }
        public string expiryDate { get; set; }
        public string policyID { get; set; }
        public string effectiveDate { get; set; }
        public string ppId { get; set; }
        public string agreementCode { get; set; }
    }

}
