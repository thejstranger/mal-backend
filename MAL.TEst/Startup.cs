using MAL.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.TEst
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        private ServiceProvider _serviceProvider;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())).AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = new ConfigProvider();
            var issuer = config.GetJwtIssuer();
            var key = config.GetJwtKey();
            var aud = config.GetJwtAudience();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),

                        ValidateLifetime = true,
                        RequireExpirationTime = true,

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = aud,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        RequireSignedTokens = true
                    };
                    options.RequireHttpsMetadata = false;
                });

            DIHandler.RegisterDI(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("test", new Info { Title = "Auth", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("auth/swagger/test/swagger.json", "Auth API V1");
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}