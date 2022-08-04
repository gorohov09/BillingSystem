﻿// <auto-generated />
using System;
using BillingSystem.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BillingSystem.DAL.Migrations
{
    [DbContext(typeof(BillingSystemDB))]
    partial class BillingSystemDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BillingSystem.Domain.Entities.CoinDomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("History")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Coin");
                });

            modelBuilder.Entity("BillingSystem.Domain.Entities.UserProfileDomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long?>("Amount")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Name" }, "Phone_Index")
                        .IsUnique();

                    b.ToTable("UserProfile");
                });

            modelBuilder.Entity("BillingSystem.Domain.Entities.CoinDomain", b =>
                {
                    b.HasOne("BillingSystem.Domain.Entities.UserProfileDomain", "UserProfile")
                        .WithMany("Coins")
                        .HasForeignKey("UserProfileId");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("BillingSystem.Domain.Entities.UserProfileDomain", b =>
                {
                    b.Navigation("Coins");
                });
#pragma warning restore 612, 618
        }
    }
}
