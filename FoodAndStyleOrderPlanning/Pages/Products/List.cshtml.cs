using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoodAndStyleOrderPlanning.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private IData<Product> productData;
        private IData<Ingredient> ingredientData;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public string SortOrderDirection { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public Dictionary<int, bool> ProductIdContainedInAtLeastOneRecipe { get; set; }

        public ListModel(IConfiguration config, IData<Product> productData, IData<Ingredient> ingredientData)
        {
            this.config = config;
            this.productData = productData;
            this.ingredientData = ingredientData;
        }

        public void OnGet(string orderBy, string orderDir)
        {
            if (orderBy == "type")
                if (orderDir == "desc")
                    Products = productData.GetByName(SearchTerm).OrderByDescending(p => p.ProductType).ThenByDescending(p => p.Name);
                else
                    Products = productData.GetByName(SearchTerm).OrderBy(p => p.ProductType).ThenBy(p => p.Name);
            else if (orderBy == "supplier")
                if (orderDir == "desc")
                    Products = productData.GetByName(SearchTerm).OrderByDescending(p => p.Supplier.Name).ThenByDescending(p => p.Name);
                else
                    Products = productData.GetByName(SearchTerm).OrderBy(p => p.Supplier.Name).ThenBy(p => p.Name);
            else
                if (orderDir == "desc")
                    Products = productData.GetByName(SearchTerm).OrderByDescending(p => p.Name);
                else
                    Products = productData.GetByName(SearchTerm).OrderBy(p => p.Name);

            var productsIdsListedAsIngredients = ingredientData.GetByName("").Select(i=>i.ProductId).ToList();

            ProductIdContainedInAtLeastOneRecipe = new Dictionary<int, bool>();

            foreach (var p in Products)
            {
                ProductIdContainedInAtLeastOneRecipe[p.Id] = productsIdsListedAsIngredients.Contains(p.Id);
            }

            if (orderDir == "asc")
                SortOrderDirection = "desc";
            else
                SortOrderDirection = "asc";
        }
    }
}