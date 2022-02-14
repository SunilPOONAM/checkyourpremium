using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class GetKotakDetails
    {
       public string Response { get; set; }
       public string GetQuotesResponse { get; set; }

    }
   // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
   public class GetQuotesResponse
   {
       public object ErrMsg { get; set; }
       public object QuotationNumber { get; set; }
       public string Response { get; set; }
       public string Status { get; set; }
   }

   public class Root
   {
       public GetQuotesResponse GetQuotesResponse { get; set; }
   }
}
