using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class BajajPremiumDetails
    {
        public Genpremdtlsres Genpremdtlsres { get; set; }
        public RootObject RootObject { get; set; }
        public Servicetaxmstobj Servicetaxmstobj { get; set; }
    }
        public class Genpremdtlsres
        {
            public string contractid { get; set; }
            public string quoterefno { get; set; }
            public string refno { get; set; }
            public string scrutinyno { get; set; }
            public string policyref { get; set; }
            public string netpremium { get; set; }
            public string servicetaxamt { get; set; }
            public string educessamt { get; set; }
            public string totalpremium { get; set; }
            public string finalpremium { get; set; }
            public string servicetaxrate { get; set; }
            public string educessrate { get; set; }
            public string stampduty { get; set; }
            public string uwloadingper { get; set; }
            public string uwloadingamt { get; set; }
            public string loadingper { get; set; }
            public string loadingamt { get; set; }
            public string discountper { get; set; }
            public string discountamt { get; set; }
            public string spdiscountper { get; set; }
            public string spdiscountamt { get; set; }
            public string param1 { get; set; }
            public string param2 { get; set; }
            public string param3 { get; set; }
            public string param4 { get; set; }
        }
   
        public class Servicetaxmstobj
        {
            public string taxcode { get; set; }
            public string taxtype { get; set; }
            public string baseamount { get; set; }
            public string sertax { get; set; }
            public string educess { get; set; }
            public string sbtax { get; set; }
            public string kkc { get; set; }
            public string cess1 { get; set; }
            public string cess2 { get; set; }
            public string cess3 { get; set; }
            public string sertaxdesc { get; set; }
            public string educessdesc { get; set; }
            public string sbcessdesc { get; set; }
            public string kkccessdesc { get; set; }
            public string cess1desc { get; set; }
            public string cess2desc { get; set; }
            public string cess3desc { get; set; }
            public string adjsertax { get; set; }
            public string adjeducess { get; set; }
            public string adjsbtax { get; set; }
            public string adjkkc { get; set; }
            public string adjcess1 { get; set; }
            public string adjcess2 { get; set; }
            public string adjcess3 { get; set; }
            public string addtaxval1 { get; set; }
            public string addtaxval2 { get; set; }
            public string addtaxval3 { get; set; }
            public string addtaxval4 { get; set; }
            public string addtaxval5 { get; set; }
            public string addtaxval1desc { get; set; }
            public string addtaxval2desc { get; set; }
            public string addtaxval3desc { get; set; }
            public string addtaxval4desc { get; set; }
            public string addtaxval5desc { get; set; }
        }

        public class RootObject
        {
            public Genpremdtlsres genpremdtlsres { get; set; }
            public string errorcoderes { get; set; }
            public List<object> errorlistres { get; set; }
            public int transactionid { get; set; }
            public Servicetaxmstobj servicetaxmstobj { get; set; }
        }
    }

