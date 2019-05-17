using MAL.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MAL.Migrations
{
    public class PostgresDBMigrationHandler : IDBMigrationHandler
    {
        public readonly IConfigProvider _configProvider;

        public PostgresDBMigrationHandler(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public void Migrate()
        {
            var retryCount = 1;
            var allowedRetry = 5;
            var db = _configProvider.GetDbConnectionString();

            do
            {
                try
                {
                    using (var cnx = new NpgsqlConnection(db))
                    {
                        var evolve = new Evolve.Evolve(cnx, x => Console.WriteLine(x))
                        {
                            Locations = new[] { "db/migrations" },
                            IsEraseDisabled = true,

                        };

                        evolve.Migrate();
                        break;
                    };
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"DB Migrations Failed. {Environment.NewLine}{ex}");
                }
                Thread.Sleep(5000);
                Console.WriteLine($"Retrying migration - Attempt number {retryCount}");
            }
            while (retryCount <= allowedRetry);
            
        }

        public void Erase()
        {
            var db = _configProvider.GetDbConnectionString();

            try
            {
                using (var cnx = new NpgsqlConnection(db))
                {
                    var evolve = new Evolve.Evolve(cnx);

                    evolve.Erase();
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB Migrations Failed. \n{ex}");
                throw;
            }
        }
    }
}
