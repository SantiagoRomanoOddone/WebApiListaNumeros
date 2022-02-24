using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.ExceptionHandler;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;
using System.IO;

namespace XUnitTesting
{
    public class ExceptionMiddlewareTest 
    {

        Mock<RequestDelegate> _next ;
        Mock<Middlewares.ExceptionHandler.IExceptionFilter> _exceptionFilter;
        DefaultHttpContext _context;
        public ExceptionMiddlewareTest()
        {
            _context = new DefaultHttpContext();
            _next = new Mock<RequestDelegate>();
            _exceptionFilter = new Mock<IExceptionFilter>();        
        }

        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_AppException()
        {
            var expectedException = new AppException();

            _exceptionFilter.Setup(repo => repo.SetStatusCode(_context, expectedException))
               .Returns(Task.FromException(expectedException));
    
            _next.SetReturnsDefault(Task.FromException(expectedException));
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);

            //Act
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<AppException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_UnauthorizedAccessException()
        {
            var expectedException1 = new UnauthorizedAccessException();

            _exceptionFilter.Setup(repo => repo.SetStatusCode(_context, expectedException1))
               .Returns(Task.FromException(expectedException1));

            _next.SetReturnsDefault(Task.FromException(expectedException1));
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);

            //Act
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<UnauthorizedAccessException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_KeyNotFoundException()
        {
            var expectedException2 = new KeyNotFoundException();

            _exceptionFilter.Setup(repo => repo.SetStatusCode(_context, expectedException2))
               .Returns(Task.FromException(expectedException2));

            _next.SetReturnsDefault(Task.FromException(expectedException2));
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);

            //Act
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<KeyNotFoundException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_ArgumentNullException()
        {
            var expectedException3 = new ArgumentNullException();

            _exceptionFilter.Setup(repo => repo.SetStatusCode(_context, expectedException3))
               .Returns(Task.FromException(expectedException3));

            _next.SetReturnsDefault(Task.FromException(expectedException3));
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);

            //Act
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<ArgumentNullException>();
        }
    }
}
