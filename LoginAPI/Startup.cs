using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using LoginAPI.Data;
using LoginAPI.Services;
using LoginAPI.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace LoginAPI
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddDbContext<LoginDbContext>();

            services.AddCors();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //var asdsad = _configuration.GetSection("AppSettings");
            //services.Configure<AppSettings>(asdsad);
           
            var key = Encoding.ASCII.GetBytes(_configuration["JwtToken:SecretKey"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtToken:Issuer"],
                    ValidAudience = _configuration["JwtToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]))
                };
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo() { Title = "WebAPI", Version = "v1" });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Login API"));

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

      

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
