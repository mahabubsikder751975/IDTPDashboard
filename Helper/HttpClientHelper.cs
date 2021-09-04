using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IDTPDashboards.Helper
{
    public class HttpClientHelper
    {
        // This method is responsible for Posting JSON data to API
        public static string Post(Uri uri, string stringData)
        {
            try
            {
                // Initialization of HttpClient
                using var client = new HttpClient();

                using HttpResponseMessage response = client.PostAsync(uri, new StringContent(stringData, Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();

                // Read the response
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " Url: " + uri.AbsoluteUri);
            }
        }
        
        public  static string Get(Uri uri)
        {
            try
            {
                HttpClient client = new HttpClient();

                var response =  client.GetAsync(uri);
                string responseBody =  response.Result.Content.ReadAsStringAsync().Result;

                // // Initialization of HttpClient
                // using var client = new HttpClient();

                // using HttpResponseMessage response = await client.GetAsync(uri).Result;
                // //response.EnsureSuccessStatusCode();

                // // Read the response
                // var result = response.Content.ReadAsStringAsync().Result;

                 return responseBody;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " Url: " + uri.AbsoluteUri);
            }
        }

        
    }
}
