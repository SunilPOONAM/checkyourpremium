using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class InsuredDetail
    {
        public NomineeDetails nomineeDetails { get; set; }
        public string existingDisability { get; set; }
        public string realtionshipWithProposer { get; set; }
        public string occupation { get; set; }
        public string plotNo { get; set; }
        public string buildingName { get; set; }
        public string nomineeRelationWithPrimaryInsured { get; set; }
        public string locationId { get; set; }
        public string state { get; set; }
        public string streetName { get; set; }
        public string city { get; set; }
        public string pinCode { get; set; }
        public string title { get; set; }
        public string emailId { get; set; }
        public List<CoverDetail> coverDetails { get; set; }
        public string partyId { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string locality { get; set; }
        public string district { get; set; }
        public string mobileNumber { get; set; }
    }
}
