﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TradeCore.AuthService.Repository;

#nullable disable

namespace TradeCore.AuthService.Migrations
{
    [DbContext(typeof(AuthDbContext))]
    partial class AuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TradeCore.AuthService.Domain.AppCustomerAggregate.AppCustomer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<Guid?>("CreatedCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("IsActive")
                        .HasColumnType("smallint");

                    b.Property<bool>("IsSystemAdmin")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("SessionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("AppCustomers");
                });

            modelBuilder.Entity("TradeCore.AuthService.Domain.AppCustomerClaimAggregate.AppCustomerClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppOperationClaimId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CreatedCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<short>("IsActive")
                        .HasColumnType("smallint");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("AppCustomerId");

                    b.ToTable("AppCustomerClaims");
                });

            modelBuilder.Entity("TradeCore.AuthService.Domain.AppOperationClaimAggregate.AppOperationClaim", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CreatedCustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FunctionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("IsActive")
                        .HasColumnType("smallint");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("ParentFunctionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AppOperationClaims");
                });

            modelBuilder.Entity("TradeCore.AuthService.Domain.AppCustomerClaimAggregate.AppCustomerClaim", b =>
                {
                    b.HasOne("TradeCore.AuthService.Domain.AppCustomerAggregate.AppCustomer", null)
                        .WithMany("AppCustomerClaims")
                        .HasForeignKey("AppCustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("TradeCore.AuthService.Domain.AppCustomerAggregate.AppCustomer", b =>
                {
                    b.Navigation("AppCustomerClaims");
                });
#pragma warning restore 612, 618
        }
    }
}
