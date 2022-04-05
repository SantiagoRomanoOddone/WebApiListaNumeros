using FluentAssertions;
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

        [Fact]
        public async Task DisponibilityCheckTest_Should_NotThrowException()
        {
            //Arrange
            Root _root = new Root();
            var mock = new Mock<Root>();
            _root = mock.Object;
            _root = JsonConvert.DeserializeObject<Root>(MockResponses.FunctionalityResponse.RESPONSE_OK);

            //Act
            var disponibilityFilter = new DisponibilityFilter();
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

            //Act
            var disponibilityFilter = new DisponibilityFilter();
            Func<Task> function = async () => { await disponibilityFilter.DisponibilityCheckAsync(_root); };

            //Assert
            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }

    }
}
