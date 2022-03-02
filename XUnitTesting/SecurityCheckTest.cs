using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Middlewares.SecurityDisponibilityHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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
            _context.Request.Headers["Authorization"] = "Basic QWRtaW46QWRtaW4xMjM=";

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheck(_context); };

            Assert.NotNull(function);
            function.Should().NotThrow<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Basic_Should_ThrowException()
        {
            _context.Request.Headers["Authorization"] = "Basic QWRtaTpBZG1pbjEy";

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheck(_context); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }
        [Fact]
        public async Task SecurityCheckTest_Bearer_Should_ThrowException()
        {
            _context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwibmJmIjoxNjQ1ODExMDQ4LCJleHAiOjE2NDU4MTExMDgsImlhdCI6MTY0NTgxMTA0OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.rRUEChrSQOTsgdrrVI5JjZW7_P-O8XpkRUn7WqzFz8k";

            var securityFilter = new SecurityFilter();

            Func<Task> function = async () => { await securityFilter.SecurityCheck(_context); };

            Assert.NotNull(function);
            function.Should().Throw<Exception>();
        }

    }
}
