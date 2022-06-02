using Microsoft.EntityFrameworkCore;
using ShopBridge.DataLayer.ProductEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBridge.DataLayer
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { 
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ShopBridge")
                .EnableSensitiveDataLogging();
            }
        }
    }
}
