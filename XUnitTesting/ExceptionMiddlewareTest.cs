
using Microsoft.AspNetCore.Http;
using Middlewares;
using Middlewares.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTesting
{
    
    public class ExceptionMiddlewareTest
    {
        [Fact]
        public async Task ExceptionMiddlewareTest_ReturnsXXXStatusCode()
        {
            #region AppException
            //arrange
            var expectedException = new AppException();

            RequestDelegate mockNextMiddleware = (HttpContext) =>
            {
                return Task.FromException(expectedException);
            };
            var httpContext = new DefaultHttpContext();

            var exceptionMiddleware = new ExceptionMiddleware(mockNextMiddleware);

            //act
            await exceptionMiddleware.InvokeAsync(httpContext);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)httpContext.Response.StatusCode);
            #endregion

            #region UnauthorizedAccessException
            ////arrange
            //var expectedException = new UnauthorizedAccessException();
            //RequestDelegate mockNextMiddleware = (HttpContext) =>
            //{
            //    return Task.FromException(expectedException);
            //};
            //var httpContext = new DefaultHttpContext();

            //var exceptionMiddleware = new ExceptionMiddleware(mockNextMiddleware);

            ////act
            //await exceptionMiddleware.InvokeAsync(httpContext);

            ////assert
            //Assert.Equal(HttpStatusCode.Unauthorized, (HttpStatusCode)httpContext.Response.StatusCode);
            #endregion

            #region KeyNotFoundException
            ////arrange
            //var expectedException = new KeyNotFoundException();
            //RequestDelegate mockNextMiddleware = (HttpContext) =>
            //{
            //    return Task.FromException(expectedException);
            //};
            //var httpContext = new DefaultHttpContext();

            //var exceptionMiddleware = new ExceptionMiddleware(mockNextMiddleware);

            ////act
            //await exceptionMiddleware.InvokeAsync(httpContext);

            ////assert
            //Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)httpContext.Response.StatusCode);
            #endregion

            #region Default
            ////arrange
            //var expectedException = new ArgumentNullException();
            //RequestDelegate mockNextMiddleware = (HttpContext) =>
            //{
            //    return Task.FromException(expectedException);
            //};
            //var httpContext = new DefaultHttpContext();

            //var exceptionMiddleware = new ExceptionMiddleware(mockNextMiddleware);

            ////act
            //await exceptionMiddleware.InvokeAsync(httpContext);

            ////assert
            //Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)httpContext.Response.StatusCode);
            #endregion

        }

    }
}
