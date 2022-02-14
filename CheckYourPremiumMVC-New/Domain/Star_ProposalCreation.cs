using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Star_ProposalCreation
    {
        private string apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        private string secretKey =  System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
        private int _postalcode = 0;
        private int _period = 2;
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
                secretKey= value;
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
        public int planId { get; set; }
        public int postalCode
        {
            get
            {
                return _postalcode;
            }
            set
            {
                _postalcode = value;
            }
        }
        public int period
        {
            get
            {
                return _period;
            }
            set
            {
                _period = value;
            }
        }
        public Int64 searchId { get; set; }
        public string companyId { get; set; }
        public List<Insured> insureds{ get; set; }

        
    }
    
}
