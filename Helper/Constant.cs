namespace IDTPDashboards.Helper
{
    public static class Constants
    {        
        public static string ServerUrl = "https://localhost:5003/";
        

        public static class APIEndPoints
        {
            public static readonly string GETIDTPSERVERDETAIL = ServerUrl + "IDTPServer";            
        }

        public const string NotFound = "404";
        public const string BadRequest = "400";        
        
    }
}
