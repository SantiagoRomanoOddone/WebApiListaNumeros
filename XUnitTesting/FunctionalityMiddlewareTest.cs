using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Middlewares;
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
    public class FunctionalityMiddlewareTest
    {
        Mock<RequestDelegate> _next;
        Mock<IFunctionalityFilter> _functionalityFilter;
        DefaultHttpContext _context;

        public FunctionalityMiddlewareTest()
        {
            _context = new DefaultHttpContext();
            _next = new Mock<RequestDelegate>();
            _functionalityFilter = new Mock<IFunctionalityFilter>();
        }

        [Fact]
        public async Task FunctionalityMiddlewareTest_MockService_Should_NotThrowException()
        {          
            Mock<IHttpClientFactory> _clientFactory = new Mock<IHttpClientFactory>();
            Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
            Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

            _configuration.Object["UrlMock:url"] = "https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.io";
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Path = "/v1/minipompom/basic/list";
            var client = new HttpClient();
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
            _functionalityFilter.Setup(x => x.FunctionalityCheck(_context, _clientFactory.Object, _memoryCache.Object, _configuration.Object))
                .Returns(Task.CompletedTask);

            var functionalityMiddleware = new FunctionalityMiddleware(_next.Object, _functionalityFilter.Object);

            //Act
            Func<Task> function = async () => { await functionalityMiddleware.InvokeAsync(_context, _clientFactory.Object, _memoryCache.Object, _configuration.Object); };

            //Assert
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task FunctionalityMiddlewareTest_MockService_Should_ThrowException()
        {
            Mock<IHttpClientFactory> _clientFactory = new Mock<IHttpClientFactory>();
            Mock<IMemoryCache> _memoryCache = new Mock<IMemoryCache>();
            Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

            _configuration.Object["UrlMock:url"] = "https://be2d9e2a-4c0f-41bb-ab02-b8731ec4654c.mock.pstmn.ioAAAAAAAAA";
            _context.Request.Headers["Channel"] = "sAAAAAAAAAAAA";
            _context.Request.Method = "GAAAAAAAAAAAA";
            _context.Request.Path = "/v1/minipompom/basic/listAAAAAAAAAAAA";
            var client = new HttpClient();
            _clientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            _functionalityFilter.Setup(x => x.FunctionalityCheck(_context, _clientFactory.Object, _memoryCache.Object, _configuration.Object))
                .Returns(Task.FromException(new Exception()));

            var functionalityMiddleware = new FunctionalityMiddleware(_next.Object, _functionalityFilter.Object);

            //Act
            Func<Task> function = async () => { await functionalityMiddleware.InvokeAsync(_context, _clientFactory.Object, _memoryCache.Object, _configuration.Object); };

            //Assert  
            function.Should().Throw<Exception>();
        }
    }
}
