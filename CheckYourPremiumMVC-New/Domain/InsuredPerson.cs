using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
  
    public class InsuredPerson
    {
        public string name { get; set; }
        public string dob { get; set; }
        public string sex { get; set; }
        public int relationshipId { get; set; }
        public string illness { get; set; }
        public string passportNumber { get; set; }
        public string passportExpiry { get; set; }
        public string visaType { get; set; }
        public string assigneeName { get; set; }
        public string assigneeRelationshipId { get; set; }
    }

}
