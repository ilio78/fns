using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAndStyleOrderPlanning.Pages.Recipes
{
    public class EditModel : PageModel
    {
        //private readonly IDataFull fullData;
        private readonly IData<Recipe> recipeData;

        [BindProperty]
        public int RecipeId { get; set; }
        [BindProperty]
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string RecipeName { get; set; }

        [BindProperty]
        [Required]
        [Range(50, 99999)]
        public int RecipeQuantity { get; set; }

        public DateTime RecipeCreatedOn { get; set; }
        public DateTime RecipeUpdatedOn { get; set; }

        public SelectList RecipeType { get; set; }

        private Recipe Recipe;

        [BindProperty]
        public RecipeViewModel RecipeViewModel { get; set; }

        public string PageTitle { get; set; }

        public List<RecipeIngredientViewModel> RecipeIngredients { get; set; }

        public EditModel(IData<Recipe> recipeData)
        {
            this.recipeData = recipeData;
        }

        private void LoadData()
        {
            RecipeId = Recipe.Id;
            RecipeName = Recipe.Name;
            RecipeQuantity = Recipe.ResultingQuantityInGrams;
            RecipeCreatedOn = Recipe.CreatedOn;
            RecipeUpdatedOn = Recipe.UpdatedOn;
            RecipeViewModel = new RecipeViewModel(Recipe); 

            RecipeIngredients = new List<RecipeIngredientViewModel>();
            foreach (Ingredient i in Recipe.Ingredients.OrderBy(i=>i.Product.Name))
                RecipeIngredients.Add(new RecipeIngredientViewModel() {
                    IngredientId = i.Id, Quantity=(int)i.Quantity, MeasuringUnit = i.Product.MeasuringUnit,
                        ProductName = i.Product.Name, ProductId = i.Product.Id, ProductSupplierName = i.Product?.Supplier.Name });
        }

        public IActionResult OnGet(int? id)
        {
            RecipeType = new SelectList((IEnumerable)LanguageResources.RecipeTypeTranslations.OrderBy(s => s.Value), "Key", "Value");

            if (id == null)
            {
                PageTitle = "Προσθήκη Νέας Συνταγής";
                RecipeIngredients = new List<RecipeIngredientViewModel>();
                RecipeViewModel = new RecipeViewModel();
            }
            else
            {
                PageTitle = "Επεξεργασία Συνταγής";
                Recipe = recipeData.GetById(id.Value);

                if (Recipe == null)
                    RedirectToPage("./List");

                LoadData();
            }
           
            return Page();
        }

        public IActionResult OnPost()
        {
            RecipeIngredients = new List<RecipeIngredientViewModel>();

            if (!ModelState.IsValid)
                return Page();

            if (RecipeId > 0)
            {
                Recipe = recipeData.GetById(RecipeId);

                if (Recipe == null)
                    RedirectToPage("./List");

                SetRecipeProperties(Recipe, DateTime.Now);
                recipeData.Update(Recipe);
            }
            else
            {
                Recipe = new Recipe();
                Recipe.CreatedOn = DateTime.Now;
                Recipe.IsActive = true;
                SetRecipeProperties(Recipe, Recipe.CreatedOn);
                recipeData.Add(Recipe);
            }

            recipeData.Commit();
            return RedirectToPage("./Edit", new { id = Recipe.Id });
        }

        private void SetRecipeProperties(Recipe recipe, DateTime updatedOn)
        {
            recipe.Name = RecipeName.Trim();
            recipe.ResultingQuantityInGrams = RecipeQuantity;
            recipe.RecipeType = RecipeViewModel.RecipeType;
            recipe.UpdatedOn = updatedOn;
        }
    }
}