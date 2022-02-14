using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain
{
   public class Car_InsuranceModel
    {
        public long Id { get; set; }
        public long Make_Id { get; set; }
        public string Make { get; set; }
        public long Model_Id { get; set; }
        public string Model { get; set; }
        public int Variant_Id { get; set; }
        public string Variant { get; set; }
        public int CC { get; set; }
        public int Seating_Capacity { get; set; }
        public long Fuel_Id { get; set; }
        public string Fuel_Type { get; set; }
        public long Segment_ID { get; set; }
        public string Vehicle_Segment { get; set; }
        public long Location_ID { get; set; }
        public string LOCATION { get; set; }
        public decimal Price { get; set; }
        public long Main_Vehicle_Type_ID { get; set; }
        public string No_Of_Wheels { get; set; }
        public Nullable<long> PlanID { get; set; }
    }
}
