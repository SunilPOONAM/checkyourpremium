//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Godigit_Responce_Data
    {
        public int Id { get; set; }
        public string BookingID { get; set; }
        public Nullable<int> paxId { get; set; }
        public Nullable<decimal> policyPremium { get; set; }
        public Nullable<decimal> Sgst { get; set; }
        public Nullable<decimal> Cgst { get; set; }
        public Nullable<decimal> Ugst { get; set; }
        public Nullable<decimal> igst { get; set; }
        public string invoiceNumber { get; set; }
        public Nullable<decimal> policyNetPremium { get; set; }
        public string policyID { get; set; }
        public string transactionDate { get; set; }
        public Nullable<int> code { get; set; }
        public string message { get; set; }
        public string policyStatus { get; set; }
        public string applicationId { get; set; }
        public string paymentLink { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public string PlanName { get; set; }
        public string Adult { get; set; }
        public string Child { get; set; }
    }
}