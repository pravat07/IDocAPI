using IDocAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IDocAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocsController : ApiController
    {
        public string Get()
        {
            return "OK";
        }
        // GET: api/Docs/5
        public async Task<IHttpActionResult> Post(Symtops model)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                //var postData = new Dictionary<string, int>() {
                //    {"AGE",24 },
                //    {"WEIGHT",64 },
                //    {"NORMAL TEMPERATURE",97 },
                //    {"CURRENT TEMPERATURE",101 },
                //    {"SEX",1 }

                //};

                var postData = new
                {
                    Inputs = new Dictionary<string, StringTable>()
                    {
                        {
                            "input1",new StringTable()
                                {
                                    ColumnNames = new string[] {"AGE", "WEIGHT", "NORMAL TEMPERATURE", "CURRENT TEMPERATURE", "SEX", "MALARIA", "Column 6"},
                                    Values = new string[,] {  { model.Age, model.Weight, model.NormalTemperature, model.CurrentTemperature, model.Gender, "0", "value" },  }
                                }
                        }
                    },
                    GlobalParameters = new Dictionary<string, string>() { }
                };

                const string apiKey = "m3m4BLVAuOjIiPuLM6rsD1clk0Hur6Z34bUzQaJdnRl76V7Mk4LTAj9b7Bh6bY++r5QUWiPkPuh2JsFmUA4gIA=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/8ed9fc9dd3c1422c9417035f14aae296/services/3c3ce96bed1c46f0a6eba1043e801f92/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                HttpResponseMessage response = await client.PostAsJsonAsync("", postData);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine("Result: {0}", result);
                    //var serializer = new JsonConvert();
                    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result);
                    var values = jsonObject.Results.output1.value.Values[0];
                    var mal = values[5];
                    var mean = values[6];
                    var deviation = values[7];
                    var obj = new object();
                    if ((float)mean > 50)
                    {
                        obj = new { Message = "You are most likely having fever." };
                    }
                    else
                    {
                        obj = new { Message = "You are having Malaria." };
                    }
                    return Ok(obj);
                }
                else
                {
                    //Console.WriteLine("Failed with status code: {0}", response.StatusCode);
                    result = "Sorry something went wrong";
                }
            }
            return Ok(result);
        }
    }
}
