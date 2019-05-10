using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAndStyleOrderPlanning.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IData<Restaurant> restaurantData;
        private IHtmlHelper htmlHelper;

        public IEnumerable<SelectListItem> Cuisines { get; set; }
        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public EditModel(IData<Restaurant> restaurantData, IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int? id)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();

            if (id == null)
                Restaurant = new Restaurant();
            else
                Restaurant = restaurantData.GetById(id.Value);

            if (Restaurant == null)
                return RedirectToPage("./NotFound");

            return Page();

        }

        public IActionResult OnPost()
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();

            if (!ModelState.IsValid)
                return Page();

            if (Restaurant.Id > 0)
                restaurantData.Update(Restaurant);
            else
                restaurantData.Add(Restaurant);

            restaurantData.Commit();
            TempData["Message"] = "Restaurant saved";
            return RedirectToPage("./Detail", new {id = Restaurant.Id});
        }
    }
}