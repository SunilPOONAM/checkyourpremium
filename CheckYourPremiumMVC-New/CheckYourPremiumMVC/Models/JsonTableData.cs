using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckYourPremiumMVC.Models
{
    public class JsonTableData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<List<string>> data { get; set; }
    }
}