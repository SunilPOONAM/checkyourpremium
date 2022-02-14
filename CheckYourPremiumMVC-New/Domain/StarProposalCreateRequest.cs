using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class StarProposalCreateRequest
    {
        private string apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        private string secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
        private string PolicyTypeName = "OMPIND";
        public string APIKEY
        {
            get
            {
                return apiKey;
            }
            set
            {
                apiKey = value;
            }
        }
        public string SECRETKEY
        {
            get
            {
                return secretKey;
            }
            set
            {
                secretKey = value;
            }
        }
        public string policyTypeName
        {
            get
            {
                return PolicyTypeName;
            }
            set
            {
                PolicyTypeName = value;
            }
        }
        public string travelStartOn { get; set; }
        public string travelEndOn { get; set; }
        public string proposerName { get; set; }
        public string proposerEmail { get; set; }
        public string proposerPhone { get; set; }
        public string proposerAddressOne { get; set; }
        public string proposerAddressTwo { get; set; }
        public string proposerAreaId { get; set; }
        public string gstTypeId { get; set; }
        public string gstIdNumber { get; set; }
        public string eiaNumber { get; set; }
        public string proposerDob { get; set; }
        public Int64 planId { get; set; }
        public string placeOfVisit { get; set; }
        public int travelDeclaration { get; set; }
        public int travelPurposeId { get; set; }
        public string physicianName { get; set; }
        public string physicianContactNumber { get; set; }
        public InsuredPerson insureds { get; set; }

    }
}
