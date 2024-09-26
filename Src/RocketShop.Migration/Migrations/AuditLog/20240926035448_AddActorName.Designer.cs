﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RocketShop.Database.EntityFramework;

#nullable disable

namespace RocketShop.Migration.Migrations.AuditLog
{
    [DbContext(typeof(AuditLogContext))]
    [Migration("20240926035448_AddActorName")]
    partial class AddActorName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RocketShop.Database.Model.AuditLog.ActivityLog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Actor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ActorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LogDetail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Actor");

                    b.HasIndex("ActorName");

                    b.HasIndex("LogDate");

                    b.HasIndex("ServiceName");

                    b.ToTable("EventLog");
                });
#pragma warning restore 612, 618
        }
    }
}
