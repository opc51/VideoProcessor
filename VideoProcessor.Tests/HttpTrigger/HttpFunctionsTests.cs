using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace VideoProcessor.Tests.HttpTrigger
{
    public class HttpFunctionsTests
    {
        [Test]
        public async Task ProcessRawVideo_NoParameters_ThrowsBadRequest()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var logger = Substitute.For<ILogger>(); ;
            var starter = Substitute.For<IDurableClient>();

            var request = Substitute.For<HttpRequest>();
            request.Query.Returns(new QueryCollection());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                request,
                starter,
                logger);

            result.Should().BeOfType<BadRequestObjectResult>();

            await starter.DidNotReceive().StartNewAsync(ORCHESTRATORNAME, string.Empty, Arg.Any<string>());
            starter.DidNotReceive().CreateCheckStatusResponse(Arg.Any<HttpRequest>(), INSTANCEID, false);
        }


        [Test]
        public async Task ProcessRawVideo_ParameterWithOutValue_ThrowsBadRequest()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var logger = Substitute.For<ILogger>();
            var starter = Substitute.For<IDurableClient>();

            var request = Substitute.For<HttpRequest>();
            request
                .Query
                .Returns(new QueryCollection());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                request,
                starter,
                logger);

            result.Should().BeOfType<BadRequestObjectResult>();
            await starter.DidNotReceive().StartNewAsync(ORCHESTRATORNAME, string.Empty, Arg.Any<string>());
            starter.DidNotReceive().CreateCheckStatusResponse(Arg.Any<HttpRequest>(), INSTANCEID, false);
        }


        [Test]
        public async Task ProcessRawVideoStarter_HappyPath()
        {
            const string ORCHESTRATORNAME = "ProcessRawVideoOrchestrator";
            const string INSTANCEID = "03488157-3ddc-4aac-848b-c5fbe4789555";

            var logger = Substitute.For <ILogger>();
            var starter = Substitute.For<IDurableClient>();

            var request = Substitute.For<HttpRequest>();
            
            var queryStringCollection = new Dictionary<string, StringValues> { 
                { "video", "MyVideo" } 
            };

            request
                .Query
                .Returns(new QueryCollection(queryStringCollection));

            starter
                .StartNewAsync(ORCHESTRATORNAME, null, "MyVideo")
                .Returns(INSTANCEID);

            starter
                .CreateCheckStatusResponse(Arg.Any<HttpRequest>(), INSTANCEID, false)
                .Returns(new OkResult());

            var result = await HttpFunctions.VideoHttpFunctions.Run(
                request,
                starter,
                logger
            );

            result.Should().BeOfType<OkResult>();

            await starter.Received().StartNewAsync(Arg.Any<string>(), Arg.Any<string>(), "MyVideo");
            starter.Received().CreateCheckStatusResponse(Arg.Any<HttpRequest>(), Arg.Any<string>(), false);
        }
    }
}
