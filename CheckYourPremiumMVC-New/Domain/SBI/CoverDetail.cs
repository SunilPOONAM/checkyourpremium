using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class CoverDetail
    {
        public CoverAttributesDetails coverAttributesDetails { get; set; }
        public string coverTypeId { get; set; }
        public string delete { get; set; }
        public string coverId { get; set; }
        public List<Benefit> benefits { get; set; }
        public string coverTypeName { get; set; }
    }
}
