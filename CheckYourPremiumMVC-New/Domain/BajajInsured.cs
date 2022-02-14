using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Domain
{
  public  class BajajInsured
    {
        private int _suminsured = 2;
        [Required]
        public string dob { get; set; }
        public int sumInsuredId
        {
            get
            {
                return _suminsured;
            }
            set
            {
                _suminsured = value;
            }
        }
        [Required]
        public string name { get; set; }
        public string sex { get; set; }
        public string illness { get; set; }
        public string description { get; set; }
        [Required]
        public string relationshipId { get; set; }

        public string occupationId { get; set; }
        public string hospitalCash { get; set; }

        public int height { get; set; }
        public int weight { get; set; }
        public string isPersonalAccidentApplicable { get; set; }
        public string engageManualLabour { get; set; }
        public string engageWinterSports { get; set; }
        public int buyBackPED { get; set; }
    }
}
