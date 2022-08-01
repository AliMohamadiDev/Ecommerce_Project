﻿// <auto-generated />
using System;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryManagement.Infrastructure.EFCore.Migrations
{
    [DbContext(typeof(InventoryContext))]
    partial class InventoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("InventoryManagement.Domain.InventoryAgg.Inventory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("InStock")
                        .HasColumnType("bit");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Inventory", (string)null);
                });

            modelBuilder.Entity("InventoryManagement.Domain.InventoryAgg.Inventory", b =>
                {
                    b.OwnsMany("InventoryManagement.Domain.InventoryAgg.InventoryOperation", "Operations", b1 =>
                        {
                            b1.Property<long>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("bigint");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<long>("Id"), 1L, 1);

                            b1.Property<long>("Count")
                                .HasColumnType("bigint");

                            b1.Property<DateTime>("CreationDate")
                                .HasColumnType("datetime2");

                            b1.Property<long>("CurrentCount")
                                .HasColumnType("bigint");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasMaxLength(1000)
                                .HasColumnType("nvarchar(1000)");

                            b1.Property<long>("InventoryId")
                                .HasColumnType("bigint");

                            b1.Property<bool>("Operation")
                                .HasColumnType("bit");

                            b1.Property<long>("OperatorId")
                                .HasColumnType("bigint");

                            b1.Property<long>("OrderId")
                                .HasColumnType("bigint");

                            b1.HasKey("Id");

                            b1.HasIndex("InventoryId");

                            b1.ToTable("InventoryOperations", (string)null);

                            b1.WithOwner("Inventory")
                                .HasForeignKey("InventoryId");

                            b1.Navigation("Inventory");
                        });

                    b.Navigation("Operations");
                });
#pragma warning restore 612, 618
        }
    }
}
