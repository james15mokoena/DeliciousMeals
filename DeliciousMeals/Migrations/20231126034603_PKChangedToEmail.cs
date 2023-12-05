using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliciousMeals.Migrations
{
    /// <inheritdoc />
    public partial class PKChangedToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Customer_CustId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Customer_CustId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_CustId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_CustId",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Cart_CustId",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "Administrator");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Order",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Invoice",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Cart",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Administrator",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Email",
                table: "Order",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Email",
                table: "Invoice",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_Email",
                table: "Cart",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Customer_Email",
                table: "Cart",
                column: "Email",
                principalTable: "Customer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Customer_Email",
                table: "Invoice",
                column: "Email",
                principalTable: "Customer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_Email",
                table: "Order",
                column: "Email",
                principalTable: "Customer",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Customer_Email",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Customer_Email",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_Email",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_Email",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_Email",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Cart_Email",
                table: "Cart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Cart");

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Administrator",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "Administrator",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrator",
                table: "Administrator",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustId",
                table: "Order",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustId",
                table: "Invoice",
                column: "CustId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_CustId",
                table: "Cart",
                column: "CustId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Customer_CustId",
                table: "Cart",
                column: "CustId",
                principalTable: "Customer",
                principalColumn: "CustId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Customer_CustId",
                table: "Invoice",
                column: "CustId",
                principalTable: "Customer",
                principalColumn: "CustId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_CustId",
                table: "Order",
                column: "CustId",
                principalTable: "Customer",
                principalColumn: "CustId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
