using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Middlewares;
using Middlewares.Auxiliaries;
using Middlewares.ExceptionHandler;
using Middlewares.FunctionalityHandler;
using Middlewares.RequestResponseLoggingHandler;
using Middlewares.SecurityDisponibilityHandler;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Telemetry;

namespace WebApiListaNumeros
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddControllers();

            services.AddHttpClient();

            services.AddHttpContextAccessor();

            services.AddSingleton<IExceptionFilter, ExceptionFilter>();

            services.AddSingleton<ICacheProvider, CacheProvider>();

            services.AddTransient<IDisponibilityFilter, DisponibilityFilter>();

            services.AddTransient<ISecurityFilter, SecurityFilter>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IRequestLogging, RequestLogging>();

            services.AddSingleton<IResponseLogging, ResponseLogging>();

            #region Distributed Tracing

            //services.AddSingleton<ITracer>(serviceProvider =>
            //{
            //    string serviceName = Assembly.GetEntryAssembly().GetName().Name;

            //    ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //    ISampler sampler = new ConstSampler(sample: true);

            //    ITracer tracer = new Tracer.Builder(serviceName)
            //        .WithLoggerFactory(loggerFactory)
            //        .WithSampler(sampler)
            //        .Build();

            //    GlobalTracer.Register(tracer);

            //    return tracer;
            //});

            //services.AddOpenTracing();

            #endregion

            #region OpenTelemetry Tracing
            services.AddOpenTelemetryTracing(
            builder =>
            {
                builder
                    .AddSource(Constant.OPENTELEMETRY_SOURCE)
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder
                        .CreateDefault()
                        .AddService(Assembly.GetEntryAssembly().GetName().Name))
                    .AddAspNetCoreInstrumentation(
                    options =>
                    {
                        options.Enrich = DataInfo.Enrich;
                        options.RecordException = true;
                    })
                    .AddJaegerExporter();
            });
            #endregion

            #region RequestResponseLogging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.AddEventSourceLogger();
            });
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiListaNumeros", Version = "v1" });
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                app.UseSwagger();
             
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/minipompom/swagger/swagger.json", "WebApiListaNumeros v1"));
            }

            app.UseHttpsRedirection();       

            app.UseRouting();
          
            app.UseAuthorization();


            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<FunctionalityMiddleware>();

            app.UseMiddleware<SecurityDisponibilityMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
