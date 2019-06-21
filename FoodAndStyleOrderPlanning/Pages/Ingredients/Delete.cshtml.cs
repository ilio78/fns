using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Ingredients
{
    public class DeleteModel : PageModel
    {
        private IData<Ingredient> ingredientData;
        private IData<Recipe> recipeData;

        public DeleteModel(IData<Ingredient> ingredientData, IData<Recipe> recipeData)
        {
            this.ingredientData = ingredientData;
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(int recipeId, int ingredientId)
        {
            Recipe recipe = recipeData.GetById(recipeId);

            if (recipe == null)
                return RedirectToPage("/Recipes/List");

            Ingredient ingredient = recipe.Ingredients.FirstOrDefault(i => i.Id == ingredientId);

            if (ingredient == null)
                return RedirectToPage("/Recipes/Edit", recipeId);

            ingredientData.Delete(ingredientId);           
            ingredientData.Commit();
            recipe.UpdatedOn = DateTime.Now;
            recipeData.Update(recipe);
            recipeData.Commit();

            return RedirectToPage("/Recipes/Edit", new { id = recipeId });
        }
    }
}