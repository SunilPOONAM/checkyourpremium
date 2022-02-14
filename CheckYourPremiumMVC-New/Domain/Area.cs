using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
    public class Area
    {
        public int areaID { get; set; }
        public string areaName { get; set; }
    }

    public class AreaList
    {
        public List<Area> area { get; set; }
    }
}
