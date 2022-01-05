namespace SuperAdminPortal.Models
{
    public class ServerInfo
    {
        public string IDTPPortal { get; set; }
        public string IDTPServer { get; set; }
        public string TestingSuite { get; set; }
        public string SendingICPPortal { get; set; }
        public string ReceivingICPPortal { get; set; }
        public string SendingPIMPortal { get; set; }
        public string ReceivingPIMPortal { get; set; }
        public string LoadBalancingServer { get; set; }
        //public string ICPServer { get; set; }
        public string SendingICPServer { get; set; }
        public string ReceivingICPServer { get; set; }
        //public string WLAPPServer { get; set; }
        public string SendingWLAPPServer { get; set; }
        public string ReceivingWLAPPServer { get; set; }
        //public string PIMServer { get; set; }
        public string SendingPIMServer { get; set; }
        public string ReceivingPIMServer { get; set; }
        public string DBServer { get; set; }
        public string IDTPServerForICP { get; set; }
        public string IDTPServerForAppServer { get; set; }
        public string TestingDatabaseServer { get; set; }
        public string TimeServer { get; set; }
    }
}
