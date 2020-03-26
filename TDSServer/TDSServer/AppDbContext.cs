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
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PositionRole> PositionRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; } 

        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Name = "MobileApp", FullName = "Мобильное приложение" },
                new Permission { Id = 2, Name = "OrderRead", FullName = "Чтение заявок" }
            };
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Administrator", FullName = "Администратор" },
                new Role { Id = 2, Name = "Driver", FullName = "Водитель" }
            };

            modelBuilder
                .Entity<Permission>()
                .HasData(permissions);

            modelBuilder
                .Entity<Role>()
                .HasData(roles);

            modelBuilder
                .Entity<RolePermission>()
                .HasKey(k => new { k.PermissionId, k.RoleId });

            modelBuilder
                .Entity<RolePermission>()
                .HasData(
                    roles
                    .Where(r => r.Id == 1)
                    .Join(permissions, k => 1, k => 1, (r, p) => new RolePermission { RoleId = r.Id, PermissionId = p.Id })
                );

            modelBuilder
                .Entity<User>()
                .HasData(
                    new User 
                    { 
                        Id = 1, 
                        Username = "admin", 
                        FullName = "Администратор", 
                        PasswordHash = UserService.GetPasswordHash("admin")
                    }
                );

            modelBuilder
                .Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });

            modelBuilder
                .Entity<PositionRole>()
                .HasKey(x => new { x.PositionId, x.RoleId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
