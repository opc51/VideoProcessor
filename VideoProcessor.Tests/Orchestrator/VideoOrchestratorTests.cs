using FluentAssertions;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Moq;
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

            var loggerMoq = new Mock<ILogger>();

            var orchestratorContext = new Mock<IDurableOrchestrationContext>();

            orchestratorContext
                .Setup(o => o.GetInput<string>())
                .Returns(videoLocation);

            orchestratorContext
                .Setup(x => x.CallActivityAsync<string>("TranscodeVideo", videoLocation))
                .ReturnsAsync($"{videoLocation}-transcoded")
                .Verifiable();

            orchestratorContext
                .Setup(x => x.CallActivityAsync<string>("ExtractThumbnail", transcodedLocation))
                .ReturnsAsync($"{videoLocation}-thumbnail")
                .Verifiable();

            orchestratorContext
                .Setup(x => x.CallActivityAsync<string>("PrependIntro", thumbnailLocation))
                .ReturnsAsync($"{videoLocation}-prepended")
                .Verifiable();

            var result = await VideoOrchestrator.ProcessRawVideo(orchestratorContext.Object, loggerMoq.Object);

            result.Should().BeOfType<VideoLocations>();

            orchestratorContext.Verify(x => x.CallActivityAsync<string>(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(3));

            orchestratorContext.Verify(x => x.CallActivityAsync<string>("TranscodeVideo", videoLocation), Times.Once());

            orchestratorContext.Verify(x => x.CallActivityAsync<string>("ExtractThumbnail", transcodedLocation), Times.Once());

            orchestratorContext.Verify(x => x.CallActivityAsync<string>("PrependIntro", thumbnailLocation), Times.Once());

            result.TranscodeLocation.Should().Be(transcodedLocation);
            result.ThumbnailLocation.Should().Be(thumbnailLocation);
            result.CompletedLocation.Should().Be(prependLocation);
        }
    }
}
