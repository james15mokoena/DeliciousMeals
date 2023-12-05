using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliciousMeals.Migrations
{
    /// <inheritdoc />
    public partial class AddedSizeColumnMealTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Meal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Meal");
        }
    }
}
