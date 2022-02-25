﻿using Microsoft.AspNetCore.Http;
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
using Middlewares.SecurityDisponibilityHandler;
using System.Net;

namespace XUnitTesting
{
    
    public class SecurityDisponibilityMiddlewareTest
    {
        Mock<RequestDelegate> _next;
        Mock<ISecurityDisponibilityFilter> _securityDisponibilityFilter;
        Mock<IMemoryCache> _memoryCache;
        DefaultHttpContext _context;

        public SecurityDisponibilityMiddlewareTest()
        {
            _context = new DefaultHttpContext();
            _next = new Mock<RequestDelegate>();
            _securityDisponibilityFilter = new Mock<ISecurityDisponibilityFilter>();
            _memoryCache = new Mock<IMemoryCache>();
        }

        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_Should_Not_ThrowException()
        {
            _context.Items["functionality-response"] = "{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}";
            _context.Request.Headers["Authorization"] = "Basic QWRtaW46QWRtaW4xMjM=";

            _securityDisponibilityFilter.Setup(repo => repo.DisponibilityCheck(_context));
            _securityDisponibilityFilter.Setup(repo => repo.SecurityCheck(_context))
                .Returns(Task.FromResult(_context));
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _securityDisponibilityFilter.Object, _memoryCache.Object);

            //Act
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().NotThrow<Exception>();
        }

        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_DisponibilityTest_Should_ThrowException()
        {
            _context.Items["functionality-response"] = "{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"02:00\",\n                        \"to_hour\": \"03:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}";    
            _context.Request.Headers["Authorization"] = "Basic QWRtaW46QWRtaW4xMjM=";

            _securityDisponibilityFilter.Setup(repo => repo.DisponibilityCheck(_context))                
            .Returns(Task.FromException(new UnauthorizedAccessException("Unauthorized! Only Weekdays from 8 am to 10 pm")));
            _securityDisponibilityFilter.Setup(repo => repo.SecurityCheck(_context));
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _securityDisponibilityFilter.Object, _memoryCache.Object);

            //Act
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //assert
            function.Should().Throw<UnauthorizedAccessException>().WithMessage("Unauthorized! Only Weekdays from 8 am to 10 pm");
            
        }             
        [Fact]
        public async Task SecurityDisponibilityMiddlewareTest_DisponibilityTest_Should_Not_ThrowException_And_SecurityTest_Should_ThrowException()
        {
            _context.Items["functionality-response"] = "{\n    \"data\": {\n        \"channel\": \"sucursal\",\n        \"endpoint\": \"/v1/minipompom/basic/list\",\n        \"method\": \"GET\",\n        \"availability\": {\n            \"business_hours\": {\n                \"includes_holidays\": true,\n                \"includes\": [\n                    {\n                        \"weekday\": \"mon\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"tue\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"wed\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"thu\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"fri\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sat\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    },\n                    {\n                        \"weekday\": \"sun\",\n                        \"from_hour\": \"08:00\",\n                        \"to_hour\": \"22:00\"\n                    }\n                ],\n                \"message\": {\n                    \"title\": \"\",\n                    \"detail\": null\n                }\n            },\n            \"out_of_service_list\": []\n        },\n        \"config\": {\n            \"security\": {\n                \"scopelevel\":\"basic\"\n            }\n        }\n    }\n}";
            _context.Request.Headers["Authorization"] = "Basic QWRtaTpBZG1pbjEyMw==";

            _securityDisponibilityFilter.Setup(repo => repo.DisponibilityCheck(_context));
            _securityDisponibilityFilter.Setup(repo => repo.SecurityCheck(_context))
                .Returns(Task.FromException(new UnauthorizedAccessException()));
            var SecurityDisponibilityMiddleware = new SecurityDisponibilityMiddleware(_next.Object, _securityDisponibilityFilter.Object, _memoryCache.Object);

            //Act
            Func<Task> function = async () => { await SecurityDisponibilityMiddleware.InvokeAsync(_context); };

            //Assert
            function.Should().Throw<UnauthorizedAccessException>();

        }
    }
}
