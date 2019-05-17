using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAL.DAL;
using MAL.Services;

namespace MAL.WebAPI
{
    public class DIHandler
    {

        private static bool _registered;

        private static ServiceProvider _serviceProvider;

        public static ServiceProvider RegisterDI(IServiceCollection services = null)
        {
            if (_registered) return _serviceProvider;

            if (services == null)
            {
                services = new ServiceCollection();
            }

            // Auto
            services.AddAutoMapper();


            // DbContext
            services.AddDbContext<MyDbContext>();

            //  OrmContext / Repository
            services.AddTransient<IDatabaseContext, EntityFrameworkContext>();

            // DbContext for injection into OrmContext
            services.AddScoped<DbContext>(provider => provider.GetService<MyDbContext>());

            // Services
            services.AddTransient<IUserService, UserService>();

            _registered = true;

            _serviceProvider = services.BuildServiceProvider();
            return _serviceProvider;
        }
    }
}
