using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;

namespace VideoProcessor.Tests.HttpTrigger
{
    public class HttpFunctionsTests
    {
        [Test]
        public async Task ProcessRawVideo_NoParameters_ThrowsBadRequest()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var loggerMoq = new Mock<ILogger>();
            var starterMOQ = new Mock<IDurableClient>();

            var requestMock = new Mock<HttpRequest>();
            requestMock
                .Setup(r => r.Query)
                .Returns(new QueryCollection());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                requestMock.Object,
                starterMOQ.Object,
                loggerMoq.Object);

            result.Should().BeOfType<BadRequestObjectResult>();

            starterMOQ.Verify(x => x.StartNewAsync(ORCHESTRATORNAME, null, It.IsAny<string>()), Times.Never);
            starterMOQ.Verify(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequest>(), INSTANCEID, false), Times.Never);
        }


        [Test]
        public async Task ProcessRawVideo_ParameterWithOutValue_ThrowsBadRequest()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var loggerMoq = new Mock<ILogger>();
            var starterMOQ = new Mock<IDurableClient>();

            var requestMock = new Mock<HttpRequest>();

            var queryStringCollection = new Dictionary<string, StringValues> {
                { "video", "" }
            };

            requestMock
                .Setup(r => r.Query)
                .Returns(new QueryCollection());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                requestMock.Object,
                starterMOQ.Object,
                loggerMoq.Object);

            result.Should().BeOfType<BadRequestObjectResult>();

            starterMOQ.Verify(x => x.StartNewAsync(ORCHESTRATORNAME, null, It.IsAny<string>()), Times.Never);
            starterMOQ.Verify(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequest>(), INSTANCEID, false), Times.Never);
        }


        [Test]
        public async Task ProcessRawVideoStarter_HappyPath()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var loggerMoq = new Mock<ILogger>();
            var starterMOQ = new Mock<IDurableClient>();

            var requestMock = new Mock<HttpRequest>();
            
            var queryStringCollection = new Dictionary<string, StringValues> { 
                { "video", "MyVideo" } 
            };

            requestMock
                .Setup(r => r.Query)
                .Returns(new QueryCollection(queryStringCollection))
                .Verifiable();

            starterMOQ
                .Setup(x => x.StartNewAsync(ORCHESTRATORNAME, null, "MyVideo"))
                .ReturnsAsync(INSTANCEID)
              .Verifiable();

            starterMOQ
                .Setup(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequest>(), INSTANCEID, false))
                .Returns(new OkResult());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                requestMock.Object,
                starterMOQ.Object,
                loggerMoq.Object
            );

            result.Should().BeOfType<OkResult>();

            starterMOQ.Verify(x => x.StartNewAsync(It.IsAny<string>(), null, It.IsAny<string>()), Times.Once);
            starterMOQ.Verify(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequest>(), It.IsAny<string>(), false), Times.Once);
        }
    }
}
