using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Order => Set<Order>();
        public DbSet<OrderItem> OrderItem => Set<OrderItem>();

        public DbSet<Item> Item => Set<Item>();
        public DbSet<Category> Category => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Category>().HasData(
            //new Category { Id = 1, Name = "מוצרי ניקיון" },
            //new Category { Id = 2, Name = "גבינות" },
            //new Category { Id = 3, Name = "ירקות ופירות" },
            //new Category { Id = 4, Name = "בשר ודגים" },
            //new Category { Id = 5, Name = "מאפים" }
        //);
        }

    }
}
