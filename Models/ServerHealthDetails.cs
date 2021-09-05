using System;

namespace IDTPDashboards.Models
{
    public class ServerHealthDetails
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int MemoryUsage { get; set; }
        public int MemoryCapacity { get; set; }

        public int CPUUsage { get; set; }
        public int CPUCapacity { get; set; }

        public int PowerUsage { get; set; }
        public int PowerCapacity { get; set; }

        public int NetworkUsage { get; set; }
        public int NetworkCapacity { get; set; }

        // public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public bool ServerHeartbeat { get; set; }
        public bool IsHelloTested { get; set; }
        public bool IsInsertTested { get; set; }
        public bool IsDBConnected { get; set; }
        
        public string ServerName { get; set; }
        public string RackName { get; set; }
        public int ServerRackId { get; set; }
        public int RackId { get; set; }
    }
}
