using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoodAndStyleOrderPlanning.Pages.Recipes
{
    public class ListModel : PageModel
    {
        private IData<Recipe> recipeData;
        private readonly IData<OrderRecipeItem> orderRecipeItemData;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public string SortOrderDirection { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        public IEnumerable<int> RecipeIdsInOrders { get; set; }

        public ListModel(IData<Recipe> recipeData, IData<OrderRecipeItem> orderRecipeItemData)
        {
            this.recipeData = recipeData;
            this.orderRecipeItemData = orderRecipeItemData;
        }

        public void OnGet(int productId, string orderBy, string orderDir)
        {
            Recipes = recipeData.GetByName(SearchTerm);

            if (productId > 0)
            {
                // need to filter Recipes
                Recipes = Recipes.Where(r => r.Ingredients.Select(i=>i.ProductId).Contains(productId));
            }

            if (orderBy == "createdDate")
                if (orderDir == "desc")
                    Recipes = Recipes.OrderByDescending(r => r.CreatedOn);
                else
                    Recipes = Recipes.OrderBy(r => r.CreatedOn);
            else
                if (orderDir == "desc")
                    Recipes = Recipes.OrderByDescending(r => r.Name);
                else
                    Recipes = Recipes.OrderBy(r => r.Name);

            if (orderDir == "asc")
                SortOrderDirection = "desc";
            else
                SortOrderDirection = "asc";

            RecipeIdsInOrders = orderRecipeItemData.GetByName("").Where(o=>o.Quantity>0).Select(o=>o.RecipeId).Distinct();
        }
    }
}