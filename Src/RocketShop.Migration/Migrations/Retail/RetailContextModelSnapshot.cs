﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RocketShop.Database.EntityFramework;

#nullable disable

namespace RocketShop.Migration.Migrations.Retail
{
    [DbContext(typeof(RetailContext))]
    partial class RetailContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RocketShop.Database.Model.Retail.MainCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameTh")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NameEn", "NameTh");

                    b.ToTable("M_Category");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("SubCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Thumbnails")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SubCategoryId", "Name");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.SubCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("MainCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("NameEn")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NameTh")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NameEn", "NameTh", "MainCategoryId");

                    b.ToTable("S_Category");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.SubModel.ProductAttribute", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("Unit")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAttribute");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.SubModel.ProductImages", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.SubModel.ProductAttribute", b =>
                {
                    b.HasOne("RocketShop.Database.Model.Retail.Product", null)
                        .WithMany("Attributes")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.SubModel.ProductImages", b =>
                {
                    b.HasOne("RocketShop.Database.Model.Retail.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Retail.Product", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
