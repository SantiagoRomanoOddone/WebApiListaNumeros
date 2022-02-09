using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApiListaNumeros.ErrorDetails;

namespace WebApiListaNumeros.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        // Constructor que por inyección de dependencias recibe el delegado del siguiente middleware. Cada vez que se instancia un Middleware sabe cual es el siguiente para poder delegarle la ejecución.
        // El parámetro loggerFactory es para poder construir un logger y loggear las excepciones en el gestor de excepciones.
        //TODO: Es con SERILOG Y MONGODB
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();

        }

        //Se invoca cuando nostros queremos delegar. Cuando el middleware invoca, va a invocar a esta operación
        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Aqui va el try - catch porque en el momento que se invoca el siguiente middleware es cuando puede llegarse a ejecutar el código con problemas
            try
            {
                await _next(httpContext);


            }
            catch (Exception ex)
            {
                //_logger logea el error con el mensjae que se quiera.
                _logger.LogError(ex, $"Oooops! Algo salió mal: {ex.Message}");
                //Este método maneja la excepción de manera global y crea un response personalizado para informarle al cliente
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }

        //Operación que encapsula el error y manda una respuesta personalizada como código de error
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
