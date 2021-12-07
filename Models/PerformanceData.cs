using System;

namespace IDTPDashboards.Models
{
    public class PerformanceData
    {
        public float cpu { get; set; }

        public float memory { get; set; }
        public float disk { get; set; }

        public float network { get; set; }
        public string machinename { get; set; }

        public DateTime mCdate { get; set; }        
    }
}
