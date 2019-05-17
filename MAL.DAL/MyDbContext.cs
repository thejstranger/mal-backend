using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MAL.Common;

namespace MAL.DAL
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigProvider config = new ConfigProvider();
            optionsBuilder.UseNpgsql(config.GetDbConnectionString());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Users> Users { get; set; }
    }
}
