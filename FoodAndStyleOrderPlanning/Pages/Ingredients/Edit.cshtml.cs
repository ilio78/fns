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
        private Recipe CurrentRecipe;

        public EditModel(IData<Ingredient> ingredientData, IData<Product> productData, IData<Recipe> recipeData)
        {
            this.ingredientData = ingredientData;
            this.productData = productData;
            this.recipeData = recipeData;
        }

        private bool LoadData(int recipeId, int? ingredientId)
        {
            CurrentRecipe = recipeData.GetById(recipeId);

            if (CurrentRecipe == null)
                return false;

            if (ingredientId == null)
            {
                PageTitle = "Προσθήκη Νέου Υλικού";
                Ingredient = new Ingredient();
                Ingredient.RecipeId = recipeId;

                // Present products not already used in the recipe!
                Products = productData.GetByName(null).
                    Where(p => !CurrentRecipe.Ingredients.Select(i => i.ProductId).Contains(p.Id)).
                    OrderBy(i=>i.Name).                    
                    Select(p => new SelectListItem() { Text = $"{p.Name} σε {LanguageResources.MeasuringUnitTranslations.First(t=>t.Key==((int)p.MeasuringUnit).ToString()).Value}", Value = p.Id.ToString() });
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
            
            ingredientData.Commit();
            CurrentRecipe = recipeData.GetById(recipeId);
            CurrentRecipe.UpdatedOn = DateTime.Now;
            recipeData.Update(CurrentRecipe);
            recipeData.Commit();

            return RedirectToPage("/Recipes/Edit", new { id = recipeId });
        }
    }
}