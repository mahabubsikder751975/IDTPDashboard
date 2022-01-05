using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAdminPortal.Models
{
    public class DBMachineInfo
    {
        public long HDDSpaceTotal { get; set; }
        public long HDDSpaceFree { get; set; }

        public string CPUCoreCount { get; set; }
        public string CPUName { get; set; }

        public string Time { get; set; }

        public string TotalRam { get; set; }
        public string FreeRam { get; set; }
    }
    public class DBMachineHDDInfo
    {
        public string Drive { get; set; }
        public long TotalSpaceGB { get; set; }
        public long FreeSpaceGB { get; set; }
    }
    public class DBMachineCPURamInfo
    {
        public int CoreCount { get; set; }
        public long TotalRam { get; set; }
        public long FreeRam { get; set; }
        public DateTime Time { get; set; }
        public string HostName { get; set; }
    }
}
