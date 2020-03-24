using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDSServer.Models;
using TDSServer.Services;

namespace TDSServer
{
    public class AppDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Role>()
                .HasData(
                    new Role { Id = 1, Name = "Administrator", Description = "Администратор" },
                    new Role { Id = 2, Name = "Driver", Description = "Водитель" }
                );

            modelBuilder
                .Entity<User>()
                .HasData(
                    new User 
                    { 
                        Id = 1, 
                        Username = "Admin", 
                        FullName = "Администратор", 
                        PasswordHash = UserService.GetPasswordHash("Admin"), 
                        RoleId = 1
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
