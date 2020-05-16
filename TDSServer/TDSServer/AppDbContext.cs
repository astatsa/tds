using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Counterparty> Counterparties { get; set; }
        public DbSet<CounterpartyType> CounterpartyTypes { get; set; }
        public DbSet<CounterpartyMaterialRest> CounterpartyMaterialRests { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<GasStation> GasStations { get; set; }
        public DbSet<Refuel> Refuels { get; set; }
        public DbSet<OrderStateHistory> OrderStateHistories { get; set; }
        public DbSet<CounterpartyMaterialMvt> CounterpartyMaterialMvts { get; set; }
        public DbSet<CounterpartyRestCorrection> CounterpartyRestCorrections { get; set; }
        public DbSet<CounterpartyRestCorrectionMaterial> CounterpartyRestCorrectionMaterials { get; set; }

        private LastEntityChangesService lastEntityChangesService;
        public AppDbContext(DbContextOptions options, LastEntityChangesService lastEntityChangesService) : base(options)
        {
            this.lastEntityChangesService = lastEntityChangesService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Name = "MobileApp", FullName = "Мобильное приложение" },
                new Permission { Id = 2, Name = "OrderRead", FullName = "Чтение заявок" },
                new Permission { Id = 3, Name = "ReferenceRead", FullName = "Чтение справочников" },
                new Permission { Id = 4, Name = "ReferenceEdit", FullName = "Изменение справочников" },
                new Permission { Id = 5, Name = "OrderEdit", FullName = "Изменение заявок" },
                new Permission { Id = 6, Name = "RefuelRead", FullName = "Чтение дозаправок" },
                new Permission { Id = 7, Name = "RefuelEdit", FullName = "Изменение дозаправок" },
                new Permission { Id = 8, Name = "UserEdit", FullName = "Изменение пользователей" },
                new Permission { Id = 9, Name = "PositionRead", FullName = "Чтение справочника должностей" }
            };
            var roles = new List<Role>
            {
                new Role { Id = 1, Name = "Administrator", FullName = "Администратор" },
                new Role { Id = 2, Name = "Driver", FullName = "Водитель" },
                new Role { Id = 3, Name = "Dispatcher", FullName = "Диспетчер" }
            };

            //Permission
            modelBuilder
                .Entity<Permission>()
                .HasData(permissions);

            //Role
            modelBuilder
                .Entity<Role>()
                .HasData(roles);

            //RolePermission
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
                .Entity<RolePermission>()
                .HasData(new RolePermission
                {
                    RoleId = 2,
                    PermissionId = 1
                });

            modelBuilder
                .Entity<RolePermission>()
                .HasData(
                    new RolePermission { RoleId = 3, PermissionId = 2 },
                    new RolePermission { RoleId = 3, PermissionId = 3 },
                    new RolePermission { RoleId = 3, PermissionId = 4 },
                    new RolePermission { RoleId = 3, PermissionId = 5 },
                    new RolePermission { RoleId = 3, PermissionId = 6 },
                    new RolePermission { RoleId = 3, PermissionId = 9 }
                );

            //User
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

            //UserRole
            modelBuilder
                .Entity<UserRole>()
                .HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder
                .Entity<UserRole>()
                .HasData(
                    new UserRole { RoleId = 1, UserId = 1 }
                );

            //PrositionRole
            modelBuilder
                .Entity<PositionRole>()
                .HasKey(x => new { x.PositionId, x.RoleId });

            //CounterpatyType
            modelBuilder
                .Entity<CounterpartyType>()
                .Property(e => e.Name)
                .HasConversion(new EnumToStringConverter<CounterpartyTypes>());
            modelBuilder
                .Entity<CounterpartyType>()
                .HasData(
                    new CounterpartyType { Id = 1, Name = Models.CounterpartyTypes.Supplier, FullName = "Поставщик" },
                    new CounterpartyType { Id = 2, Name = Models.CounterpartyTypes.Customer, FullName = "Покупатель" }
                );

            //CounterpartyMaterialRest
            modelBuilder
                .Entity<CounterpartyMaterialRest>()
                .HasKey(x => new { x.CounterpartyId, x.MaterialId });

            //Order
            modelBuilder
                .Entity<Order>()
                .Property(x => x.DateCreate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            //OrderState
            modelBuilder
                .Entity<OrderState>()
                .Property(x => x.Name)
                .HasConversion(new EnumToStringConverter<OrderStates>());
            modelBuilder
                .Entity<OrderState>()
                .HasData(
                    new OrderState { Id = 1, Name = Models.OrderStates.New, FullName = "Новый" },
                    new OrderState { Id = 2, Name = Models.OrderStates.Viewed, FullName = "Просмотрен" },
                    new OrderState { Id = 3, Name = Models.OrderStates.Loaded, FullName = "Погрузка произведена" },
                    new OrderState { Id = 4, Name = Models.OrderStates.Completed, FullName = "Завершен" }
                );

            //Employee
            modelBuilder
                .Entity<Employee>()
                .HasOne(x => x.User)
                .WithOne(x => x.Employee);

            //CounterpartyMaterialMvt
            modelBuilder
                .Entity<CounterpartyMaterialMvt>()
                .HasKey(x => new { x.RegistratorTypeId, x.RegistratorId, x.CounterpartyId, x.MaterialId });

            //Measure
            modelBuilder
                .Entity<Measure>()
                .HasData(new Measure { Id = 1, Name = "т", FullName = "тонны" });

            //Material
            modelBuilder
                .Entity<Material>()
                .Property(x => x.MeasureId)
                .HasDefaultValue(1);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(
                    x =>
                    (x.State == EntityState.Modified
                    || x.State == EntityState.Added
                    || x.State == EntityState.Deleted)
                    && x.Metadata.ClrType != null)
                .Select(x => x.Metadata.ClrType.FullName)
                .Distinct()
                .ToArray();
            int res = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            if(res > 0)
            {
                lastEntityChangesService.EntityChanged(changedEntities);
            }
            return res;
        }

        public DateTime GetEntityLastChangeDate<TEntity>() => lastEntityChangesService.GetEntityLastChangeDate<TEntity>();
    }
}
