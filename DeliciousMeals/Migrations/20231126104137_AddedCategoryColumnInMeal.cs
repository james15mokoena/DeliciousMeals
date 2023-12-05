﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliciousMeals.Migrations
{
    /// <inheritdoc />
    public partial class AddedCategoryColumnInMeal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Meal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Meal");
        }
    }
}
