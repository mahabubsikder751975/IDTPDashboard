#pragma checksum "C:\IDTPDashboard\Views\Home\ICPNetworkMonitor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "541dbdfbd7219a365eccb9c82e48ebff8a31b92a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ICPNetworkMonitor), @"mvc.1.0.view", @"/Views/Home/ICPNetworkMonitor.cshtml")]
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
#line 1 "C:\IDTPDashboard\Views\_ViewImports.cshtml"
using IDTPDashboards;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\IDTPDashboard\Views\_ViewImports.cshtml"
using IDTPDashboards.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"541dbdfbd7219a365eccb9c82e48ebff8a31b92a", @"/Views/Home/ICPNetworkMonitor.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1de202496fa847446cb7ebfb3e3a55cf1c7d1a5e", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ICPNetworkMonitor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n\n");
            WriteLiteral("\n");
            WriteLiteral(@"
<div class=""container"">
    <canvas id=""forceDirectedGraph""></canvas>
</div>

<script>  

var myChart;

createNetChart = function () {
                $.ajax({
                url: '/Home/GetICPNetChartData',
                type: 'POST',        
                dataType: 'json',
                contentType: 'application/json',
                success: function (icpdata) {     
                    originalpointcolor = new Array(icpdata.length).fill(""#66ff00"");
                    copiedpointcolor = JSON.parse(JSON.stringify(originalpointcolor));
                    createChart(icpdata, ""forceDirectedGraph"", ""forceDirectedGraph"", ""vertical"");
                }
            });
        };

createNetChart();

let originalpointcolor=[];
let copiedpointcolor;

function createChart(nodes, id, type, orientation) {    
  myChart=new Chart(document.getElementById(id).getContext(""2d""), {
    type,
    data: {
      labels: nodes.map((d) => d.name),
      datasets: [
        {          
          pointRadius: 20,
      ");
            WriteLiteral(@"    pointHoverRadius: 30,
          data: nodes.map((d) => Object.assign({}, d)),
          pointBackgroundColor: copiedpointcolor
        }
      ]
    },
    options: {
      dragData: true,
      dragX: true,
      tree: {
        orientation
      },
      layout: {
        padding: {
          top: 5,
          left: 5,
          right: 5,
          bottom: 5
        }
      },
      plugins: {
        tooltip:false,
        title: {
          display: true,
          text: ['ICP Network with IDTP Server',''],   
          padding: {
                    top: 10,
                    bottom: 30
                },                           
        },
        legend: {
          display: false
        },
        datalabels: {
          align: orientation === ""vertical"" ? ""bottom"" : ""right"",
          offset: 6,
          backgroundColor: ""white"",          
          formatter: (v) => {
            return v.name;
          }
        }
      },    
    animation: {
    onComplete: function() {
        let ctx");
            WriteLiteral(@" = this.$context.chart.ctx;
        ctx.textAlign = ""center"";
        ctx.textBaseline = ""middle"";        
        let chart = this;
        let datasets = this.config.data.datasets;
        let sum=new Array();
        datasets.forEach(function (dataset, i) {
            ctx.font = ""Bold 16px Helvetica"";
            switch ( chart.getDatasetMeta(i).type ) {
              case ""forceDirectedGraph"":
                    ctx.fillStyle = ""#4E4E4C"";                    
                    chart.getDatasetMeta(i).data.forEach(function (p, j) {
                        ctx.fillText(datasets[i].data[j].name, p.x, p.y);
                    });
                    break;
                case ""line"":
                    ctx.fillStyle = ""Black"";
                    chart.getDatasetMeta(i).data.forEach(function (p, j) {
                        ctx.fillText(datasets[i].data[j], p.x, p.y - 20);
                    });
                    break;
                case ""bar"":
                    ctx.fillStyle = ""White"";
        ");
            WriteLiteral(@"            chart.getDatasetMeta(i).data.forEach(function (p, j) {
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
  });
}
Chart.defaults.font.family='Helvetica'
Chart.defaults.font.size=18

loadData = function () {

 $.ajax({
        url: '/Home/GetICPServerData',
        type: 'POST',        
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {     
            // loop through the All data and fill the dropdown
            $.each(dat");
            WriteLiteral(@"a, function (index, item) {

              if(item.isHelloTested==false){
                myChart.config.data.datasets[0]['pointBackgroundColor'][index] = '#f2f1ed'; 
              }
              else{
                 myChart.config.data.datasets[0]['pointBackgroundColor'][index] = originalpointcolor[index];                
              }              


             // if(item.serverHeartbeat==false){
             //   myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=""#CC7722"";                
             // }
             // else{
             //   myChart.config.data.datasets[item.serverRackId].backgroundColor[item.rackId]=copieddatasets[item.serverRackId].backgroundColor[item.rackId];                
             // }

            });  
            myChart.update();
                     
        }
    });
};

//loadData()

setInterval(function() {
    loadData()  
}, 5000);

 </script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
