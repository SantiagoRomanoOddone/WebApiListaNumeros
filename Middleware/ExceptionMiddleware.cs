using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Middleware.ErrorConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
      
        // El parámetro loggerFactory es para poder construir un logger y loggear las excepciones en el gestor de excepciones.
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Oooops! Algo salió mal: {ex.Message}");
                //Este método encapsula el error y crea un response personalizado para informarle al cliente
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }
        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            //Se coloca el código de error que se quiera . Este es para mostrar la diferencia -> cod 406
            context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorInfo()
            {
                StatusCode = StatusCodes.Status406NotAcceptable, //Custom error
                Message = $"Algo salió mal. Error!: {exception.Message}",
                StackTrace = exception.StackTrace
            }));
        }
    }
}
