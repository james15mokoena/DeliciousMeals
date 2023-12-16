using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliciousMeals.Migrations
{
    /// <inheritdoc />
    public partial class AddedPurchasedMealtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchasedMeal",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type:"int",nullable:false).Annotation("SqlServer:Identity","1,1"),
                    MealId = table.Column<int>(type:"int",nullable:false),
                    MealName = table.Column<string>(type:"nvarchar(450)",nullable:false),
                    Quantity = table.Column<int>(type:"int",nullable:false),
                    DatePurchased = table.Column<DateTime>(type:"datetime2",nullable:false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedMeal", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_PurchasedMeal_Meal_MealId",
                        column: x => x.MealId,
                        principalTable: "Meal",
                        principalColumn: "MealId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedMeal_Meal",
                table: "Meal",
                column: "MealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchasedMeal");
        }
    }
}
