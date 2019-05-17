using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.Migrations
{
    public interface IDBMigrationHandler
    {
        void Migrate();
        void Erase();
    }
}
