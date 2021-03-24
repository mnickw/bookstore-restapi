using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bookstore_restapi.Models;

namespace bookstore_restapi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<CartElement> CartElements { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartElement>().HasKey(table => new {
                table.UserId,
                table.BookId
            });
            modelBuilder.Entity<OrderBook>().HasKey(table => new {
                table.OrderId,
                table.BookId
            });

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Name = "ASP.NET Core in action", Author = "Andrew Lock", Price = 80 },
                new Book { Id = 2, Name = "Pro ASP.NET Core 3", Author = "Adam Freeman", Price = 70 });
            
            modelBuilder.Entity<CartElement>().HasData(
                new CartElement { UserId = "auth0|60524cdf7f519e00700fbe53", BookId = 1, BookQnty = 1 },
                new CartElement { UserId = "auth0|60524cdf7f519e00700fbe53", BookId = 2, BookQnty = 3 });
        }
    }
}
