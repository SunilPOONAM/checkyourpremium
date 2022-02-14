using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class SBIHealthRequest
    {
        public AgentReceiptDetailsRequestHeader AgentReceiptDetailsRequestHeader { get; set; }
        public AgentReceiptDetailsRequestBody AgentReceiptDetailsRequestBody { get; set; }
    }
    public class AgentReceiptDetailsRequestHeader
    {
        public string requestID { get; set; }
        public string action { get; set; }
        public string channel { get; set; }
        public string state { get; set; }
        public string transactionTimestamp { get; set; }
    }

    public class Payload
    {
        public string EffectiveDate { get; set; }
        public string con { get; set; }
        public string currencyType { get; set; }
        public string instumentTotal { get; set; }
        public string payor { get; set; }
        public string payorCustCodeArr { get; set; }
        public string payorType { get; set; }
        public string quoteNo { get; set; }
        public string receiptDate { get; set; }
        public string receipting1 { get; set; }
        public string receiveAmountArr { get; set; }
        public string bankAccount { get; set; }
        public string totalPremium { get; set; }
        public string transactionID { get; set; }
    }

    public class AgentReceiptDetailsRequestBody
    {
        public Payload payload { get; set; }
    }

}
