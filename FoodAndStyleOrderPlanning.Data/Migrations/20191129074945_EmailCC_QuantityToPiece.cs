using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodAndStyleOrderPlanning.Data.Migrations
{
    public partial class EmailCC_QuantityToPiece : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "EmailCC",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityToPiece",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailCC",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "QuantityToPiece",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Suppliers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
