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
        public async Task RequestWithBasicAuth_BasicCredentialsOkOnDisponibilityRange_ReturnOk()
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
        public async Task RequestWithBasicAuth_BasicCredentialsNotOkOnDisponibilityRange_ReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_NOTOK);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsOkOnDisponibilityRange_ReturnOk()
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
        public async Task RequestWithBearerAuth_BearerCredentialsNotOkOnDisponibilityRange_ReturnUnauthorized()
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_NOTOK);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task RequestWithBasicAuth_BasicCredentialsOkOutOfDisponibilityRange_ReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_OK);
            _client.DefaultRequestHeaders.Add("channel", "mock");
            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsOkOutOfDisponibilityRange_ReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_OK);
            _client.DefaultRequestHeaders.Add("channel", "mock");
            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert         
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task RequestWithBasicAuth_BasicCredentialsOk_FunctionalityResponseNotFound_ReturnInternalServerError()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", MockResponse.SecurityResponse.RESPONSE_BASIC_OK);
            _client.DefaultRequestHeaders.Add("channel", "mocknotfound");
            // Act
            var response = await _client.GetAsync("v1/minipompom/basic/list");

            // Assert         
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsOk_FunctionalityResponseNotFound_ReturnInternalServerError()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_OK);
            _client.DefaultRequestHeaders.Add("channel", "mocknotfound");
            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");

            // Assert         
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsNotOk_ReturnUnauthorizedWithMessage_MustHaveThreeSegments()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_NOTOK_ONE_ARGUMENT);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");
           
            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");
            var responseString = await response.Content.ReadAsStringAsync();
            
            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal("{\"message\":\"IDX12741: JWT: 'System.String' must have three segments (JWS) or five segments (JWE).\"}", responseString);
        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsNotOk_ReturnUnauthorizedWithMessage_LifetimeValidationFailed()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_NOTOK);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            Assert.Equal("{\"message\":\"IDX10223: Lifetime validation failed. The token is expired. ValidTo: 'System.DateTime', Current time: 'System.DateTime'.\"}", responseString);
        }
        [Fact]
        public async Task RequestWithBearerAuth_BearerCredentialsNotOk_ReturnUnauthorizedWithMessage_SignatureValidationFailed()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MockResponse.SecurityResponse.RESPONSE_BEARER_NOTOK_WRONG_SIGNATURE);
            _client.DefaultRequestHeaders.Add("channel", "sucursal");

            // Act
            var response = await _client.GetAsync("v1/minipompom/jwt/list");
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert                    
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            Assert.Equal("{\"message\":\"IDX10503: Signature validation failed. Keys tried: 'System.Text.StringBuilder'.\\nExceptions caught:\\n 'System.Text.StringBuilder'.\\ntoken: 'System.IdentityModel.Tokens.Jwt.JwtSecurityToken'.\"}", responseString);
        }
    }
}
