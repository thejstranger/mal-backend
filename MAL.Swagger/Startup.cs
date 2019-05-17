using MAL.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.Swagger
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

        public IConfiguration Configuration { get; }

        private ServiceProvider _serviceProvider;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IConfigProvider, ConfigProvider>();

            _serviceProvider = services.BuildServiceProvider();
            var configProvider = _serviceProvider.GetService<IConfigProvider>();
            var iss = configProvider.GetJwtIssuer();
            var key = configProvider.GetJwtKey();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),

                        ValidateLifetime = true,
                        RequireExpirationTime = true,

                        ValidateIssuer = true,
                        ValidIssuer = iss,

                        ValidateAudience = true,
                        ValidAudience = iss,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        RequireSignedTokens = true
                    };
                    options.RequireHttpsMetadata = false;
                });

            services.AddSwaggerGen(c =>
            {

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

            DIHandler.RegisterDI(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var configProvider = _serviceProvider.GetService<IConfigProvider>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //                c.RoutePrefix = "auth";
                //c.SwaggerEndpoint($"/{configProvider.GetIngressRoutePath()["auth"]}/swagger/auth/swagger.json", "Auth API V1");
                //c.SwaggerEndpoint($"/{configProvider.GetIngressRoutePath()["api"]}/swagger/webapi/swagger.json", "Api API V1");
            });

            app.UseMvc();
        }
    }
}
