﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RocketShop.Database.EntityFramework;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    [DbContext(typeof(WarehouseContext))]
    partial class WarehouseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RocketShop.Database.Model.Warehouse.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("NameTH")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ProvinceId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Warehouse.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("NameEN")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameTH")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Warehouse.SubDistrict", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("DistrictId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<string>("NameEN")
                        .HasColumnType("text");

                    b.Property<string>("NameTH")
                        .HasColumnType("text");

                    b.Property<int?>("PostalCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("DistrictId");

                    b.HasIndex("PostalCode");

                    b.ToTable("SubDistricts");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Warehouse.Views.AddressView", b =>
                {
                    b.Property<int>("DistrictCode")
                        .HasColumnType("integer");

                    b.Property<int>("DistrictId")
                        .HasColumnType("integer");

                    b.Property<string>("DistrictNameEN")
                        .HasColumnType("text");

                    b.Property<string>("DistrictNameTH")
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<int?>("PostalCode")
                        .HasColumnType("integer");

                    b.Property<int>("ProvinceCode")
                        .HasColumnType("integer");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("integer");

                    b.Property<string>("ProvinceNameEN")
                        .HasColumnType("text");

                    b.Property<string>("ProvinceNameTH")
                        .HasColumnType("text");

                    b.Property<int>("SubDistrictId")
                        .HasColumnType("integer");

                    b.Property<string>("SubDistrictNameEN")
                        .HasColumnType("text");

                    b.Property<string>("SubDistrictNameTH")
                        .HasColumnType("text");

                    b.ToTable((string)null);

                    b.ToView("V_Address", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
