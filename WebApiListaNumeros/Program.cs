using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiListaNumeros
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             //PRUEB
             //.ConfigureLogging(logging =>
             //{
             //    logging.ClearProviders();
             //    logging.AddOpenTelemetry(options =>
             //    {
             //        options.AddProcessor(new SimpleExportProcessor<LogRecord>(new ConsoleExporter<LogRecord>(new ConsoleExporterOptions())));
             //    });
             //})
                //PRUEBA
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
