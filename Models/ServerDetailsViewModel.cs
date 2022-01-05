using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAdminPortal.Models
{
    public class ServerDetailsViewModel
    {
        public ServerDetailsViewModel()
        {
            APIServers = new();
            PortalServers = new();
            LBServers = new();
            DBServers = new();
        }
        public List<ServerDetails> APIServers { get; set; }
        public List<ServerDetails> PortalServers { get; set; }
        public List<ServerDetails> LBServers { get; set; }
        public List<ServerDetails> DBServers { get; set; }
        public List<ServerDetails> TimeServers { get; set; }
    }
}
