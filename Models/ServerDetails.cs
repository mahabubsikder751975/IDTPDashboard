using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAdminPortal.Models
{
    public class ServerDetails
    {
        
        public bool PingResult { get; set; }
        public bool IsTimeInSync { get; set; }

        public string HostName { get; set; }
        public string MachineIP { get; set; }
        public string MachineID { get; set; }

        public string CPUModel { get; set; }
        public string CPUCoreCount { get; set; }

        public string MemoryTotal { get; set; }
        public string MemoryFree { get; set; }

        public string HardDiskTotal { get; set; }
        public string HardDiskFree { get; set; }

        public string Time { get; set; }

        public ServerType ServerType { get; set; }
        public bool ConnectionFailure { get; set; }
    }
    public enum ServerType
    {
        LB,
        DB,
        API,
        PORTAL,
        Time
    }
}
