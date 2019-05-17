using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.DAL
{

    public class EntityFrameworkContext : IDatabaseContext
    {
        private readonly DbContext _dbContext;

        private IRepository<Users> _userRepository;

        public EntityFrameworkContext()
        {
        }

        public EntityFrameworkContext(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> RetrieveRepository<T>() where T : class
        {
            return new EntityFrameworkRepository<T>(this);
        }

        public virtual IRepository<Users> UserRepository =>
          _userRepository ?? (_userRepository = RetrieveRepository<Users>());


        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual EntityEntry Entry<T>(T entity) where T : class
        {
            return _dbContext.Entry(entity);
        }

        public virtual DbSet<T> Set<T>() where T : class
        {
            return _dbContext.Set<T>();
        }
    }
  
}
