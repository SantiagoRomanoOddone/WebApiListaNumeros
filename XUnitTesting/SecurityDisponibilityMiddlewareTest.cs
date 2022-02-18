using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.IO;
using Moq;

namespace XUnitTesting
{
    
    public class SecurityDisponibilityMiddlewareTest
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;


        [Fact]
        public async Task InvokeOnSunday_ThrowUnauthorizedAccessException()
        {


        }



        [Fact]

        //theory inline data
        public async Task SecurityDisponibilityMiddlewareTest_Disponibility_Security()
        {

            var itemss = new Root
            {
                data =
                {
                    channel = "sucursal",
                    endpoint = "/v1/minipompom/basic/list",
                    method = "GET",
                    availability =
                    {
                        business_hours =
                        {
                            includes_holidays = true,
                            includes =
                            {
                                [0] =
                                {
                                    weekday = "mon",
                                    from_hour = "08:00",
                                    to_hour = "22:00"
                                },
                                [1] =
                                {
                                    weekday = "tue",
                                    from_hour = "08:00",
                                    to_hour = "22:00"
                                },
                            },
                            message =
                            {
                                title = "",
                                detail = null
                            }
                        },
                        out_of_service_list = {}
                    },
                    config =
                    {
                        security =
                        {
                            scopelevel = "basic"
                        }
                    }

                }
            };
            var items = new Dictionary<string, object>() {
            { "functionality-response", "123456" }
            };



            var httpContextMoq = new Mock<HttpContext>();

            httpContextMoq.Setup(x => x.Items)
                .Returns(new Dictionary<object, object>());

            var httpContext = httpContextMoq.Object;


            //Create a new instance of the middleware
            RequestDelegate mockNextMiddleware = (HttpContext) =>
            {
                return Task.FromResult(0);
            };
            var securityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(mockNextMiddleware, _memoryCache, _configuration);


            //Act 
            await securityDisponibilityMiddleware.InvokeAsync(httpContext);
            //securityDisponibilityMiddleware.SecurityCheck(httpContext);


            Assert.True(httpContext.Items.ContainsKey("functionality-response"));

            //Assert.NotNull(httpContext);

        }


    }
}
