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
    public class SecurityCheckTest
    {
        DefaultHttpContext _context;

        public SecurityCheckTest()
        {
            _context = new DefaultHttpContext();
        }
        [Fact]
        public async Task SecurityCheckTest_Basic_Should_NotThrowException()
        {
            //Arrange
            _context.Request.Path = "/v1/minipompom/basic/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_OK;
            //Act
            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_context); };

            //Assert
            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Basic_Should_ThrowException()
        {
            _context.Request.Path = "/v1/minipompom/basic/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BASIC_NOTOK;

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_context); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Bearer_Should_ThrowException()
        {
            _context.Request.Path = "/v1/minipompom/jwt/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BEARER_NOTOK;

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_context); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Bearer_Should_NotThrowException()
        {
            _context.Request.Path = "/v1/minipompom/jwt/list";
            _context.Request.Headers["Authorization"] = MockResponses.SecurityResponse.RESPONSE_BEARER_OK;

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheckAsync(_context); };

            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }

    }
}
