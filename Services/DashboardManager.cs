using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IDTPDashboards.Models;
using IDTPDashboards.Helper;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;


namespace IDTPDashboards.Services
{

    public class DashboardManager
    {
        private readonly string _connectionString;
        public DashboardManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureDbConnection");
        }   
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
            // var rng = new Random();
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
                            ServerHeartbeat = string.IsNullOrWhiteSpace(idtpsvrs[k].IPAddress) ? false : IsMachinelive(idtpsvrs[k].IPAddress),
                            IsHelloTested = false,  
                            IsInsertTested = false,

                            //IsHelloTested = TestHello(idtpsvrs[k].IPAddress),  
                            //IsInsertTested = TestDataInsert(idtpsvrs[k].IPAddress),
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

        private bool IsMachinelive(string ipadress) 
        {

            bool output = true;
            var ping = new System.Net.NetworkInformation.Ping();
            var ipaddr = ipadress.Split(':')[0];
            var result = ping.Send(ipaddr);

            if (result.Status != System.Net.NetworkInformation.IPStatus.Success)
                output = false;

            return output;
        }



        private bool TestHello(string ipadress)   
        {
            try{

                // //ToDo 
                var url=new Uri(ipadress+'/'+Constants.APIEndPoints.TESTHELLO);
                var response = HttpClientHelper.Get(url);
                
                if (response.StatusCode==200)	
                	return true;
			    else
				    return false;
                
                return true;
            }
            catch(Exception ex){
                return false;

            }

        } 

        private bool TestDataInsert(string ipadress)   
        {
            try{

                // //ToDo 
                var url=new Uri(ipadress+'/'+Constants.APIEndPoints.TESTINSERT);
                var response = HttpClientHelper.Post(url,"Insert data");


                return true;
            }
            catch(Exception ex){
                return false;

            }

        } 

        public dynamic GetICPNetChartData(){
           
            var jsonData = File.ReadAllText("tree.json");
            // IList<ICPServerIP> iCPServerIPs = JsonConvert.DeserializeObject<IList<ICPServerIP>>(jsonData);
            //return JsonConvert.SerializeObject(jsonData);
            return jsonData;
        }

        public IEnumerable<ICPServerHealthDetails> GetICPMachineCounters()
        {                        
            var rng = new Random();
            List<ICPServerHealthDetails> servers = new();
            int k=0;
          
            var icpData = GetICPNetChartData();
           // ICPNetChartData chartData  = JsonConvert.DeserializeObject(icpData);
           // IList<ICPNetChartData> chartData = JsonConvert.DeserializeObject<IList<ICPNetChartData>>(json);
            var chartData = JsonConvert.DeserializeObject<IList<ICPNetChartData>>(icpData);

            for(int j=0;j<chartData.Count;j++)
            {
                servers.Add( new ICPServerHealthDetails
                {
                    Date = DateTime.Now,
                    TemperatureC = 0,                    
                    IsHelloTested = TestHello(chartData[j].ipport),                      
                    BankName = chartData[j].name                                            
                });
            k++;
            }
       

            // //For Demo
            //   int index = rng.Next(servers.Count);           
            //   servers.ToArray()[index].ServerHeartbeat=false;
            
            return servers;
           
        }   

        public IEnumerable<PerformanceData> GetServerPerfData()
        {                        
            var rng = new Random();
            List<PerformanceData> servers = new();
            int k=0;
          
            var icpData = GetServerPerfJSONDataStream();
           // ICPNetChartData chartData  = JsonConvert.DeserializeObject(icpData);
           // IList<ICPNetChartData> chartData = JsonConvert.DeserializeObject<IList<ICPNetChartData>>(json);
            var perfData = JsonConvert.DeserializeObject<IList<PerformanceData>>(icpData);

            return perfData;           
        } 

         public dynamic GetServerPerfJSONDataStream(){
           
            var jsonData = File.ReadAllText("serverPerfDataGenerator.json");
            // IList<ICPServerIP> iCPServerIPs = JsonConvert.DeserializeObject<IList<ICPServerIP>>(jsonData);
            //return JsonConvert.SerializeObject(jsonData);
            return jsonData;
        }

        public List<PerformanceData> GetServerPerfDataFromDB(string machinename)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("IDTP_Dashboard_GetMachinePerfData", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MachineName", machinename));
                    cmd.Parameters.Add(new SqlParameter("@TDate", DateTime.Now));
                    //cmd.Parameters.Add(new SqlParameter("@pageNumber", PageNo));

                    var performanceDatas = new List<PerformanceData>();
                   

                    sql.Open();

                    try
                    {
                        using (var reader =  cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                performanceDatas.Add(PerformanceDataMapToValue(reader));
                            }                           
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    return performanceDatas;
                }
            }
        }

        public List<ServerName> GetMachinesNameFromDB()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("IDTP_Dashboard_GetMachinesName", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

                    List<ServerName> machinenames = new List<ServerName>();
                   

                    sql.Open();

                    try
                    {
                        using (var reader =  cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               machinenames.Add(ServerNameMapToValue(reader));
                            }                           
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    return machinenames;
                }
            }
        }

        private PerformanceData PerformanceDataMapToValue(SqlDataReader reader)
        {
            return new PerformanceData()
            {
                cpu=float.Parse(reader["CPU_Used_Percent"].ToString()),
                memorytotal=float.Parse(reader["Memory_Total"].ToString()),
                memoryused=float.Parse(reader["Memory_Used"].ToString()),
                memory=float.Parse(reader["Memory_Used_Percent"].ToString()),
                disk=float.Parse(reader["Disk_Used_Percent"].ToString()), 
                diskused=float.Parse(reader["Disk_Used"].ToString()), 
                disktotal=float.Parse(reader["Disk_Total"].ToString()), 
                network=float.Parse(reader["Network_Used_Percent"].ToString()),
                networkused=float.Parse(reader["Network_Used"].ToString()),
                networktotal=float.Parse(reader["Network_Total"].ToString()),
                machinename=reader["MachineName"].ToString(),
                mCdate= DateTime.Parse(reader["MetricCollectionDate"].ToString())
            };
        }
        private ServerName ServerNameMapToValue(SqlDataReader reader)
        {
            return new ServerName()
            {
                Id=Int32.Parse(reader["Id"].ToString()),
                MachineName=reader["MachineName"].ToString()
            };
        }

    }
}
