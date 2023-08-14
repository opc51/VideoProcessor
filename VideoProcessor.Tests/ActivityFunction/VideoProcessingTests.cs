using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VideoProcessor.ActivityFunctions;

namespace VideoProcessor.Tests.ActivityFunction
{
    public class VideoProcessingTests
    {
        [Test]
        public async Task TranscodeVideo_HappyPath()
        {
            var loggerMoq = new Mock<ILogger>();

            var sut = await VideoProcessing.TranscodeVideo("MyVideo", loggerMoq.Object);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--transcoded");
        }


        [Test]
        public async Task ExtractThumbnail_HappyPath()
        {
            var loggerMoq = new Mock<ILogger>();

            var sut = await VideoProcessing.ExtractThumbnail("MyVideo", loggerMoq.Object);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--ExtractThumbnail");
        }


        [Test]
        public async Task PrependIntro_HappyPath()
        {
            var loggerMoq = new Mock<ILogger>();

            var sut = await VideoProcessing.PrependIntro("MyVideo", loggerMoq.Object);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--PrependIntro");
        }
    }
}
