using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using VideoProcessor.ActivityFunctions;

namespace VideoProcessor.Tests.ActivityFunction
{
    public class VideoProcessingTests
    {
        [Test]
        public async Task TranscodeVideo_HappyPath()
        {
            var logger = Substitute.For<ILogger>();

            var sut = await VideoProcessing.TranscodeVideo("MyVideo", logger);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--transcoded");
        }


        [Test]
        public async Task ExtractThumbnail_HappyPath()
        {
            var logger = Substitute.For<ILogger>();

            var sut = await VideoProcessing.ExtractThumbnail("MyVideo", logger);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--ExtractThumbnail");
        }


        [Test]
        public async Task PrependIntro_HappyPath()
        {
            var logger = Substitute.For<ILogger>();

            var sut = await VideoProcessing.PrependIntro("MyVideo", logger);

            sut.Should().NotBeNull();
            sut.Should().Be("MyVideo--PrependIntro");
        }
    }
}
