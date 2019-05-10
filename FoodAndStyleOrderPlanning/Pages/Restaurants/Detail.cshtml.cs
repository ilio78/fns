using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages
{
    public class DetailModel : PageModel
    {
        public Restaurant Restaurant { get; set; }

        [TempData]
        public string Message { get; set; }

        private readonly IData<Restaurant> restaurantData;

        public DetailModel(IData<Restaurant> restaurantData)
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
    }
}