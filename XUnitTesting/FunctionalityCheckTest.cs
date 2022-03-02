using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.FunctionalityHandler;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTesting
{
    public class FunctionalityCheckTest
    {
        Mock<IHttpClientFactory> _clientFactory;
        //Mock<IFunctionalityFilter> _functionalityFilter;
        Mock<IMemoryCache> _memoryCache;
        DefaultHttpContext _context;      

        public FunctionalityCheckTest()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            //_functionalityFilter = new Mock<IFunctionalityFilter>();
            _memoryCache = new Mock<IMemoryCache>();
            _context = new DefaultHttpContext();
        }

        [Fact]
        public async Task FunctionalityChekTest_Should_ThrowException()
        {        
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/lista";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{hola}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);            
            var functionabilityFilter = new FunctionalityFilter(_clientFactory.Object, _memoryCache.Object);

            Func<Task> function = async () => { await functionabilityFilter.FunctionalityCheck(_context);} ;

            //Assert
            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task FunctionalityChekTest_Should_NotThrowException()
        {
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/lista";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
            var functionabilityFilter = new FunctionalityFilter(_clientFactory.Object, _memoryCache.Object);

            Func<Task> function = async () => { await functionabilityFilter.FunctionalityCheck(_context); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
    }
}
