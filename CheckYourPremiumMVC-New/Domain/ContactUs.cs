using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class ContactUs
    {

        public int Id { get; set; }
       public string From { get; set; }
        public string Mobile_No { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }

    }


     public class Complaint
    {

        public int Id { get; set; }
          public string From { get; set; }
        public string Mobile_No { get; set; }
          public string Subject { get; set; }
        public string Policy_No { get; set; }
        public string Message { get; set; }
          public string Date { get; set; }
:
    }
      public class Post_sales_service
    {

        public int Id { get; set; }
           public string From { get; set; }
        public string Mobile_No { get; set; }
          public string Subject { get; set; }
           public string Date { get; set; }
       
        public string Message { get; set; }
:
    }
          public class Claim_Assistance
    {
        public int Id { get; set; }
        public string Mobile_No { get; set; }
               public string From { get; set; }
               public string Date { get; set; }
          public string Subject { get; set; }
         public string Policy_No { get; set; }
        public string Message { get; set; }
    }

    
}
