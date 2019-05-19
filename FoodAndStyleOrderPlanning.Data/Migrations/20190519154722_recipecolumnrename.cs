using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodAndStyleOrderPlanning.Data.Migrations
{
    public partial class recipecolumnrename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultingQuntityInKilograms",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "ResultingQuantityInGrams",
                table: "Recipes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultingQuantityInGrams",
                table: "Recipes");

            migrationBuilder.AddColumn<float>(
                name: "ResultingQuntityInKilograms",
                table: "Recipes",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
