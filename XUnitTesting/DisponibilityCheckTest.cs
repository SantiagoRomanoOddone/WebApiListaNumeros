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
    public class DisponibilityCheckTest
    {
        DefaultHttpContext _context;
        Mock<IHttpContextAccessor> _httpContextAccessor;

        public DisponibilityCheckTest()
        {
            _context = new DefaultHttpContext();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task DisponibilityCheckTest_Should_NotThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_OK);
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var disponibilityFilter = new DisponibilityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await disponibilityFilter.DisponibilityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task DisponibilityCheckTest_Should_ThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_NOT_OK);
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(_context);

            //Act
            var disponibilityFilter = new DisponibilityFilter(_httpContextAccessor.Object);
            Func<Task> function = async () => { await disponibilityFilter.DisponibilityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }

    }
}
