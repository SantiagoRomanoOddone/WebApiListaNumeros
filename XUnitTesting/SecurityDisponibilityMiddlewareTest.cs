using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using Middlewares.SecurityDisponibilityHandler;
using Newtonsoft.Json;
using XUnitTesting.Responses;

namespace XUnitTesting
{
    public class SecurityDisponibilityMiddlewareTest
    {
        Mock<RequestDelegate> _next;
        Mock<ISecurityFilter> _securityFilter;
        Mock<IDisponibilityFilter> _disponibilityFilter;
        Mock<IHttpContextAccessor> _httpContextAccessor;
        Mock<Root> _root;
        DefaultHttpContext _context;

        public SecurityDisponibilityMiddlewareTest()
        {
            _context = new DefaultHttpContext();
            _next = new Mock<RequestDelegate>();
            _securityFilter = new Mock<ISecurityFilter>();
            _disponibilityFilter = new Mock<IDisponibilityFilter>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _root = new Mock<Root>();
        }

        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_DisponibilityTest_Should_ThrowException()
        {
            //Arrange
            _context.Items["functionality-response"] = MockResponses.FunctionalityResponse.RESPONSE_NOT_OK;
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);
            _securityFilter.Setup(repo => repo.SecurityCheckAsync(_root.Object));
            _disponibilityFilter.Setup(repo => repo.DisponibilityCheckAsync(_root.Object));
            _next.SetReturnsDefault(Task.FromException(new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm")));

            //Act
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _disponibilityFilter.Object, _securityFilter.Object, _httpContextAccessor.Object);           
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<UnauthorizedAccessException>().WithMessage("Unauthorized! Only Weekdays from 8 am to 10 pm");
        }
        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_SecurityTest_Should_ThrowException()
        {
            //Arrange
            _context.Items["functionality-response"] = MockResponses.FunctionalityResponse.RESPONSE_OK;
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_NOTOK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);
            _securityFilter.Setup(repo => repo.SecurityCheckAsync(_root.Object));
            _disponibilityFilter.Setup(repo => repo.DisponibilityCheckAsync(_root.Object));
            _next.SetReturnsDefault(Task.FromException(new UnauthorizedAccessException()));

            //Act
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _disponibilityFilter.Object, _securityFilter.Object, _httpContextAccessor.Object);
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<UnauthorizedAccessException>();
        }
        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_Should_InvokeNext()
        {
            //Arrange
            _context.Items["functionality-response"] = MockResponses.FunctionalityResponse.RESPONSE_OK;
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);
            _securityFilter.Setup(repo => repo.SecurityCheckAsync(_root.Object));
            _disponibilityFilter.Setup(repo => repo.DisponibilityCheckAsync(_root.Object));

            //Act
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _disponibilityFilter.Object, _securityFilter.Object, _httpContextAccessor.Object);
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().NotThrow<Exception>();
        }

    }
}
