using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class GodigitDetails
    {
       public string bookingId { get; set; }
        public string policyPremium { get; set; }
        public string sgst { get; set; }
        public string cgst { get; set; }
        public string ugst { get; set; }
        public string igst { get; set; }
       
        public string policyNetPremium { get; set; }
       
        public string invoiceNumber { get; set; }
        
        public string code { get; set; }
         public string message { get; set; }
         public string paxId { get; set; }
        
         public string policyID { get; set; }
         public string transactionDate { get; set; }
        
         public string policyStatus { get; set; }
         public string applicationId { get; set; }
         public string paymentLink { get; set; }
         public string schedulePath { get; set; }
    }
   //public class PaymentDetails
   //{
   //    public string policyPremium { get; set; }
   //    public string sgst { get; set; }
   //    public string cgst { get; set; }
   //    public string ugst { get; set; }
   //    public string igst { get; set; }
   //    public string invoiceNumber { get; set; }
   //    public string policyNetPremium { get; set; }
   //}

   //public class Status
   //{
   //    public string code { get; set; }
   //    public string message { get; set; }
   //}

   //public class InsuredPerson1
   //{
   //    public string paxId { get; set; }
   //    public PaymentDetails paymentDetails { get; set; }
   //    public string policyID { get; set; }
   //    public string transactionDate { get; set; }
   //    public Status status { get; set; }
   //    public string policyStatus { get; set; }
   //    public string applicationId { get; set; }
   //    public string paymentLink { get; set; }
   //}

   //public class Status2
   //{
   //    public string code { get; set; }
   //    public string message { get; set; }
   //}

   //public class RootObject
   //{
   //    public string bookingId { get; set; }
   //    public List<InsuredPerson1> insuredPersons { get; set; }
   //    public Status2 status { get; set; }
   //}
  
}
