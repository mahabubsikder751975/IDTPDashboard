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
        
        public dynamic GetServerData(){
            DashboardManager dashboardManager = new();
            var data = dashboardManager.GetMachineCounters(_idtpsvrs);

            return data;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult MonitorRoutes()
        {            
            return View();
        }
    }
}
