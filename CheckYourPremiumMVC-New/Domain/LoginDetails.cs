using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class LoginDetails
    {
       public int UserId { get; set; }
       public string EmailId { get; set; }
       public string Mobile{ get; set; }
       public string Password { get; set; }
       public string NewPassword { get; set; }
       public string RepeatNewPass { get; set; }
       public int attempttime { get; set; }
       
    }
}
