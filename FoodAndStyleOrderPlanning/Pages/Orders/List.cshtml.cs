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
    public class ListModel : PageModel
    {
        [BindProperty]
        public IEnumerable<Order> Orders { get; set; }

        public int RecipeId { get; set; }
        public string OrdersWithRecipeText { get; set; }

        private readonly IData<Order> orderData;
        private readonly IData<Recipe> recipeData; 

        public ListModel(IData<Order> orderData, IData<Recipe> recipeData)
        {
            this.orderData = orderData;
            this.recipeData = recipeData;
        }
        
        public IActionResult OnGet(int recipeId)
        {
            Orders = orderData.GetByName(null).OrderByDescending(o => o.CreatedOn).ThenBy(o => o.Name);

            if (recipeId < 1)
                return Page();

            var recipe = recipeData.GetByName("").Where(r => r.Id == recipeId).FirstOrDefault();

            if (recipe == null)
                return RedirectToPage("./List");

            RecipeId = recipeId;

            List<int> orderIds = new List<int>();
            foreach (Order o in Orders)
                if (o.OrderRecipeItems.Select(r => r.RecipeId).Contains(recipeId))
                    orderIds.Add(o.Id);

            Orders = Orders.Where(o => orderIds.Contains(o.Id)).ToList();

            if (Orders.Count() == 0)
                return RedirectToPage("./List");

            if (Orders.Count() == 1)
                OrdersWithRecipeText = $"Η παρακάτω παραγγελία περιέχει την συνταγή '{recipe.Name}'";
            else
                OrdersWithRecipeText = $"Οι παρακάτω παραγγελίες περιέχουν την συνταγή '{recipe.Name}'";
            
            return Page();
        }
    }
}