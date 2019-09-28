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
    public class ToggleActiveModel : PageModel
    {   private IData<Recipe> recipeData;


        public ToggleActiveModel(IData<Recipe> recipeData)
        {
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(int id)
        {
            Recipe recipe = recipeData.GetById(id);
            if (recipe == null)
                return RedirectToPage("./List");

            recipe.IsActive = !recipe.IsActive;

            recipeData.Update(recipe);
            recipeData.Commit();
            
            return RedirectToPage("./List");
        }

    }
}