using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using WebApiListaNumeros;
using WebApiListaNumeros.Controllers;
using Xunit;
using XUnitTesting.Responses;

namespace IntegrationTesting
{
    public class AuthControllerIntegrationTesting
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public AuthControllerIntegrationTesting()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
               .UseStartup<Startup>());
            _client = _server.CreateClient();
        }
        [Fact]
        public async Task BasicAuthRequest_Should_ReturnList()
        {
            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert         
            responseString.Should().NotBeNullOrEmpty();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task BearerAuthRequest_Should_ReturnList()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwiaW1wdXQtYm9keSI6IntcIm1ldGhvZFwiOlwiUE9TVFwiLFwiY2hhbm5lbFwiOlwic3VjdXJzYWxcIixcInBhdGhcIjpcIi92MS9taW5pcG9tcG9tL2p3dC9jcmVhdGlvbi9BdXRoXCJ9IiwibmJmIjoxNjQ2NjY0MDkzLCJleHAiOjE2NDY2NjUyOTMsImlhdCI6MTY0NjY2NDA5MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.5C5Tyfi7egmmYf3W7UIW3lUuAhVUpsHG7MS-HaHfzCk");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert         
            responseString.Should().NotBeNullOrEmpty();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task BearerAuthRequest_Should_ReturnUnauthorized()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IkFkbWluIiwibmJmIjoxNjQ2MjM2ODg4LCJleHAiOjE2NDYyMzgwODgsImlhdCI6MTY0NjIzNjg4OCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzOTMiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.wTQ2VHx-biD-4b0QVGT6SXTe_T7Zoun6psTgE_AcQWk");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
