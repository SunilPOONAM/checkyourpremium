using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class Createquoterequest
    {
        public List<InsuredDetail> insuredDetails { get; set; }
        public string product { get; set; }
        public ProposerDetails proposerDetails { get; set; }
        public List<Claus> clauses { get; set; }
        public LocationDetails locationDetails { get; set; }
        public CommercialConsideration commercialConsideration { get; set; }
        public QuoteDetails quoteDetails { get; set; }
    }
}
