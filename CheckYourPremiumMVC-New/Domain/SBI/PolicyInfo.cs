using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class PolicyInfo
    {
        public string startDate { get; set; }
        public string channelType { get; set; }
        public string ttlClaimedAmt { get; set; }
        public string numberPreviousClaims { get; set; }
        public string endDate { get; set; }
        public string previousPolicy { get; set; }
        public string policyDuration { get; set; }
    }
}
