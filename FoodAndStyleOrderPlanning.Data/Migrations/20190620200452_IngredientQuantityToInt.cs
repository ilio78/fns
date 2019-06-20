using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodAndStyleOrderPlanning.Data.Migrations
{
    public partial class IngredientQuantityToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Ingredients",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Quantity",
                table: "Ingredients",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
