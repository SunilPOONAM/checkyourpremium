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
    
    public partial class PolicyPlan
    {
        public long PlanID { get; set; }
        public string ProductCode { get; set; }
        public string PlanType { get; set; }
        public string PlanName { get; set; }
        public string PlanDesc { get; set; }
        public string PlanImage { get; set; }
        public string ST_Included { get; set; }
        public long CompanyID { get; set; }
    
        public virtual Company Company { get; set; }
    }
}
