using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class GodigitResponceDetails
    {
        public string applicationNumber { get; set; }
        public string applicationId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNumber { get; set; }
        public string email { get; set; }
        public string transactionNumber { get; set; }
        public string dateOfTransaction { get; set; }
        public string premiumAmount { get; set; }
        public string productCode { get; set; }
        public string link { get; set; }
        public int status { get; set; }
        public string mode { get; set; }
        public long createdDate { get; set; }
        public long lastModifiedDate { get; set; }
        public string successReturnUrl { get; set; }
        public string cancelReturnUrl { get; set; }
        public string merchantTxnId { get; set; }
        public string serviceUser { get; set; }
        public string policyStartDate { get; set; }
        public string policyEndDate { get; set; }
        public int expiryHours { get; set; }
        public string gatewayName { get; set; }
        public int gatewayIdentifier { get; set; }
        public string jusPayRespParam { get; set; }
        public string jusPayStatus { get; set; }
        public int serviceStatusCode { get; set; }
    }
}
