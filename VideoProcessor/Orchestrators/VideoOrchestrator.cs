using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VideoProcessor.DTO.OutputDtos;
using Log = Microsoft.Extensions.Logging;

namespace VideoProcessor.Orchestrators
{
    /// <summary>
    /// Defines the workflow associate with Video Processing tasks
    /// </summary>
    public static class VideoOrchestrator
    {
        /// <summary>
        /// The workflow involved with with recieving a video and getting into release format
        /// </summary>
        /// <param name="context">An <see cref="IDurableOrchestrationContext"/></param> that manages the workflow
        /// <param name="log">The logging of type <see cref="Log.ILogger."/></param>
        /// <returns>An output DTO of type <see cref="VideoLocations"/></returns>
        [FunctionName(nameof(ProcessRawVideo))]
        public static async Task<VideoLocations> ProcessRawVideo(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            Log.ILogger log)
        {
            // var videoLocation = context.GetInput<InputVideoDto>();
            var videoLocation = context.GetInput<string>();

            log = context.CreateReplaySafeLogger(log);
            log.LogInformation($"{nameof(ProcessRawVideo)} has started, input was found to be valid");

            var transcodedLocation = await context.CallActivityAsync<string>("TranscodeVideo", videoLocation);

            var thumbnailLocation = await context.CallActivityAsync<string>("ExtractThumbnail", transcodedLocation);

            var withIntroLocation = await context.CallActivityAsync<string>("PrependIntro", thumbnailLocation);

            return new VideoLocations() {
                TranscodeLocation = transcodedLocation,
                ThumbnailLocation = thumbnailLocation,
                CompletedLocation = withIntroLocation
            };
        }
    }
}
