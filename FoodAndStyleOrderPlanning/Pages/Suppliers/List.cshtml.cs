using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoodAndStyleOrderPlanning.Pages.Suppliers
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private IData<Supplier> data;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }

        public ListModel(IConfiguration config, IData<Supplier> data)
        {
            this.config = config;
            this.data = data;
        }

        public void OnGet()
        {
            Suppliers = data.GetByName(SearchTerm).OrderBy(s => s.Name);
        }
    }
}