using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Middlewares;
using Middlewares.ExceptionHandler;
using Middlewares.FunctionalityHandler;
using Middlewares.SecurityDisponibilityHandler;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
//using OpenTracing;
//using OpenTracing.Util;

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

            #region Open Telemetry
            services.AddOpenTelemetryTracing(
            builder =>
            {
                builder
                    .AddSource(nameof(CacheProvider.FunctionalityCheckAsync))
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder
                        .CreateDefault()
                        .AddService(Assembly.GetEntryAssembly().GetName().Name))                   
                    .AddAspNetCoreInstrumentation(
                    options =>
                    {
                        options.Enrich = Enrich;
                        options.RecordException = true;
                    }
                    )
                    .AddOtlpExporter(options => options.Endpoint = new Uri(Environment.GetEnvironmentVariable("Jaeger")));
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

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMiddleware<FunctionalityMiddleware>();

            app.UseMiddleware<SecurityDisponibilityMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void Enrich(Activity activity, string eventName, object obj)
        {
            if (obj is HttpRequest request)
            {
                var context = request.HttpContext;
                activity.AddTag("http.flavor", GetHttpFlavour(request.Protocol));
                activity.AddTag("http.scheme", request.Scheme);
                activity.AddTag("http.client_ip", context.Connection.RemoteIpAddress);
                activity.AddTag("http.request_content_length", request.ContentLength);
                activity.AddTag("http.request_content_type", request.ContentType);

                var user = context.User;
                if (user.Identity?.Name is not null)
                {
                    activity.AddTag("enduser.id", user.Identity.Name);
                    activity.AddTag(
                        "enduser.scope",
                        string.Join(',', user.Claims.Select(x => x.Value)));
                }
            }
            else if (obj is HttpResponse response)
            {
                activity.AddTag("http.response_content_length", response.ContentLength);
                activity.AddTag("http.response_content_type", response.ContentType);
            }
        }
        public static string GetHttpFlavour(string protocol)
        {
            if (HttpProtocol.IsHttp10(protocol))
            {
                return "1.0";
            }
            else if (HttpProtocol.IsHttp11(protocol))
            {
                return "1.1";
            }
            else if (HttpProtocol.IsHttp2(protocol))
            {
                return "2.0";
            }
            else if (HttpProtocol.IsHttp3(protocol))
            {
                return "3.0";
            }

            throw new InvalidOperationException($"Protocol {protocol} not recognised.");
        }


    }
}
