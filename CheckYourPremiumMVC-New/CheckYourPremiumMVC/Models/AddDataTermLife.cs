using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckYourPremiumMVC.Models
{
    public class AddDataTermLife
    {
        public string fullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Smoke { get; set; }
        public string Age { get; set; }
        public string Income { get; set; }
        public string Gender { get; set; }

       // public string DOB { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]  
        public DateTime DOB { get; set; } 
             public string CustID { get; set; }
    }
    public class City_Cls
    {
        public int State_ID { get; set; }
        public string City_Name { get; set; }


    }
    public class GetDataValue
    {
        public string Gender { get; set; }
        public string Full_Name { get; set; }
        public string MobileNo { get; set; }
        public string income { get; set; }
        public string City { get; set; }
        public string Self { get; set; }
        public string Sona { get; set; }
        public string Spouses { get; set; }
        public string SAge { get; set; }
        public string Action { get; set; }
    }
}