using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renci.SshNet;
using SuperAdminPortal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SuperAdminPortal.Controllers
{
    public class ServerMonitorController : Controller
    {
        public IActionResult Index()
        {
            var detailsViewModel = GetServersDetails();
            return View(detailsViewModel);
        }

        // Called via ajax every 7 seconds
        [HttpPost]
        public dynamic GetServersDetailsInfo()
        {
            return GetServersDetails();
        }

        private ServerDetailsViewModel GetServersDetails()
        {
            ServerDetailsViewModel detailsViewModel = new();
            ServerInfo serverInfo;
            using (StreamReader r = new StreamReader("ServerConfigurations.json"))
            {
                string json = r.ReadToEnd();
                serverInfo = JsonConvert.DeserializeObject<ServerInfo>(json);
            }

            DeploymentDetailsInfo deploymentDetails = new();
            using (StreamReader r = new StreamReader("DeploymentDetailsConfiguration.json"))
            {
                string json = r.ReadToEnd();
                deploymentDetails = JsonConvert.DeserializeObject<DeploymentDetailsInfo>(json);
            }

            List<ServerDetails> apiServerDetails = new();
            foreach (var item in deploymentDetails.IDTPServerConfig)
            {
                ServerDetails apiServer = GetServerInfoSSHNet(item);
                apiServer.ServerType = ServerType.API;
                apiServer.MachineIP = item.MachineIP;
                if (!apiServer.ConnectionFailure)
                {
                    apiServer.PingResult = PingHost(item.MachineIP);
                    apiServer.MachineID = GetAPIMachineIDSSHNet(item);
                    apiServer.IsTimeInSync = ValidateTimeInSync(apiServer.Time);
                }
                apiServerDetails.Add(apiServer);
            }
            detailsViewModel.APIServers = apiServerDetails;

            List<ServerDetails> portalServerDetails = new();
            foreach (var item in deploymentDetails.IDTPPortalConfig)
            {
                ServerDetails portalServer = GetServerInfoSSHNet(item);
                portalServer.ServerType = ServerType.PORTAL;
                portalServer.MachineIP = item.MachineIP;
                if (!portalServer.ConnectionFailure)
                {
                    portalServer.PingResult = PingHost(item.MachineIP);
                    portalServer.MachineID = GetPortalMachineIDSSHNet(item);
                    portalServer.IsTimeInSync = ValidateTimeInSync(portalServer.Time);
                }
                portalServerDetails.Add(portalServer);
            }
            detailsViewModel.PortalServers = portalServerDetails;

            List<ServerDetails> lbServerDetails = new();
            foreach (var item in deploymentDetails.LBServerConfig)
            {
                ServerDetails lbServer = new();
                var pingResult = PingHost(item.MachineIP);
                if (pingResult)
                {
                    lbServer.ServerType = ServerType.LB;

                    lbServer = GetServerInfoSSHNet(item);
                    lbServer.IsTimeInSync = ValidateTimeInSync(lbServer.Time);
                    lbServer.MachineIP = item.MachineIP;
                    lbServer.MachineID = "N/A";
                    lbServer.PingResult = pingResult;
                    lbServerDetails.Add(lbServer);
                }
                else
                {
                    lbServer.ServerType = ServerType.LB;
                    lbServer.MachineIP = item.MachineIP;
                    lbServerDetails.Add(lbServer);
                }
            }
            detailsViewModel.LBServers = lbServerDetails;

            List<ServerDetails> dbServerDetails = new();
            foreach (var item in deploymentDetails.IDTPDatabaseConfig)
            {
                //ServerDetails portalServer = GetServerInfoSSHNet(item);
                ServerDetails dbServer = new();
                dbServer.ServerType = ServerType.DB;
                dbServer.MachineIP = item.MachineIP;
                dbServer.PingResult = PingHost(item.MachineIP);
                
                dbServer.MachineID = "N/A";
                //dbServer.HostName = "N/A";
                //dbServer.CPUCoreCount = "N/A";
                //dbServer.CPUModel = "N/A";
                //dbServer.HardDiskFree = "N/A";
                //dbServer.HardDiskTotal = "N/A";
                //dbServer.MemoryFree = "N/A";
                //dbServer.MemoryTotal = "N/A";
                //dbServer.Time = "N/A";
                dbServer = GetDBServersDetails(dbServer, item);
                dbServer.IsTimeInSync = ValidateTimeInSync(dbServer.Time); // true; // FOR NOW HARD CODE
                dbServerDetails.Add(dbServer);
            }
            detailsViewModel.DBServers = dbServerDetails;


            List<ServerDetails> timeServerDetails = new();
            foreach (var item in deploymentDetails.TimeServerConfig)
            {
                ServerDetails timeServer = new();
                var pingResult = PingHost(item.MachineIP);
                if (pingResult)
                {
                    timeServer.ServerType = ServerType.LB;

                    timeServer = GetServerInfoSSHNet(item);
                    timeServer.IsTimeInSync = ValidateTimeInSync(timeServer.Time);

                    timeServer.MachineID = "N/A";
                    timeServer.MachineIP = item.MachineIP;
                    timeServer.PingResult = pingResult;
                    timeServerDetails.Add(timeServer);
                }
                else
                {
                    timeServer.ServerType = ServerType.LB;
                    timeServer.MachineIP = item.MachineIP;
                    timeServerDetails.Add(timeServer);
                }
            }
            detailsViewModel.TimeServers = timeServerDetails;


            return detailsViewModel;
        }

        private ServerDetails GetDBServersDetails(ServerDetails dbServer, DatabaseInfo machine)
        {
            try
            {
                DBMachineInfo machineInfo = new();
                string connString = GenerateConnectionString(machine, "master");
                string hddQuery = @$"SELECT Drive,TotalSpaceGB, FreeSpaceGB
                                FROM
                                (SELECT DISTINCT
                                    SUBSTRING(dovs.volume_mount_point, 1, 10) AS Drive
                                , CONVERT(INT, dovs.total_bytes / 1024.0 / 1024.0 / 1024.0) AS TotalSpaceGB
                                , CONVERT(INT, dovs.available_bytes / 1048576.0) / 1024 AS FreeSpaceGB
                                FROM    sys.master_files AS mf
                                CROSS APPLY sys.dm_os_volume_stats(mf.database_id, mf.file_id) AS dovs)
                                AS DE";
                long hddTotal = 0;
                long hddFree = 0;
                try
                {
                    List<DBMachineHDDInfo> hddPerDrive = SqlClientHelper.GetData<DBMachineHDDInfo>(hddQuery, System.Data.CommandType.Text, connString);

                    if (hddPerDrive.Count > 0)
                    {
                        hddTotal = hddPerDrive.Sum(x => x.TotalSpaceGB);
                        hddFree = hddPerDrive.Sum(x => x.FreeSpaceGB);
                    }
                }
                catch (Exception queryEx)
                {
                    Console.WriteLine("GetDBServersDetails>HDDQuery Ex: " + queryEx.Message);
                }
                dbServer.HardDiskTotal = hddTotal.ToString() + "GB";
                dbServer.HardDiskFree = hddFree.ToString() + "GB";

                string procNameQuery = @$"declare @ProcInfo as table
                                          (
	                                          [Value] Varchar(100),
	                                          [Data] Varchar(300)
                                          )  
                                          INSERT INTO @ProcInfo
                                          EXEC sys.xp_instance_regread N'HKEY_LOCAL_MACHINE', N'HARDWARE\DESCRIPTION\System\CentralProcessor\0', N'ProcessorNameString';
                                          SELECT Data FROM @ProcInfo";
                string processorName = string.Empty;
                try
                {
                    processorName = SqlClientHelper.GetSingleRecord(procNameQuery, System.Data.CommandType.Text, connString);
                }
                catch (Exception procNameEx)
                {
                    Console.WriteLine("GetDBServersDetails>CPUNameQuery Ex: " + procNameEx.Message);
                }

                dbServer.CPUModel = processorName;

                string cpuRamDetailsQuery = $@"SELECT cpu_count AS [CoreCount],
                                            physical_memory_kb/1024 AS [TotalRam], (physical_memory_kb/1024 - committed_kb/1024) AS [FreeRam]
                                            , GETDATE() As Time
                                            ,@@servername As HostName
                                            --,committed_target_kb/1024 AS [CommittedMemory]
                                            FROM sys.dm_os_sys_info WITH (NOLOCK) OPTION (RECOMPILE);";
                try
                {
                    List<DBMachineCPURamInfo> cpuRamInfo = SqlClientHelper.GetData<DBMachineCPURamInfo>(cpuRamDetailsQuery, System.Data.CommandType.Text, connString);

                    if (cpuRamInfo.Count > 0)
                    {
                        dbServer.CPUCoreCount = cpuRamInfo[0].CoreCount.ToString();
                        dbServer.MemoryTotal = cpuRamInfo[0].TotalRam.ToString() + "MB";
                        dbServer.MemoryFree = cpuRamInfo[0].FreeRam.ToString() + "MB";
                        dbServer.Time = cpuRamInfo[0].Time.ToString();
                        dbServer.HostName = cpuRamInfo[0].HostName;
                    }
                }
                catch (Exception queryEx)
                {
                    Console.WriteLine("GetDBServersDetails>CpuRamInfoQuery Ex: " + queryEx.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDBServersDetails Ex: " + ex.Message);
            }
            return dbServer;
        }
        public static string GenerateConnectionString(DatabaseInfo databaseInfo, string db_name)
        {
            string dbServerIP = databaseInfo.MachineIP;
            dbServerIP = dbServerIP.Contains(':') ? dbServerIP.Replace(':', ',') : dbServerIP;
            string connString = $"Server={dbServerIP};Database={db_name};User ID={databaseInfo.UserName};password={databaseInfo.Password}";
            return connString;
        }
        private bool ValidateTimeInSync(string time)
        {
            //var timenow = DateTime.Now;
            //var timeInServer = Convert.ToDateTime(time);
            ////Console.WriteLine($"Time local: {timenow}, Time Server {timeInServer}, Time diff: {(timenow - timeInServer).TotalSeconds}");
            //if ((timenow - Convert.ToDateTime(time)).TotalSeconds > 30 || (timenow - Convert.ToDateTime(time)).TotalSeconds < -30)
            //{
            //    return false;
            //}
            //return true;
            return true;
        }

        private ServerDetails GetServerInfoSSHNet(MachineInfo item)
        {
            try
            {
                string resp = string.Empty;
                ServerDetails responseDto = new ServerDetails();
                using (SshClient client = new SshClient(item.MachineIP, Convert.ToInt32(item.SSHPort), item.UserName, item.Password))
                {
                    //Console.WriteLine("Connect to server");
                    client.Connect();
                    //Console.WriteLine("Connected to server");
                    string command = @$"hostname; cat /proc/cpuinfo | grep -m1 ""cpu core""; cat /proc/cpuinfo | grep -m1 ""model name""; cat /proc/meminfo | grep ""MemTotal""; cat /proc/meminfo | grep ""MemFree""; df -H /; date";
                    var command2 = client.RunCommand(command);
                    resp = command2.Result;
                    //Console.WriteLine(resp);
                    List<string> detailInfo = resp.Split('\n').Select(x => x.Trim()).ToList();
                    if (detailInfo.Count > 0)
                    {
                        responseDto.HostName = !string.IsNullOrEmpty(detailInfo[0]) ? detailInfo[0] : string.Empty;
                        responseDto.CPUModel = !string.IsNullOrEmpty(detailInfo[2]) ? detailInfo[2].Split(':')[1] : string.Empty;
                        responseDto.CPUCoreCount = !string.IsNullOrEmpty(detailInfo[1]) ? detailInfo[1].Split(':')[1] : string.Empty;
                        responseDto.MemoryTotal = !string.IsNullOrEmpty(detailInfo[3]) ? detailInfo[3].Split(':')[1].TrimStart() : string.Empty;
                        responseDto.MemoryFree = !string.IsNullOrEmpty(detailInfo[4]) ? detailInfo[4].Split(':')[1].TrimStart() : string.Empty;

                        var tempTime = !string.IsNullOrEmpty(detailInfo[7]) ? detailInfo[7] : string.Empty;
                        if (!string.IsNullOrEmpty(tempTime))
                        {
                            if (tempTime.Contains("PDT"))
                            {
                                tempTime = tempTime.Replace("PDT", string.Empty);
                                tempTime = tempTime.TrimEnd();
                            }
                        }
                        responseDto.Time = tempTime;

                        var allHardDiskInfo = detailInfo[6].Split();
                        List<string> hardDiskDetails = new();
                        foreach (var info in allHardDiskInfo)
                        {
                            if (!string.IsNullOrEmpty(info))
                            {
                                hardDiskDetails.Add(info);
                            }
                        }
                        responseDto.HardDiskTotal = hardDiskDetails[1];
                        responseDto.HardDiskFree = hardDiskDetails[3];
                    }
                    client.Dispose();
                }
                return responseDto;
            }
            catch (Exception ex)
            {
                return new ServerDetails() { ConnectionFailure = true };
            }
        }
        private string GetPortalMachineIDSSHNet(MachineInfo item)
        {
            try
            {
                string resp = string.Empty;
                ServerDetails responseDto = new ServerDetails();
                using (SshClient client = new SshClient(item.MachineIP, Convert.ToInt32(item.SSHPort), item.UserName, item.Password))
                {
                    client.Connect();
                    string command = @$"cat /var/www/idtpportal/appsettings.json | grep ""MachineID*""";
                    var command2 = client.RunCommand(command);
                    resp = command2.Result;
                    //Console.WriteLine(resp);
                    var responses = resp.Replace("\"", "").Replace(" ", "").Replace("\r", "").Replace("\n", "").Split(":");
                    resp = "Machine_"+ responses[2]; // string.Concat(responses[1].Split('_')[1], "_", responses[1].Split('_')[2].Substring(0, responses[1].Split('_')[2].Length - 2));
                    //Console.WriteLine(resp);
                    client.Dispose();
                }
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPortalMachineIDSSHNet ex" + ex.Message);
                return string.Empty;
            }
        }
        private string GetAPIMachineIDSSHNet(MachineInfo item)
        {
            try
            {
                string resp = string.Empty;
                ServerDetails responseDto = new ServerDetails();
                using (SshClient client = new SshClient(item.MachineIP, Convert.ToInt32(item.SSHPort), item.UserName, item.Password))
                {
                    client.Connect();
                    string command = @$"cat /var/www/idtpapi/appsettings.json | grep ""PingReturn""";
                    var command2 = client.RunCommand(command);
                    resp = command2.Result;
                    //Console.WriteLine(resp);
                    var responses = resp.Replace("\"", "").Replace(" ", "").Replace("\r", "").Replace("\n", "").Split(":");
                    resp = string.Concat(responses[1].Split('_')[2], "_", responses[1].Split('_')[3]);
                    client.Dispose();
                }
                return resp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetPortalMachineIDSSHNet ex" + ex.Message);
                return string.Empty;
            }
        }
        private static bool PingHost(string nameOrAddress)
        {
            return GetPing(nameOrAddress);
        }
        internal static bool GetPing(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;
            if (nameOrAddress.Contains(":"))
            {
                nameOrAddress = nameOrAddress.Split(":")[0];
            }
            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                if (reply != null) pingable = reply.Status == IPStatus.Success;
                //time = reply.RoundtripTime.ToString();
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                pinger?.Dispose();
            }

            return pingable;
        }
    }
}
