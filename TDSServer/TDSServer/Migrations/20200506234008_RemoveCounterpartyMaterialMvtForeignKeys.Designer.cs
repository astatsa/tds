﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TDSServer;

namespace TDSServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200506234008_RemoveCounterpartyMaterialMvtForeignKeys")]
    partial class RemoveCounterpartyMaterialMvtForeignKeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TDSServer.Models.Counterparty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TypeId")
                        .HasColumnName("CounterpartyTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Counterparties");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyMaterialMvt", b =>
                {
                    b.Property<int>("CounterpartyId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsComing")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Quantity")
                        .HasColumnType("double");

                    b.Property<int>("RegistratorId")
                        .HasColumnType("int");

                    b.Property<int>("RegistratorTypeId")
                        .HasColumnType("int");

                    b.HasKey("CounterpartyId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CounterpartyMaterialMvts");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyMaterialRest", b =>
                {
                    b.Property<int>("CounterpartyId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<double>("Rest")
                        .HasColumnType("double");

                    b.HasKey("CounterpartyId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CounterpartyMaterialRests");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyRestCorrection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CounterpartyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CounterpartyId");

                    b.HasIndex("UserId");

                    b.ToTable("CounterpartyRestCorrections");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyRestCorrectionMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Correction")
                        .HasColumnType("double");

                    b.Property<int>("CounterpartyRestCorrectionId")
                        .HasColumnType("int");

                    b.Property<int?>("MaterialId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CounterpartyRestCorrectionId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CounterpartyRestCorrectionMaterials");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("CounterpartyTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Поставщик",
                            IsDeleted = false,
                            Name = "Supplier"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Покупатель",
                            IsDeleted = false,
                            Name = "Customer"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TDSServer.Models.GasStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("GasStations");
                });

            modelBuilder.Entity("TDSServer.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MeasureId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("MeasureId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("TDSServer.Models.Measure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Measures");
                });

            modelBuilder.Entity("TDSServer.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateCreate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("OrderStateId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<double>("Volume")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DriverId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("OrderStateId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("TDSServer.Models.OrderState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("OrderStates");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Новый",
                            IsDeleted = false,
                            Name = "New"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Просмотрен",
                            IsDeleted = false,
                            Name = "Viewed"
                        },
                        new
                        {
                            Id = 3,
                            FullName = "Погрузка произведена",
                            IsDeleted = false,
                            Name = "Loaded"
                        },
                        new
                        {
                            Id = 4,
                            FullName = "Завершен",
                            IsDeleted = false,
                            Name = "Completed"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.OrderStateHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int?>("OrderStateId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("OrderStateId");

                    b.HasIndex("UserId");

                    b.ToTable("OrderStateHistories");
                });

            modelBuilder.Entity("TDSServer.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Мобильное приложение",
                            IsDeleted = false,
                            Name = "MobileApp"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Чтение заявок",
                            IsDeleted = false,
                            Name = "OrderRead"
                        },
                        new
                        {
                            Id = 3,
                            FullName = "Чтение справочников",
                            IsDeleted = false,
                            Name = "ReferenceRead"
                        },
                        new
                        {
                            Id = 4,
                            FullName = "Изменение справочников",
                            IsDeleted = false,
                            Name = "ReferenceEdit"
                        },
                        new
                        {
                            Id = 5,
                            FullName = "Изменение заявок",
                            IsDeleted = false,
                            Name = "OrderEdit"
                        },
                        new
                        {
                            Id = 6,
                            FullName = "Чтение дозаправок",
                            IsDeleted = false,
                            Name = "RefuelRead"
                        },
                        new
                        {
                            Id = 7,
                            FullName = "Изменение дозаправок",
                            IsDeleted = false,
                            Name = "RefuelEdit"
                        },
                        new
                        {
                            Id = 8,
                            FullName = "Изменение пользователей",
                            IsDeleted = false,
                            Name = "UserEdit"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("TDSServer.Models.PositionRole", b =>
                {
                    b.Property<int>("PositionId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("PositionId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("PositionRoles");
                });

            modelBuilder.Entity("TDSServer.Models.Refuel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("DriverId")
                        .HasColumnType("int");

                    b.Property<int?>("GasStationId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Volume")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("GasStationId");

                    b.ToTable("Refuels");
                });

            modelBuilder.Entity("TDSServer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Администратор",
                            IsDeleted = false,
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Водитель",
                            IsDeleted = false,
                            Name = "Driver"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.RolePermission", b =>
                {
                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("PermissionId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");

                    b.HasData(
                        new
                        {
                            PermissionId = 1,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 2,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 3,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 4,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 5,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 6,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 7,
                            RoleId = 1
                        },
                        new
                        {
                            PermissionId = 8,
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("TDSServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Администратор",
                            IsDeleted = false,
                            PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("TDSServer.Models.Counterparty", b =>
                {
                    b.HasOne("TDSServer.Models.CounterpartyType", "Type")
                        .WithMany("Counterparty")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyMaterialMvt", b =>
                {
                    b.HasOne("TDSServer.Models.Counterparty", "Counterparty")
                        .WithMany()
                        .HasForeignKey("CounterpartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyMaterialRest", b =>
                {
                    b.HasOne("TDSServer.Models.Counterparty", "Counterparty")
                        .WithMany()
                        .HasForeignKey("CounterpartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyRestCorrection", b =>
                {
                    b.HasOne("TDSServer.Models.Counterparty", "Counterparty")
                        .WithMany()
                        .HasForeignKey("CounterpartyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TDSServer.Models.CounterpartyRestCorrectionMaterial", b =>
                {
                    b.HasOne("TDSServer.Models.CounterpartyRestCorrection", null)
                        .WithMany("MaterialCorrections")
                        .HasForeignKey("CounterpartyRestCorrectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.Employee", b =>
                {
                    b.HasOne("TDSServer.Models.Position", "Position")
                        .WithMany("Employees")
                        .HasForeignKey("PositionId");

                    b.HasOne("TDSServer.Models.User", "User")
                        .WithOne("Employee")
                        .HasForeignKey("TDSServer.Models.Employee", "UserId");
                });

            modelBuilder.Entity("TDSServer.Models.Material", b =>
                {
                    b.HasOne("TDSServer.Models.Measure", "Measure")
                        .WithMany()
                        .HasForeignKey("MeasureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.Order", b =>
                {
                    b.HasOne("TDSServer.Models.Counterparty", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Employee", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.OrderState", "OrderState")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStateId");

                    b.HasOne("TDSServer.Models.Counterparty", "Supplier")
                        .WithMany()
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.OrderStateHistory", b =>
                {
                    b.HasOne("TDSServer.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.HasOne("TDSServer.Models.OrderState", "OrderState")
                        .WithMany()
                        .HasForeignKey("OrderStateId");

                    b.HasOne("TDSServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.PositionRole", b =>
                {
                    b.HasOne("TDSServer.Models.Position", "Position")
                        .WithMany("PositionRoles")
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Role", "Role")
                        .WithMany("PositionRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.Refuel", b =>
                {
                    b.HasOne("TDSServer.Models.Employee", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");

                    b.HasOne("TDSServer.Models.GasStation", "GasStation")
                        .WithMany()
                        .HasForeignKey("GasStationId");
                });

            modelBuilder.Entity("TDSServer.Models.RolePermission", b =>
                {
                    b.HasOne("TDSServer.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TDSServer.Models.UserRole", b =>
                {
                    b.HasOne("TDSServer.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TDSServer.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
