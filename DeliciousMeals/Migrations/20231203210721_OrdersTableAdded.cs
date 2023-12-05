using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliciousMeals.Migrations
{
    /// <inheritdoc />
    public partial class OrdersTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Order",
               columns: table => new
               {
                   OrderId = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   MealId = table.Column<int>(type: "int", nullable: false),
                   DateOrdered = table.Column<DateTime>(type: "datetime2", nullable: false),
                   IsReady = table.Column<string>(type: "char(1)", nullable: false),
                   IsCollected = table.Column<string>(type: "char(1)", nullable: false),
                   TimeCompleted = table.Column<DateTime>(type: "datetime2", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Order", x => x.OrderId);
                   table.ForeignKey(
                       name: "FK_Order_Customer_Email",
                       column: x => x.Email,
                       principalTable: "Customer",
                       principalColumn: "Email",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_Order_Meal_MealId",
                       column: x => x.MealId,
                       principalTable: "Meal",
                       principalColumn: "MealId",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateIndex(
                name: "IX_Order_Email",
                table: "Order",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Order_MealId",
                table: "Order",
                column: "MealId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Order");
        }
    }
}
