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
        private IData<Product> data;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public string SortOrderDirection { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public ListModel(IConfiguration config, IData<Product> data)
        {
            this.config = config;
            this.data = data;
        }

        public void OnGet(string orderBy, string orderDir)
        {
            if (orderBy == "type")
                if (orderDir == "desc")
                    Products = data.GetByName(SearchTerm).OrderByDescending(p => p.ProductType).ThenByDescending(p => p.Name);
                else
                    Products = data.GetByName(SearchTerm).OrderBy(p => p.ProductType).ThenBy(p => p.Name);
            else if (orderBy == "supplier")
                if (orderDir == "desc")
                    Products = data.GetByName(SearchTerm).OrderByDescending(p => p.Supplier.Name).ThenByDescending(p => p.Name);
                else
                    Products = data.GetByName(SearchTerm).OrderBy(p => p.Supplier.Name).ThenBy(p => p.Name);
            else
                if (orderDir == "desc")
                    Products = data.GetByName(SearchTerm).OrderByDescending(p => p.Name);
                else
                    Products = data.GetByName(SearchTerm).OrderBy(p => p.Name);

            if (orderDir == "asc")
                SortOrderDirection = "desc";
            else
                SortOrderDirection = "asc";
        }
    }
}