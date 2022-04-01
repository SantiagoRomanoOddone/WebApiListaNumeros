using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.ExceptionHandler;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

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
            //Arrange
            var expectedException = new AppException();
            _exceptionFilter.Setup(repo => repo.SetStatusCodeAsync(expectedException))
               .Returns(Task.FromException(expectedException));
            _next.SetReturnsDefault(Task.FromException(expectedException));

            //Act
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().Throw<AppException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_UnauthorizedAccessException()
        {
            //Arrange
            var expectedException1 = new UnauthorizedAccessException();
            _exceptionFilter.Setup(repo => repo.SetStatusCodeAsync(expectedException1))
               .Returns(Task.FromException(expectedException1));
            _next.SetReturnsDefault(Task.FromException(expectedException1));

            //Act
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().Throw<UnauthorizedAccessException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_KeyNotFoundException()
        {
            //Arrange
            var expectedException2 = new KeyNotFoundException();
            _exceptionFilter.Setup(repo => repo.SetStatusCodeAsync(expectedException2))
               .Returns(Task.FromException(expectedException2));
            _next.SetReturnsDefault(Task.FromException(expectedException2));

            //Act
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().Throw<KeyNotFoundException>();
        }
        [Fact]
        public async Task ExceptionMiddlewareTest_ShouldReturn_ArgumentNullException()
        {
            //Arrange
            var expectedException3 = new ArgumentNullException();
            _exceptionFilter.Setup(repo => repo.SetStatusCodeAsync(expectedException3))
               .Returns(Task.FromException(expectedException3));
            _next.SetReturnsDefault(Task.FromException(expectedException3));

            //Act
            var exceptionMiddleware = new ExceptionMiddleware(_next.Object, _exceptionFilter.Object);
            Func<Task> function = async () => { await exceptionMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<ArgumentNullException>();
        }
    }
}
