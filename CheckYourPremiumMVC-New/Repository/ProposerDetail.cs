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
    
    public partial class ProposerDetail
    {
        public long ProposerId { get; set; }
        public string ProposerName { get; set; }
        public string proposerEmail { get; set; }
        public string proposerPhone { get; set; }
        public string proposerAddressOne { get; set; }
        public string proposerAddressTwo { get; set; }
        public Nullable<long> proposerAreaId { get; set; }
        public string gstTypeId { get; set; }
        public string gstIdNumber { get; set; }
        public string eiaNumber { get; set; }
        public Nullable<System.DateTime> proposerDob { get; set; }
        public string placeOfVisit { get; set; }
        public Nullable<int> travelPurposeId { get; set; }
        public string physicianName { get; set; }
        public string physicianContactNumber { get; set; }
        public long FkSearchId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> PlanId { get; set; }
        public Nullable<System.DateTime> travelStartOn { get; set; }
        public Nullable<System.DateTime> travelEndOn { get; set; }
        public string SearchType { get; set; }
    }
}
