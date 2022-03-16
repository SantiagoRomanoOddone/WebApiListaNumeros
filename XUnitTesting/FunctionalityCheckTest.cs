using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Middlewares.FunctionalityHandler;
using Middlewares.Models;
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
using XUnitTesting.Responses;

namespace XUnitTesting
{   
    public class FunctionalityCheckTest
    {
        Mock<IHttpClientFactory> _clientFactory;
        Mock<IMemoryCache> _memoryCache;
        DefaultHttpContext _context;
        Mock<ICacheEntry> _cacheEntry;
        public FunctionalityCheckTest()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _memoryCache = new Mock<IMemoryCache>();
            _context = new DefaultHttpContext();
            _cacheEntry = new Mock<ICacheEntry>();
        }
        
        [Fact]
        public async Task FunctionalityChekTest_Should_ThrowException()
        {        
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/list";

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

            //Act
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
            _context.Request.Path = "/v1/minipompom/basic/list";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(MockResponses.FunctionalityResponse.RESPONSE_OK),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            _memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_cacheEntry.Object);

            var functionabilityFilter = new FunctionalityFilter(_clientFactory.Object, _memoryCache.Object);

            Func<Task> function = async () => { await functionabilityFilter.FunctionalityCheck(_context); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
    }
}
