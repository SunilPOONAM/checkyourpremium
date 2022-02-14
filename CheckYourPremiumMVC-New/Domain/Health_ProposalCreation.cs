using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Health_ProposalCreation
    {
        private string apiKey = System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();//"53ab886c7c7e88d8e2f7a12872888b32";// System.Configuration.ConfigurationSettings.AppSettings["StarAPIKey"].ToString();
        private string secretKey = System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();//"c68ba59148d7012f352e7efdadffd06f";//System.Configuration.ConfigurationSettings.AppSettings["StarSecretKey"].ToString();
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
        public string policyTypeName { get; set; }
        public string AgeTo { get; set; }
        public string AgeFrom { get; set; }
        public string PlanId { get; set; }
        public string period { get; set; }
        public string postalCode { get; set; }
        public int schemeId { get; set; }
        public int sumInsuredId{ get; set; }

        public Insured insureds_0 { get; set; }
        public Insured insureds_1 { get; set; }
        public Insured insureds_2 { get; set; }
        public Insured insureds_3{ get; set; }
     
    }
}

