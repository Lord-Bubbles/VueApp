﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VueApp1.Server.Helpers;

#nullable disable

namespace VueApp1.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240205172709_remove id, set username as primary key")]
    partial class removeidsetusernameasprimarykey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("VueApp1.Server.Models.AccountModel", b =>
                {
                    b.Property<string>("username")
                        .HasColumnType("text");

                    b.Property<string>("hashPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("salt")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("username");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("VueApp1.Server.Models.UserModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseSerialColumn(b.Property<int>("ID"));

                    b.Property<int>("age")
                        .HasColumnType("integer");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("school")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
