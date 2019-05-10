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
    public class DeleteModel : PageModel
    {
        private readonly IData<Recipe> recipeData;

        public Recipe Recipe { get; set; }

        public DeleteModel(IData<Recipe> recipeData)
        {
            this.recipeData = recipeData;
        }

        public IActionResult OnGet(int id)
        {
            Recipe = recipeData.GetById(id);
            if (Recipe == null)
                return RedirectToPage("./List");
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var restaurant = recipeData.Delete(id);
            recipeData.Commit();
            if (restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = "Restaurant deleted";
            return RedirectToPage("./List");

        }
    }
}