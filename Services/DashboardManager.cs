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
            "Database Server", "API Server 1", "API Server 2", "API Server 3", "Portal Server", "LB Server"
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
                            ServerHeartbeat = IsMachineActive(idtpsvrs[k].IPAddress),                            
                            ServerName = IDTPServers[j],  
                            RackName = IDTPRacks[i],
                            ServerRackId=j,  
                            RackId = i,                            
                        });
                    k++;
                    }
            } 

            // //For Demo
            //   int index = rng.Next(servers.Count);           
            //   servers.ToArray()[index].ServerHeartbeat=false;
            
            return servers;
           
        }

        private bool IsMachineActive(string url) 
        {

            bool output = true;
            var ping = new System.Net.NetworkInformation.Ping();

            var result = ping.Send(url);

            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                output = false;

            return output;
        }



        private bool IsHealthy(string ipadress)   
        {
            try{

                // //ToDo 
                var url=new Uri(ipadress+'/'+Constants.APIEndPoints.TESTHELLO);
                var response = HttpClientHelper.Get(url);


                return true;
            }
            catch(Exception ex){
                return false;

            }
      
        } 


    }
}