using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Middlewares.SecurityDisponibilityHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitTesting.Responses;

namespace XUnitTesting
{
    public class DisponibilityCheckTest
    {
        DefaultHttpContext _context;

        public DisponibilityCheckTest()
        {
            _context = new DefaultHttpContext();
        }
        [Fact]
        public async Task DisponibilityCheckTest_Should_NotThrowException()
        {

            _context.Items["functionality-response"] = MockResponses.FunctionalityResponse.RESPONSE_OK;
            var functionabilityFilter = new DisponibilityFilter();

            Func<Task> function = async () => { await functionabilityFilter.DisponibilityCheck(_context); };

            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task DisponibilityCheckTest_Should_ThrowException()
        {

            _context.Items["functionality-response"] = MockResponses.FunctionalityResponse.RESPONSE_NOT_OK;
            var functionabilityFilter = new DisponibilityFilter();

            Func<Task> function = async () => { await functionabilityFilter.DisponibilityCheck(_context); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }

    }
}
