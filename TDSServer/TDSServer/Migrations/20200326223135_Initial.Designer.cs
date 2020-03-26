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
    [Migration("20200326223135_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TDSServer.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("UserId");

                    b.ToTable("Employees");
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

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Мобильное приложение",
                            Name = "MobileApp"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Чтение заявок",
                            Name = "OrderRead"
                        });
                });

            modelBuilder.Entity("TDSServer.Models.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

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

            modelBuilder.Entity("TDSServer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FullName = "Администратор",
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = 2,
                            FullName = "Водитель",
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
                        });
                });

            modelBuilder.Entity("TDSServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

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
                            PasswordHash = "�iv�A���M�߱g��s�K��o*�H�",
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
                });

            modelBuilder.Entity("TDSServer.Models.Employee", b =>
                {
                    b.HasOne("TDSServer.Models.Position", "Position")
                        .WithMany("Employees")
                        .HasForeignKey("PositionId");

                    b.HasOne("TDSServer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
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