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
using WebApiListaNumeros.Services;

namespace WebApiListaNumeros
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMemoryCache();
            services.AddControllers();
            services.AddHttpClient();


            #region Authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Validate the server. That generates the token
                    ValidateIssuer = true,
                    //Validate the recipient of the token is authorized to receive
                    ValidateAudience = true,
                    //Check if the token is not expired and the signing key of the issuer is valid
                    ValidateLifetime = false,
                    //Validate signature of the token
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "https://localhost:44393",
                    ValidAudience = "https://localhost:44388",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeywqewqeqqqqqqqqqqqweeeeeeeeeeeeeeeeeeeqweqe"))

                    ////I have to specify the values for "Audience", "Issuer" and "Secret key" in this project inside the appsettings.json file.
                    //ValidIssuer = Configuration["Jwt:Issuer"],
                    //ValidAudience = Configuration["Jwt:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });

            #endregion
            services.AddTransient<IUserService, UserService>();

            services.AddSingleton<IExceptionFilter, ExceptionFilter>();

            services.AddTransient<IFunctionalityFilter, FunctionalityFilter>();

            services.AddTransient<ISecurityDisponibilityFilter, SecurityDisponibilityFilter>();           

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiListaNumeros v1"));
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
