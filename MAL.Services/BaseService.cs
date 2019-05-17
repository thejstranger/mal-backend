using AutoMapper;
using System;
using MAL.DAL;
using System.Collections.Generic;
using System.Text;

namespace MAL.Services
{
    public abstract class BaseService
    {
        public IDatabaseContext _dbContext { get; }

        protected BaseService(IDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}