using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FoodAndStyleOrderPlanning.Core
{

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }
    }

    public class Recipe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public int ResultingQuantityInGrams { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
    }

    public class Ingredient
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        [Required]
        [Range(1,9999)]
        public int Quantity { get; set; }
    }

    //public class RecipeViewModel
    //{
    //    public int Id { get; set; }

    //    public string Name { get; set; }

    //    public float ResultingQuntityInKilograms { get; set; }

    //    public List<IngredientViewModel> Ingredients { get; set; }
    //}

    //public class IngredientViewModel
    //{
    //    public int ProductId { get; set; }
    //    public float Quantity { get; set; }

    //    public MeasuringUnit MeasuringUnit { get; set; }
    //}

    public class RecipeIngredientViewModel
    {
        public int IngredientId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public MeasuringUnit MeasuringUnit { get; set; }
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public ICollection<OrderRecipeItem> OrderRecipeItems { get; set; }
    }

    public class RecipeChoicesViewModel
    {
        public RecipeChoicesViewModel()
        {
            Choices = new List<ChoiceViewModel>();
        }

        public IList<ChoiceViewModel> Choices { get; set; }
    }

    public class ChoiceViewModel
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string RecipeQuantity { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderProductItem
    {
        public Product Product { get; set; }
        public float Quantity { get; set; }
    }



    public class OrderRecipeItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }

        [Required]
        public Recipe Recipe { get; set; }

        [Required]
        [Range(0,9999)]
        public int Quantity { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
