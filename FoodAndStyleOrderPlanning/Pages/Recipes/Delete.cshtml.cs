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
        private readonly IData<OrderRecipeItem> orderRecipeItemData;
        public IEnumerable<int> RecipeIdsInOrders { get; set; }


        public Recipe Recipe { get; set; }

        public DeleteModel(IData<Recipe> recipeData, IData<OrderRecipeItem> orderRecipeItemData)
        {
            this.recipeData = recipeData;
            this.orderRecipeItemData = orderRecipeItemData;
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
            var recipe = recipeData.Delete(id);
            if (recipe == null)
                return RedirectToPage("./List");
            
            if (orderRecipeItemData.GetByName("").Where(o => o.RecipeId == recipe.Id && o.Quantity > 0).Count() > 0)
                return RedirectToPage("./List");

            recipeData.Commit();

            return RedirectToPage("./List");
        }
    }
}