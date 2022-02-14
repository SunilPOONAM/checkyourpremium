using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckYourPremiumMVC.Models
{
    public class ShowHealthInsurenceQuate
    {

        public string Name { get; set; }
        public string Gender { get; set; }
        public string Logo { get; set; }
            public string PremiumDesc { get; set; }
            public string SumAssured { get; set; }
            public string AgeFrom { get; set; }
            public string AgeFromDuration { get; set; }
            public string AgeTo { get; set; }
            public string AgeToDuration { get; set; }
            public string Premium { get; set; }
            public string Duration { get; set; }
            public string Type { get; set; }
            public string Individual { get; set; }
            public string Adults { get; set; }
            public string Childrens { get; set; }
            public string PlanID { get; set; }
            public string ProductName { get; set; }
            public string Floater { get; set; }
            public string BeforeServiceTax { get; set; }

            public string SelectValue { get; set; }
            public string SelectBlance { get; set; }
    }
}