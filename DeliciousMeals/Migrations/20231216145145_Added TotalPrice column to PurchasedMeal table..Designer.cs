﻿// <auto-generated />
using System;
using DeliciousMeals.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliciousMeals.Migrations
{
    [DbContext(typeof(DeliciousMealsDbContext))]
    [Migration("20231216145145_Added TotalPrice column to PurchasedMeal table.")]
    partial class AddedTotalPricecolumntoPurchasedMealtable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DeliciousMeals.Models.Administrator", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Administrator");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<string>("MealImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.HasIndex("Email");

                    b.HasIndex("MealId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Customer", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Email");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"));

                    b.Property<DateTime>("DateGenerated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<string>("MealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("InvoiceId");

                    b.HasIndex("Email");

                    b.HasIndex("MealId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Meal", b =>
                {
                    b.Property<int>("MealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MealId"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsAvailable")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MealId");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime>("DateOrdered")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IsCollected")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<string>("IsReady")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<string>("MealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("TimeCompleted")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderId");

                    b.HasIndex("Email");

                    b.HasIndex("MealId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("DeliciousMeals.Models.PurchasedMeal", b =>
                {
                    b.Property<int>("PurchaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PurchaseId"));

                    b.Property<DateTime>("DatePurchased")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<string>("MealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PurchaseId");

                    b.HasIndex("Email");

                    b.HasIndex("MealId");

                    b.ToTable("PurchasedMeal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Review", b =>
                {
                    b.Property<int>("RevId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RevId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("MealId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RevId");

                    b.HasIndex("Email");

                    b.HasIndex("MealId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Cart", b =>
                {
                    b.HasOne("DeliciousMeals.Models.Customer", "Customer")
                        .WithMany("CartItems")
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliciousMeals.Models.Meal", "Meal")
                        .WithMany("Carts")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Invoice", b =>
                {
                    b.HasOne("DeliciousMeals.Models.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliciousMeals.Models.Meal", "Meal")
                        .WithMany("Invoices")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Order", b =>
                {
                    b.HasOne("DeliciousMeals.Models.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliciousMeals.Models.Meal", "Meal")
                        .WithMany("Orders")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.PurchasedMeal", b =>
                {
                    b.HasOne("DeliciousMeals.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliciousMeals.Models.Meal", "Meal")
                        .WithMany()
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Review", b =>
                {
                    b.HasOne("DeliciousMeals.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliciousMeals.Models.Meal", "Meal")
                        .WithMany()
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Customer", b =>
                {
                    b.Navigation("CartItems");

                    b.Navigation("Invoices");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("DeliciousMeals.Models.Meal", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Invoices");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
