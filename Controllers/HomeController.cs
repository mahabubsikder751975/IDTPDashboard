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

namespace IDTPDashboards.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
            ServerHealthDetails serverHealthDetails = new();         
            // Read the response
             var result = HttpClientHelper.Post(new Uri(Constants.APIEndPoints.GETIDTPSERVERDETAIL), JsonConvert.SerializeObject(serverHealthDetails));
            var responseObj = JsonConvert.DeserializeObject<List<ServerHealthDetails>>(result);
            return responseObj;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
