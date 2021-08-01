using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IDTPDashboards.Models;
using IDTPDashboards.Helper;
using Newtonsoft.Json;

namespace IDTPDashboards.Services
{

    public class DashboardManager
    {
         private static readonly string[] IDTPServers = new[]
        {
            "Database Server", "API Server 1", "API Server 2", "API Server 3", "Portal Server 1", "Portal Server 2"
        };

        private static readonly string[] IDTPRacks = new[]
        {
            "Rack 1", "Rack 2", "Rack 3"
        };

        public IEnumerable<ServerHealthDetails> GetMachineCounters( List<ServerIP> idtpsvrs)
        {                        
            var rng = new Random();
            List<ServerHealthDetails> servers = new();
            int k=0;
            for(int i=0;i<IDTPRacks.Count();i++)
            {
                  for(int j=0;j<IDTPServers.Count();j++)
                    {
                      servers.Add( new ServerHealthDetails
                        {
                            Date = DateTime.Now,
                            TemperatureC = 0,
                            ServerHeartbeat = IsHealthy(idtpsvrs[k].IPAddress),                            
                            ServerName = IDTPServers[j],  
                            RackName = IDTPRacks[i],
                            ServerRackId=j,  
                            RackId = i,                            
                        });
                    k++;
                    }
            } 

            // //For Demo
            // int index = rng.Next(servers.Count);           
            // servers.ToArray()[index].ServerHeartbeat=false;
            
            return servers;
           
        }

        private bool IsHealthy(string ipadress)   
        {
            try{
                //ToDo 
                var result = HttpClientHelper.Post(new Uri(ipadress+'/'+Constants.APIEndPoints.TESTHELLO), JsonConvert.SerializeObject(null));
                var responseObj = JsonConvert.DeserializeObject<List<ServerHealthDetails>>(result);
                return true;
            }
            catch(Exception ex){
                return false;

            }
      
        } 


    }
}