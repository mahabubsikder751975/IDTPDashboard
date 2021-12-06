#pragma checksum "E:\IDTPDashboard\Views\Home\ServerMonitor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "acad4c61b026c01a11901a4cea10ec0e7912061b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ServerMonitor), @"mvc.1.0.view", @"/Views/Home/ServerMonitor.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\IDTPDashboard\Views\_ViewImports.cshtml"
using IDTPDashboards;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\IDTPDashboard\Views\_ViewImports.cshtml"
using IDTPDashboards.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"acad4c61b026c01a11901a4cea10ec0e7912061b", @"/Views/Home/ServerMonitor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1de202496fa847446cb7ebfb3e3a55cf1c7d1a5e", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ServerMonitor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ServerHealthDetails>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\IDTPDashboard\Views\Home\ServerMonitor.cshtml"
  
    ViewData["Title"] = "IDTP Server Monitor";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""container"">
    <canvas id=""myChart"" width=""400"" height=""200""></canvas>
</div>


<script>
   
const labels = [""Rack 1"",""Rack 2"",""Rack 3""];
//#CC7722
//LB #4F7942
//DB #234F1E
//API #597D35
//Porta #597D35

let origindatasets = [
    {
      label: 'Database',
      data:  [45,45,45],
      backgroundColor: [
                '#CC7722'
            ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    },
    {
      label: 'API Server 3',
      data:  [45,45,45],
      backgroundColor: [
                '#CC7722'
                ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    },
    {
      label: 'API Server 2',
      data: [45,45,45],
      backgroundColor: [
                '#CC7722'
                ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    },
     {
      label: 'API Server 1',
      data: [45,45,45],
      backgroundColor: [
    ");
            WriteLiteral(@"           '#CC7722'
                ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    },
    
    {
      label: 'Portal Server',
      data: [45,45,45],
      backgroundColor: [
                '#CC7722'
                ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    },
     {
      label: 'LB Server',
      data: [45,45,45],
      backgroundColor: [
                '#CC7722'
                ],
      stack: 'Stack 0',
      borderWidth: 2,
      borderRadius: 5,
      borderSkipped: false,
    }
  ]

let copieddatasets = JSON.parse(JSON.stringify(origindatasets));


const data1 = {
  labels: labels,
  datasets: origindatasets,
};

const config = {
  type: 'bar',
  data: data1,
  options: {    
    plugins: {
    tooltip:false,
      title: {
        display: true,
        text: 'IDTP Server Health Monitoring Dashboard'
      },
    },
    responsive: true,    
    interaction: {
      intersect: fal");
            WriteLiteral(@"se,
    },
    scales: {
      x: {
        stacked: true,
      },
      y: {
        display: false,
        stacked: true,
         ticks: {
                    font: {
                        family: 'Helvetica', // Your font family
                        size: 18,
                    }
                }

      }
    },
    animation: {
    onComplete: function() {
        let ctx = this.$context.chart.ctx;
        ctx.textAlign = ""center"";
        ctx.textBaseline = ""middle"";
        let chart = this;
        let datasets = this.config.data.datasets;
        let sum=new Array();
        datasets.forEach(function (dataset, i) {
            ctx.font = ""18px Helvetica"";

            switch ( chart.getDatasetMeta(i).type ) {
                case ""line"":
                    ctx.fillStyle = ""Black"";
                    chart.getDatasetMeta(i).data.forEach(function (p, j) {
                        ctx.fillText(datasets[i].data[j], p.x, p.y - 20);
                    });
                    break;
             ");
            WriteLiteral(@"   case ""bar"":
                    ctx.fillStyle = ""White"";
                    chart.getDatasetMeta(i).data.forEach(function (p, j) {
                        ctx.fillText(datasets[i].label, p.x, p.y + 20);
                    });
                    break;
                case ""horizontalBar"":
                    ctx.fillStyle = ""Black"";
                    chart.getDatasetMeta(i).data.forEach(function (p, j) {
                    if (sum[j]== null) { sum[j] = 0; }
                    sum[j]=sum[j]+parseFloat(datasets[i].data[j]);
                    if (i==datasets.length-1) {ctx.fillText(sum[j], p.x+10, p.y);}

                    });

                    break;
            }
        });
    }
  }
  
  }
};

Chart.defaults.font.family='Helvetica'
Chart.defaults.font.size=18

var ctx = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(ctx,config);


loadData = function () {

 $.post('GetServerData', {}, 
        function(data){
          $.each(data, function (index, item) {

    ");
            WriteLiteral(@"          if(item.isHelloTested==false){
                myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=copieddatasets[item.serverRackId].backgroundColor[item.rackId];   
              }
              else{
                 myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=""#4F7942"";                
              }


              if(item.isInsertTested==false){
                myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=copieddatasets[item.serverRackId].backgroundColor[item.rackId];   
              }
              else{
                 myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=""#4F7942"";              
              }


              if(item.serverHeartbeat==false){
                myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=copieddatasets[item.serverRackId].backgroundColor[item.rackId];               
              }
              else{
              ");
            WriteLiteral(@"  myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=""#4F7942"";               
              }

            });  

        myChart.update();
                  
        }); 
    
};

loadData()

//setInterval(function() {
//     loadData()  
//}, 5000);

</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ServerHealthDetails>> Html { get; private set; }
    }
}
#pragma warning restore 1591
