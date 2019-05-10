using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoodAndStyleOrderPlanning.Pages.Restaurants
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private IData<Restaurant> restaurantData;

        public string Message { get; set; }
        public IEnumerable<Restaurant> Restaurants;

        [BindProperty(SupportsGet=true)]
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, IData<Restaurant> restaurantData)
        {
            this.config = config;
            this.restaurantData = restaurantData;
        }

        public void OnGet()
        {
            Message = config["Message"];
            Restaurants = restaurantData.GetByName(SearchTerm);
        }
    }
}