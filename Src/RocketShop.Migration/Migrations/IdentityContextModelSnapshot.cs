﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RocketShop.Database.EntityFramework;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.AdditionalPayroll", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PayrollId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("PayrollId");

                    b.ToTable("AdditionalPayroll");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.ChangePasswordHistory", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ChangeAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Reset")
                        .HasColumnType("boolean");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Reset");

                    b.HasIndex("UserId");

                    b.ToTable("ChangePasswordHistories");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("ApplicationAdmin")
                        .HasColumnType("boolean");

                    b.Property<bool>("CreateEmployee")
                        .HasColumnType("boolean");

                    b.Property<bool>("CreateFixtureRequest")
                        .HasColumnType("boolean");

                    b.Property<bool>("CreateProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("DeleteProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("EndFixtureRequest")
                        .HasColumnType("boolean");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Sell")
                        .HasColumnType("boolean");

                    b.Property<bool>("SellSpeicalProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("SellerProductManagement")
                        .HasColumnType("boolean");

                    b.Property<bool>("SetResign")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateEmployee")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateFixture")
                        .HasColumnType("boolean");

                    b.Property<bool>("UpdateProduct")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewAnotherSalesValues")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewAnotherUserFixtureData")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewEmployeeDeepData")
                        .HasColumnType("boolean");

                    b.Property<bool>("ViewSaleData")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ApplicationAdmin = true,
                            CreateEmployee = true,
                            CreateFixtureRequest = true,
                            CreateProduct = true,
                            DeleteProduct = true,
                            EndFixtureRequest = true,
                            RoleName = "Application Starter",
                            Sell = true,
                            SellSpeicalProduct = true,
                            SellerProductManagement = true,
                            SetResign = true,
                            UpdateEmployee = true,
                            UpdateFixture = true,
                            UpdateProduct = true,
                            ViewAnotherSalesValues = true,
                            ViewAnotherUserFixtureData = true,
                            ViewEmployeeDeepData = true,
                            ViewSaleData = true
                        });
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("EmployeeCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastLoggedIn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdateBy")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderName")
                        .HasColumnType("text");

                    b.Property<bool>("Resigned")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmployeeCode")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("ProviderKey");

                    b.HasIndex("ProviderName");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserAddiontialExpense", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<int?>("Month")
                        .HasColumnType("integer");

                    b.Property<bool>("OneTimePay")
                        .HasColumnType("boolean");

                    b.Property<bool>("Paid")
                        .HasColumnType("boolean");

                    b.Property<string>("PreiodType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AddiontialExpense");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserFinancal", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BanckName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ProvidentFund")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SocialSecurites")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAddiontialExpense")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalPayment")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TravelExpenses")
                        .HasColumnType("numeric");

                    b.HasKey("UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFinancal");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserInformation", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BrithDay")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreateBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CurrentPosition")
                        .HasColumnType("text");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<char?>("Gender")
                        .HasColumnType("character(1)");

                    b.Property<string>("LastUpdateBy")
                        .HasColumnType("text");

                    b.Property<string>("ManagerId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResignDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("StartWorkDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId");

                    b.HasIndex("ManagerId");

                    b.ToTable("UserInformations");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserPayroll", b =>
                {
                    b.Property<string>("PayRollId")
                        .HasColumnType("text");

                    b.Property<DateTime>("PayrollDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("ProvidentFund")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SocialSecurites")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAdditionalPay")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalPayment")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TravelExpenses")
                        .HasColumnType("numeric");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PayRollId");

                    b.HasIndex("UserId");

                    b.ToTable("Payroll");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserProvidentFund", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("Currenct")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("ProvidentFund");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("RoleId", "UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.Views.UserFinancialView", b =>
                {
                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("AccumulatedProvidentFund")
                        .HasColumnType("numeric");

                    b.Property<string>("BanckName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("ProvidentFundPerMonth")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SocialSecurites")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalAddiontialExpense")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TotalPayment")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TravelExpenses")
                        .HasColumnType("numeric");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.ToTable((string)null);

                    b.ToView("V_UFinacial", (string)null);
                });

            modelBuilder.Entity("RocketShop.Database.Model.Identity.Views.UserView", b =>
                {
                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Department")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmployeeCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ManagerId")
                        .HasColumnType("text");

                    b.Property<string>("Position")
                        .HasColumnType("text")
                        .HasColumnName("CurrentPosition");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("Id");

                    b.ToTable((string)null);

                    b.ToView("V_User", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RocketShop.Database.Model.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RocketShop.Database.Model.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RocketShop.Database.Model.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RocketShop.Database.Model.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
