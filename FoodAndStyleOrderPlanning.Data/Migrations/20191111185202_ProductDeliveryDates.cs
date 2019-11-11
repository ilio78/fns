using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodAndStyleOrderPlanning.Data.Migrations
{
    public partial class ProductDeliveryDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeType",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnFriday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnMonday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnSaturday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnSunday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnThursday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnTuesday",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryOnWednesday",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipeType",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "DeliveryOnFriday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnMonday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnSaturday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnSunday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnThursday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnTuesday",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeliveryOnWednesday",
                table: "Products");
        }
    }
}
