using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TextCheck
{
    public static class Function1
    {
        /// <summary>
        /// This method checks if text in the body satisfies exam condition.
        /// </summary>
        /// <param name="req">HTTP request body</param>
        /// <param name="log">Logger object</param>
        /// <returns></returns>
        [FunctionName("TextCheck")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Text check initiated");

            string text = req.Query["text"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            text = text
                   ?? data?.text;

            var sentences = text.Split('.').Length;
            var res = sentences >= 3 && text.Length >= 10;
            return res ? (ActionResult) new OkObjectResult("appropriate") 
                : new OkObjectResult("inappropriate");

        }
    }
}
