using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IDTPDashboards.Models;
using IDTPDashboards.Helper;
using Newtonsoft.Json;
using IDTPDashboards.Services;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace IDTPDashboards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly List<ServerIP> _idtpsvrs;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
        {
            _configuration=(IConfiguration)configuration;

            _idtpsvrs = _configuration.GetSection("ServerIPs").Get<List<ServerIP>>();
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ServerMonitor()
        {            
            return View();
        }
         public IActionResult MonitorServerPerformance()
        {            
            return View();
        }

        public IActionResult ICPNetworkMonitor()
        { 
            // var icpJsonData = GetICPServerData();                    
            // ViewData["icpJsonData"] = JsonConvert.SerializeObject(icpJsonData); 
            //var passData = JsonConvert.SerializeObject(icpJsonData); 

            return View();
        }

        public dynamic GetServerData(){
            DashboardManager dashboardManager = new(_configuration);
            var data = dashboardManager.GetMachineCounters(_idtpsvrs);

            return data;
        }

         public dynamic GetICPNetChartData(){
            DashboardManager dashboardManager = new(_configuration);           
            var returnData = dashboardManager.GetICPNetChartData();       

            return returnData;            
        }

        public dynamic GetICPServerData(){
            DashboardManager dashboardManager = new(_configuration);
            var data = dashboardManager.GetICPMachineCounters();

            return data;
        }

        [HttpPost]
        public List<PerformanceData> GetServerPerfData(string machinename){
            DashboardManager dashboardManager = new(_configuration);
            List<PerformanceData> data =null;
            
            data = dashboardManager.GetServerPerfDataFromDB(machinename);
              
            return data;
        }

         [HttpPost]
        public List<ServerName> GetMachinesName(){
           
            DashboardManager dashboardManager = new(_configuration);
            List<ServerName> data =null;

            data = dashboardManager.GetMachinesNameFromDB();
              
            return data;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MonitorRoutes()
        {
            return Redirect("http://192.168.1.31:8070/ServerMonitor/Index");
            //return View();
        }
    }
}
