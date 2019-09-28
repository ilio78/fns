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
        public string SortOrderDirection { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }

        public ListModel(IConfiguration config, IData<Supplier> data)
        {
            this.config = config;
            this.data = data;
        }

        public void OnGet(string orderBy, string orderDir)
        {
            if (orderBy == "email")
                if (orderDir == "desc")
                    Suppliers = data.GetByName(SearchTerm).OrderByDescending(r => r.Email);
                else
                    Suppliers = data.GetByName(SearchTerm).OrderBy(r => r.Email);
            else
               if (orderDir == "desc")
                Suppliers = data.GetByName(SearchTerm).OrderByDescending(r => r.Name);
            else
                Suppliers = data.GetByName(SearchTerm).OrderBy(r => r.Name);

            if (orderDir == "asc")
                SortOrderDirection = "desc";
            else
                SortOrderDirection = "asc";
        }
    }
}