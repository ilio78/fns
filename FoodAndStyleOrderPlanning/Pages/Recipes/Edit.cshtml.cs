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

        [BindProperty]
        public RecipeViewModel RecipeViewModel { get; set; }

        public string PageTitle { get; set; }

        public List<RecipeIngredientViewModel> RecipeIngredients { get; set; }

        public EditModel(IData<Recipe> recipeData)
        {
            this.recipeData = recipeData;
        }

        private void LoadData(int id)
        {
            var recipe = recipeData.GetById(id);

            RecipeId = recipe.Id;
            RecipeName = recipe.Name;
            RecipeQuantity = recipe.ResultingQuantityInGrams;
            RecipeCreatedOn = recipe.CreatedOn;
            RecipeUpdatedOn = recipe.UpdatedOn;
            RecipeViewModel = new RecipeViewModel(recipe); 

            RecipeIngredients = new List<RecipeIngredientViewModel>();
            foreach (Ingredient i in recipe.Ingredients.OrderBy(i=>i.Product.Name))
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
                LoadData(id.Value);
            }
           
            return Page();
        }

        public IActionResult OnPost()
        {
            RecipeIngredients = new List<RecipeIngredientViewModel>();

            if (!ModelState.IsValid)
                return Page();

            Recipe recipe = null;

            if (RecipeId > 0)
            {
                recipe = recipeData.GetById(RecipeId);
                SetRecipeProperties(recipe, DateTime.Now);
                recipeData.Update(recipe);
            }
            else
            {
                recipe = new Recipe();
                recipe.CreatedOn = DateTime.Now;
                recipe.IsActive = true;
                SetRecipeProperties(recipe, recipe.CreatedOn);
                recipeData.Add(recipe);
            }

            recipeData.Commit();

            return RedirectToPage("./Edit", new { id = recipe.Id });

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