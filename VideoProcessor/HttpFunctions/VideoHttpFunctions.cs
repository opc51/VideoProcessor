using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VideoProcessor.HttpFunctions
{
    /// <summary>
    /// The endpoint that is called to begin Video Processing commands
    /// </summary>
    public static class VideoHttpFunctions
    {

        /// <summary>
        /// The endpoint that is used to begin the workflow to proceses a raw video into release format
        /// 
        /// This endpoint not require authorisation. It accepts GET requests
        /// </summary>
        /// <param name="request">The incoming request of type <see cref="HttpRequest"/></param>
        /// <param name="starter"> The <see cref="IDurableOrchestrationClient"/> used to trigger the workflow</param>
        /// <param name="log">The <see cref="ILogger"/> used for logging</param>
        /// <returns></returns>
        [FunctionName("ProcessVideoStarter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest request,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string video;

            try
            {
                video = request.GetQueryParameterDictionary()["video"];
                if (string.IsNullOrEmpty(video))
                    return new BadRequestObjectResult("The parameter video cannot be empty");
            }
            catch(Exception)
            {
                return new BadRequestObjectResult("Missing parameter called 'video' missing from query string");
            }

            string instanceId = await starter.StartNewAsync("ProcessRawVideoOrchestrator", null, video);

            log.LogInformation($"HttpStart. Instance Id = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(request, instanceId);
        }



    }
}
