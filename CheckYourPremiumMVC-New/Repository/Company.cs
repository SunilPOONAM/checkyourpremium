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
    
    public partial class Company
    {
        public Company()
        {
            this.PolicyPlans = new HashSet<PolicyPlan>();
        }
    
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string LogoImage { get; set; }
    
        public virtual ICollection<PolicyPlan> PolicyPlans { get; set; }
    }
}