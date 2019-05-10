using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Restaurants
{
    public class DeleteModel : PageModel
    {
        private readonly IData<Restaurant> restaurantData;

        public Restaurant Restaurant { get; set; }

        public DeleteModel(IData<Restaurant> restaurantData)
        {
            this.restaurantData = restaurantData;
        }

        public IActionResult OnGet(int id)
        {
            Restaurant = restaurantData.GetById(id);
            if (Restaurant == null)
                return RedirectToPage("./NotFound");
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var restaurant = restaurantData.Delete(id);
            restaurantData.Commit();
            if (restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = "Restaurant deleted";
            return RedirectToPage("./List");

        }
    }
}