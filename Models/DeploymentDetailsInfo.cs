using System.Collections.Generic;

namespace SuperAdminPortal.Models
{
    public class DeploymentDetailsInfo
    {
        public DeploymentDetailsInfo()
        {
            IDTPPortalConfig = new();
            IDTPServerConfig = new();
            IDTPDatabaseConfig = new();
            LBServerConfig = new();
            TimeServerConfig = new();
        }
        public string SourceLocation { get; set; }
        public List<MachineInfo> IDTPPortalConfig { get; set; }
        public List<MachineInfo> IDTPServerConfig { get; set; }
        public List<MachineInfo> LBServerConfig { get; set; }
        public List<MachineInfo> TimeServerConfig { get; set; }
        public List<DatabaseInfo> IDTPDatabaseConfig { get; set; }
        public string TestingMachineUserName { get; set; }
        public string TestingMachinePassword { get; set; }
        public int TestingMachinePort { get; set; }
        public string TestingDatabaseUserName { get; set; }
        public string TestingDatabasePassword { get; set; }
    }
    public class MachineInfo
    {
        public string MachineIP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SSHPort { get; set; }
    }
    public class DatabaseInfo
    {
        public string MachineIP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
