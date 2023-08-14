using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VideoProcessor.DTO.OutputDtos;
using VideoProcessor.Orchestrators;

namespace VideoProcessor.Tests.Orchestrator
{
    public class VideoOrchestratorTests
    {
        [Test]
        public async Task ProcessRawVideo_HappyPath()
        {
            const string videoLocation = "VideoLocation";
            const string transcodedLocation = $"{videoLocation}-transcoded";
            const string thumbnailLocation = $"{videoLocation}-thumbnail";
            const string prependLocation = $"{videoLocation}-prepended";

            var logger = Substitute.For<ILogger>();

            var orchestratorContext = Substitute.For<IDurableOrchestrationContext>();

            orchestratorContext
                .GetInput<string>()
                .Returns(videoLocation);

            orchestratorContext
                .CallActivityAsync<string>("TranscodeVideo", videoLocation)
                .Returns($"{videoLocation}-transcoded");

            orchestratorContext
                .CallActivityAsync<string>("ExtractThumbnail", transcodedLocation)
                .Returns($"{videoLocation}-thumbnail");

            orchestratorContext
                .CallActivityAsync<string>("PrependIntro", thumbnailLocation)
                .Returns($"{videoLocation}-prepended");

            var result = await VideoOrchestrator.ProcessRawVideo(orchestratorContext, logger);

            result.Should().BeOfType<VideoLocations>();

            await orchestratorContext
                .Received()
                .CallActivityAsync<string>(Arg.Any<string>(), Arg.Any<string>());

            await orchestratorContext
                .Received()
                .CallActivityAsync<string>("TranscodeVideo", videoLocation);

            await orchestratorContext
                .Received()
                .CallActivityAsync<string>("ExtractThumbnail", transcodedLocation);

            await orchestratorContext
                .Received()
                .CallActivityAsync<string>("PrependIntro", thumbnailLocation);

            result.TranscodeLocation.Should().Be(transcodedLocation);
            result.ThumbnailLocation.Should().Be(thumbnailLocation);
            result.CompletedLocation.Should().Be(prependLocation);
        }
    }
}
