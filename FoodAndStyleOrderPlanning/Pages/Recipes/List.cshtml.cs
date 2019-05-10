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
        private readonly IConfiguration config;
        private IData<Recipe> data;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IEnumerable<Recipe> Recipes { get; set; }

        public ListModel(IConfiguration config, IData<Recipe> data)
        {
            this.config = config;
            this.data = data;
        }

        public void OnGet()
        {
            Recipes = data.GetByName(SearchTerm);
        }
    }
}