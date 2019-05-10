using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAndStyleOrderPlanning.Pages.Ingredients
{
    public class EditModel : PageModel
    {
        public string PageTitle { get; set; }

        [BindProperty]
        public Ingredient Ingredient { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Products { get; set; }

        private IData<Product> productData;
        private IData<Ingredient> ingredientData;
        private IData<Recipe> recipeData;

        public EditModel(IData<Ingredient> ingredientData, IData<Product> productData, IData<Recipe> recipeData)
        {
            this.ingredientData = ingredientData;
            this.productData = productData;
            this.recipeData = recipeData;
        }

        private bool LoadData(int recipeId, int? ingredientId)
        {
            Recipe recipe = recipeData.GetById(recipeId);

            if (recipe == null)
                return false;

            if (ingredientId == null)
            {
                PageTitle = "Προσθήκη Νέου Υλικού";
                Ingredient = new Ingredient();
                Ingredient.RecipeId = recipeId;

                // Present products not already used in the recipe!
                Products = productData.GetByName(null).
                    Where(p => !recipe.Ingredients.Select(i => i.ProductId).Contains(p.Id)).
                    OrderBy(i=>i.Name).
                    Select(p => new SelectListItem() { Text = $"{p.Name} σε {p.MeasuringUnit}", Value = p.Id.ToString() });
            }
            else
            {
                PageTitle = "Επεξεργασία Υλικού";
            }
            return true;
        }

        public IActionResult OnGet(int recipeId, int? ingredientId)
        {
            if (!LoadData(recipeId, ingredientId))
                return RedirectToPage("/Recipes/List");

            if (Products.Count() == 0)
                return RedirectToPage("/Recipes/Edit", new { id = recipeId });

            return Page();
        }

        public IActionResult OnPost(int recipeId, int? ingredientId)
        {
            if (!ModelState.IsValid)
            {
                if (!LoadData(recipeId, ingredientId))
                    return RedirectToPage("/Recipes/List");
                return Page();
            }

            if (Ingredient.RecipeId < 1)
            {
                Ingredient.RecipeId = recipeId;
                ingredientData.Add(Ingredient);
            }
            else
            {

            }

            ingredientData.Commit();

            return RedirectToPage("/Recipes/Edit", new { id = recipeId });
        }
    }
}