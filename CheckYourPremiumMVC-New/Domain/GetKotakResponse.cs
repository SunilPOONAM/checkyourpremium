using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    // class GetKotakResponse
    //{
   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
   public class Product
   {
       public string schemeCode { get; set; }
   }

   public class Premium
   {
       public double value { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
       public double totalgst { get; set; }
   }

   public class Loading
   {
       public string type { get; set; }
       public double amount { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
       public double totalgst { get; set; }
       public object totalAmount { get; set; }
   }

   public class Coverage
   {
       public string code { get; set; }
       public double sumAssured { get; set; }
       public Premium premium { get; set; }
       public List<Loading> loading { get; set; }
   }

   public class Premium2
   {
       public string frequency { get; set; }
       public double installmentPremium { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
       public double totalgst { get; set; }
   }

   public class PolicyInfo
   {
       public Product product { get; set; }
       public int policyTerm { get; set; }
       public int premiumPayingTerm { get; set; }
       public List<Coverage> coverages { get; set; }
       public Premium2 premium { get; set; }
   }

   public class Premium3
   {
       public double value { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
       public double totalgst { get; set; }
       public double totalPremium { get; set; }
       public double annualizedPremium { get; set; }
       public double stampDutyAmount { get; set; }
       public double grossPremium { get; set; }
   }

   public class Loading2
   {
       public string type { get; set; }
       public double amount { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
       public double totalgst { get; set; }
       public double totalAmount { get; set; }
   }

   public class Coverage2
   {
       public string variant { get; set; }
       public string code { get; set; }
       public double sumAssured { get; set; }
       public Premium3 premium { get; set; }
       public List<Loading2> loading { get; set; }
   }

   public class Premium4
   {
       public string frequency { get; set; }
       public double installmentPremium { get; set; }
       public double sgst { get; set; }
       public double cgst { get; set; }
       public double igst { get; set; }
       public double ugst { get; set; }
   }

   public class SecondYearPremiumInfo
   {
       public List<Coverage2> coverages { get; set; }
       public Premium4 premium { get; set; }
   }

   public class MaturityBenefitInfo
   {
   }

   public class Root1
   {
       public PolicyInfo policyInfo { get; set; }
       public string result { get; set; }
       public SecondYearPremiumInfo secondYearPremiumInfo { get; set; }
       public MaturityBenefitInfo maturityBenefitInfo { get; set; }
   }

    }


