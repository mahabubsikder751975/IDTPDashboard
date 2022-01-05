namespace SuperAdminPortal.Models
{
    public class DeploymentConfiguration
    {
        public string SourceLocation { get; set; }
        public string MachineUserName { get; set; }
        public string MachinePassword { get; set; }
        public int MachinePort { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string DeploymentMachineIP { get; set; }

    }
}
