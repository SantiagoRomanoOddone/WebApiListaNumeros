using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.FunctionalityHandler;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using XUnitTesting.Responses;

namespace XUnitTesting
{
    public class FunctionalityMiddlewareTest
    {
        Mock<RequestDelegate> _next;
        Mock<ICacheProvider> _cacheprovider;
        DefaultHttpContext _context;

        public FunctionalityMiddlewareTest()
        {
            _context = new DefaultHttpContext();
            _next = new Mock<RequestDelegate>();
            _cacheprovider = new Mock<ICacheProvider>();
        }

        [Fact]
        public async Task RequestWithPathOk_ReturNextInvoke()
        {
            _context.Request.Headers["Channel"] = "sucursal";
            _context.Request.Method = "GET";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _context.Request.Path = "/v1/minipompom/basic/list";

            _cacheprovider.Setup(x => x.FunctionalityCheckAsync(_context))
                .Returns(Task.CompletedTask);

            var functionalityMiddleware = new FunctionalityMiddleware(_next.Object, _cacheprovider.Object);

            //Act
            Func<Task> function = async () => { await functionalityMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task RequestWithWrongPath_ReturFunctionalityException()
        {
            _context.Request.Headers["Channel"] = "mocknotfound";
            _context.Request.Method = "GET";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _context.Request.Path = "/v1/minipompom/basic/list";

            _cacheprovider.Setup(x => x.FunctionalityCheckAsync(_context))
                .Returns(Task.FromException(new ArgumentNullException()));

            var functionalityMiddleware = new FunctionalityMiddleware(_next.Object, _cacheprovider.Object);

            //Act
            Func<Task> function = async () => { await functionalityMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().Throw<ArgumentNullException>();

        }
    }
}
