using MAL.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MAL.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = ConfigureServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var dbMigrationHandler = serviceProvider.GetService<IDBMigrationHandler>();

            PerformDbMigration(dbMigrationHandler);
        }

        private static void PerformDbMigration(IDBMigrationHandler dbMigrationHandler)
        {
            dbMigrationHandler.Migrate();
        }

        private static ServiceCollection ConfigureServices()
        {
            var srvCollection = new ServiceCollection();
            srvCollection.AddSingleton<IConfigProvider, ConfigProvider>();
            srvCollection.AddTransient<IDBMigrationHandler, PostgresDBMigrationHandler>();

            return srvCollection;
        }
    
    }
}
