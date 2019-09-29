using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Orders
{
    public class ToggleActiveModel : PageModel
    {
        private IData<Order> data;

        public ToggleActiveModel(IData<Order> recipeData)
        {
            this.data = recipeData;
        }

        public IActionResult OnGet(int id)
        {
            Order order = data.GetById(id);

            if (order == null)
                return RedirectToPage("./List");

            order.IsActive = !order.IsActive;

            data.Update(order);
            data.Commit();

            return RedirectToPage("./List");
        }
    }
}