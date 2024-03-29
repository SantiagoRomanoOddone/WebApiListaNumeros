﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Middlewares.FunctionalityHandler;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
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
        Mock<IHttpContextAccessor> _httpContextAccessor;
        public FunctionalityCheckTest()
        {
            _clientFactory = new Mock<IHttpClientFactory>();
            _memoryCache = new Mock<IMemoryCache>();
            _context = new DefaultHttpContext();
            _cacheEntry = new Mock<ICacheEntry>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task FunctionalityCheckTest_Should_ThrowException()
        {
            //Arrange
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);
            _memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_cacheEntry.Object);

            Environment.SetEnvironmentVariable("urlMock", "http://localhost:8080/");
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
            var cacheProvider = new CacheProvider(_clientFactory.Object, _memoryCache.Object, _httpContextAccessor.Object);
            Func<Task> function = async () => { await cacheProvider.FunctionalityCheckAsync(); };

            //Assert
            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task FunctionalityCheckTest_Should_NotThrowException()
        {
            //Arrange
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BEARER_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);
            _memoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
            .Returns(_cacheEntry.Object);

            Environment.SetEnvironmentVariable("urlMock", "http://localhost:8080/");
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

            //Act
            var cacheProvider = new CacheProvider(_clientFactory.Object, _memoryCache.Object, _httpContextAccessor.Object);
            Func<Task> function = async () => { await cacheProvider.FunctionalityCheckAsync(); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
    }
}
