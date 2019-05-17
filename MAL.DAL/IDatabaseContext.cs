using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.DAL
{
    public interface IDatabaseContext
    {
        IRepository<Users> UserRepository { get; }

        void SaveChanges();
    }
}
