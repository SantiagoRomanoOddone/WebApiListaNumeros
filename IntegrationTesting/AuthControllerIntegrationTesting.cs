using FluentAssertions;
using IntegrationTesting.Responses;
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
        public async Task BasicAuthRequest_DisponibilityCheck_ShouldBe_OK_SecurityCheck_ShouldBe_Ok()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_OK);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert         
            responseString.Should().NotBeNullOrEmpty();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task BasicAuthRequest_DisponibilityCheck_ShouldBe_OK_SecurityCheck_Should_throw_Unauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_NOTOK);

            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }
        [Fact]
        public async Task BearerAuthRequest_DisponibilityCheck_ShouldBe_OK_SecurityCheck_ShouldBe_Ok()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_OK);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert         
            responseString.Should().NotBeNullOrEmpty();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
        [Fact]
        public async Task BearerAuthRequest_DisponibilityCheck_ShouldBe_OK_SecurityCheck_Should_throw_Unauthorized()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_NOTOK);

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task BasicAuthRequest_DisponibilityCheck_Should_throw_Unauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_OK);
            _client.DefaultRequestHeaders.Add("channel", "mock");
            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task BearerAuthRequest_DisponibilityCheck_Should_throw_Unauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_OK);
            _client.DefaultRequestHeaders.Add("channel", "mock");
            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task BasicAuthRequest_FunctionalityResponse_NotFound_Should_throw_InternalServerError()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_OK);
            _client.DefaultRequestHeaders.Add("channel", "mocknotfound");
            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task BearerAuthRequest_FunctionalityResponse_NotFound_Should_throw_InternalServerError()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_OK);
            _client.DefaultRequestHeaders.Add("channel", "mocknotfound");
            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert         
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
