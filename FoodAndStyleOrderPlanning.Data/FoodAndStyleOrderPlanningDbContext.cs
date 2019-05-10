using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodAndStyleOrderPlanning.Data
{
    public class FoodAndStyleOrderPlanningDbContext : DbContext
    {

        public FoodAndStyleOrderPlanningDbContext(DbContextOptions<FoodAndStyleOrderPlanningDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderRecipeItem> OrderRecipeItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}
