using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAL.Services;
using MAL.WebAPI.Helpers;
using MAL.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;

namespace MAL.WebAPI
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //_serviceProvider = services.BuildServiceProvider();
            var config = new ConfigProvider();//_serviceProvider.GetService<IConfigProvider>();
            var issuer = config.GetJwtIssuer();
            var key = config.GetJwtKey();
            var aud = config.GetJwtAudience();

            // Add JWT Authorisation 
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

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("controllers", new Info { Title = "Controllers", Version = "v1" });

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
          //  var config = _serviceProvider.GetService<IConfigProvider>();

            // Enable middleware to save generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("user/swagger/controllers/swagger.json", "API V1");
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();

           
        }
    }
}
