using System.Collections.Generic;

namespace SuperAdminPortal.Models
{
    public class DeployedURLStatus
    {
        public DeployedURLStatus()
        {
            IDTPStatus = new();
            ICPPIMPortalResult = new();
            IDTPPortalStatus = new();
        }
        public List<IDTPAPIResult> IDTPStatus { get; set; }
        public List<GeneralStatus> ICPPIMPortalResult { get; set; }
        public List<GeneralStatus> IDTPPortalStatus { get; set; }

        public string IDTPAPIDeploymentTime { get; set; }

        public class IDTPAPIResult
        {
            public string URL { get; set; }
            public bool HelloResult { get; set; }
            public bool InsertResult { get; set; }
        }
        
        public class DatabaseStatus
        {
            public string DBIP { get; set; }
            public string DBName { get; set; }
            public string Status { get; set; }
            public string CreationTime { get; set; }
        }
        public class GeneralStatus
        {
            public string ComponentName { get; set; }
            public string URL { get; set; }
            public bool HelloResult { get; set; }
            public string DeploymentTime { get; set; }
            public string ServiceName { get; set; }
        }
    }
}
