using hsm_api.Domain.StartProduction;
using hsm_api.Models;
using hsm_api.Models.Messages;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace hsm_api_test.Domain.StartProduction
{
    public class StartProductionHttpMessageSender_PostAsync
    {
        [Fact]
        public async void PostAsync_To_Callback_Url()
        {
            var message = new StartProductionMessage();
            var expectedCallbackUrl = "http://localhost/test";
            var subscriber = new Webhook { CallbackUrl = expectedCallbackUrl };
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var client = new HttpClient(mockMessageHandler.Object);
            var sender = new StartProductionHttpMessageSender(client, Mock.Of<ILogger<StartProductionHttpMessageSender>>());

            mockMessageHandler.Protected()
                              .Setup<Task<HttpResponseMessage>>("SendAsync", 
                                                                ItExpr.Is<HttpRequestMessage>(x => x.RequestUri.AbsoluteUri == expectedCallbackUrl),
                                                                ItExpr.IsAny<CancellationToken>())
                              .ReturnsAsync(new HttpResponseMessage());

            await sender.PostAsync(message, subscriber);
        }
    }
}
