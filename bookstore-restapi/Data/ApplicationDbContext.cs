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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Name = "ASP.NET Core in action", Author = "Andrew Lock", Price = 80 },
                new Book { Id = 2, Name = "Pro ASP.NET Core 3", Author = "Adam Freeman", Price = 70 });

        }
    }
}
