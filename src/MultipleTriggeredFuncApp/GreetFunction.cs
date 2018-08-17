using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace MultipleTriggeredFuncApp
{
    /// <summary>
    /// Sample HTTP trigger to test locally without 
    /// </summary>
    public static class HttpSampleFunction
    {
        [FunctionName(nameof(Greet))]
        public static async Task<HttpResponseMessage> Greet([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            return await DoGreet(req, log);
        }

        [FunctionName(nameof(GreetAlt))]
        public static async Task<HttpResponseMessage> GreetAlt([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            return await DoGreet(req, log);
        }

        private static async Task<HttpResponseMessage> DoGreet(HttpRequestMessage req, TraceWriter log, [CallerMemberName]string functionName = null)
        {
            log.Info($"C# HTTP trigger function processed a request named: {functionName}.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                name = data?.name;
            }

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
