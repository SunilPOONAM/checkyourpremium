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
    
    public partial class Compare_TermLife
    {
        public int Id { get; set; }
        public string Company_Id { get; set; }
        public string Company { get; set; }
        public string Plan_Name { get; set; }
        public string Minimum_Entry_Age { get; set; }
        public string Maximum_Entry_Age { get; set; }
        public string Cover_Upto { get; set; }
        public string Premium_Payment_Option { get; set; }
        public string Payment_Payment_Mode { get; set; }
        public string Minimum_Sum_Assured { get; set; }
        public string Medical_Test_Required { get; set; }
    }
}