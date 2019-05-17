using MAL.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MAL.Swagger
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

            // Services
            services.AddTransient<IUserService, UserService>();

            _registered = true;

            _serviceProvider = services.BuildServiceProvider();
            return _serviceProvider;
        }
    }
}
