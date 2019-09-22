using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Recipes
{
    public class CopyModel : PageModel
    {
        private IData<Product> productData;
        private IData<Ingredient> ingredientData;
        private IData<Recipe> recipeData;


        public CopyModel(IData<Ingredient> ingredientData, IData<Product> productData, IData<Recipe> recipeData)
        {
            this.ingredientData = ingredientData;
            this.productData = productData;
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(int id)
        {
            Recipe recipe = recipeData.GetById(id);
            if (recipe == null)
                return RedirectToPage("./List");

            Recipe recipeCopy = new Recipe();
            recipeCopy.Name = recipe.Name + " - Copy";
            recipeCopy.CreatedOn = DateTime.Now;
            recipeCopy.UpdatedOn = DateTime.Now;
            recipeCopy.ResultingQuantityInGrams = recipe.ResultingQuantityInGrams;
            recipeData.Add(recipeCopy);
            recipeData.Commit();

            foreach(Ingredient i in recipe.Ingredients)
            {
                Ingredient newIngredient = new Ingredient();
                newIngredient.Quantity = i.Quantity;
                newIngredient.RecipeId = recipeCopy.Id;
                newIngredient.ProductId = i.ProductId;
                ingredientData.Add(newIngredient);
            }
            ingredientData.Commit();

            return RedirectToPage("./List");
        }

    }
}