using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class InvalidLogin
    {
        public string IP { get; set; }
        public DateTime Attempttime { get; set; }
        public int AttemptCount { get; set; }
    }
}
