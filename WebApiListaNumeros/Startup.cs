using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middlewares;
using Middlewares.ExceptionHandler;
using Middlewares.FunctionalityHandler;
using Middlewares.SecurityDisponibilityHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddSingleton<IExceptionFilter, ExceptionFilter>();

            //services.AddTransient<IFunctionalityFilter, FunctionalityFilter>();

            services.AddSingleton<ICacheProvider, CacheProvider>();

            services.AddTransient<IDisponibilityFilter, DisponibilityFilter>();

            services.AddTransient<ISecurityFilter, SecurityFilter>();

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
    }
}
