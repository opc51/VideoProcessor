using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace VideoProcessor.ActivityFunctions
{
    /// <summary>
    /// The activity functions used to process a raw video into release format
    /// </summary>
    public static class VideoProcessorActivityFunctions
    {
        // To do - depency injection needs to be puut into these Activity Functions


        /// <summary>
        /// Put the video into the correct format
        /// </summary>
        /// <param name="inputVideo">Details of the video provided</param>
        /// <param name="log">The <see cref="ILogger"/> used to log messages </param>
        /// <returns>The location of the video in the correct <see cref="string"/> format</returns>
        [FunctionName(nameof(TranscodeVideo))]
        public static async Task<string> TranscodeVideo([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Inside {nameof(TranscodeVideo)} to {inputVideo}.");
            await Task.Delay(2000);
            return $"{inputVideo}--transcoded.mp4";
        }

        /// <summary>
        /// Extracts a thumbnail from the video
        /// </summary>
        /// <param name="inputVideo">Details of the video provided</param>
        /// <param name="log">The <see cref="ILogger"/> used to log messages </param>
        /// <returns>The <see cref="string"/> location of the video thumbnail</returns>
        [FunctionName(nameof(ExtractThumbnail))]
        public static async Task<string> ExtractThumbnail([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Inside {nameof(ExtractThumbnail)} to {inputVideo}.");
            await Task.Delay(2000);
            return $"{inputVideo}--ExtractThumbnail.mp4";
        }

        /// <summary>
        /// Adds a standard 5 second logo onto the beginning of a video
        /// </summary>
        /// <param name="inputVideo">Details of the video provided</param>
        /// <param name="log">The <see cref="ILogger"/> used to log messages </param>
        /// <returns>The <see cref="string"/> location of the prepended video </returns>
        [FunctionName(nameof(PrependIntro))]
        public static async Task<string> PrependIntro([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Inside {nameof(PrependIntro)} to {inputVideo}.");
            await Task.Delay(2000);
            return $"{inputVideo}--PrependIntro.mp4";
        }
    }
}
