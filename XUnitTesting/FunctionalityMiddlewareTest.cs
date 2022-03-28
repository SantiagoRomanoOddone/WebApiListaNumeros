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
