using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Middlewares.Models;
using Middlewares.SecurityDisponibilityHandler;
using Moq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;
using XUnitTesting.Responses;

namespace XUnitTesting
{
    public class SecurityCheckTest
    {
        DefaultHttpContext _context;
        Mock<IHttpContextAccessor> _httpContextAccessor;

        public SecurityCheckTest()
        {
            _context = new DefaultHttpContext();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }
        [Fact]
        public async Task SecurityCheckTest_Basic_Should_NotThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_OK);
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var securityFilter = new SecurityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Basic_Should_ThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_OK);
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_NOTOK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var securityFilter = new SecurityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Bearer_Should_ThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_BEARER_OK);
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BEARER_NOTOK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var securityFilter = new SecurityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_root); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Bearer_Should_NotThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_BEARER_OK);
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BEARER_OK;
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var securityFilter = new SecurityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }

    }
}
