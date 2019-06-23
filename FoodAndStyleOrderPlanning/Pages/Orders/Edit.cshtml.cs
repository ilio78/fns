using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Pages.Orders
{

    public class EditModel : PageModel
    {
        [BindProperty]
        public Order Order { get; set; }

        [BindProperty]
        public RecipeChoicesViewModel RecipeChoices{ get; set; }

       
        private readonly IData<Recipe> recipeData;
        private readonly IData<Order> orderData;
        private readonly IData<OrderRecipeItem> orderRecipeItemData;
        private readonly IData<Product> productData;

        public IList<OrderProductItem> OrderProductItems { get; set; }

        public EditModel(IData<Recipe> recipeData, IData<Order> orderData, IData<OrderRecipeItem> orderRecipeItemData, IData<Product> productData)
        {
            this.recipeData = recipeData;
            this.orderData = orderData;
            this.orderRecipeItemData = orderRecipeItemData;
            this.productData = productData;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                Order = new Order();
                Order.CreatedBy = "temp";
                Order.UpdatedBy = "temp";
                RecipeChoices = new RecipeChoicesViewModel();
                OrderProductItems = new List<OrderProductItem>();
                return Page();
            }

            Order = orderData.GetById(id.Value);
            if (Order == null)
                return RedirectToPage("./List");

            var Recipes = recipeData.GetByName(null).ToList();

            RecipeChoices = new RecipeChoicesViewModel();
       
            int i = 1;
            foreach (Recipe r in Recipes)
                RecipeChoices.Choices.Add(new ChoiceViewModel()
                {
                    Id = i++, Name = r.Name, RecipeResultingQuantity = String.Format("{0:n0}", r.ResultingQuantityInGrams),
                    RecipeId = r.Id
                });
            
            var alreadySelected = orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id).ToList();

            foreach (var c in RecipeChoices.Choices)
            {
                foreach(var orderRecipeItem in alreadySelected.Where(o => o.RecipeId == c.RecipeId))
                {
                    switch (orderRecipeItem.Day)
                    {
                        case DayOfWeek.Monday:
                            c.OrderQuantity_Monday = orderRecipeItem.Quantity;
                            break;
                        case DayOfWeek.Tuesday:
                            c.OrderQuantity_Tuesday = orderRecipeItem.Quantity;
                            break;
                        case DayOfWeek.Wednesday:
                            c.OrderQuantity_Wednesday = orderRecipeItem.Quantity;
                            break;
                        case DayOfWeek.Thursday:
                            c.OrderQuantity_Thursday = orderRecipeItem.Quantity;
                            break;
                        case DayOfWeek.Friday:
                            c.OrderQuantity_Friday = orderRecipeItem.Quantity;
                            break;
                    }
                }
            }

            List<int> recipeIdsWithQuantityOverZero = RecipeChoices.Choices.Where(r => r.OrderQuantity_Monday > 0).Select(r => r.RecipeId).ToList();

            OrderProductItems = new List<OrderProductItem>();

            foreach (Recipe recipe in Recipes.Where(r=> recipeIdsWithQuantityOverZero.Contains(r.Id)))
            {
                foreach(Ingredient ingredient in recipe.Ingredients)
                {
                    OrderProductItem item = OrderProductItems.SingleOrDefault(o => o.Product.Id == ingredient.Product.Id);
                    if (item == null)
                    {
                        item = new OrderProductItem() { Product = ingredient.Product, Supplier = ingredient.Product.Supplier };
                        OrderProductItems.Add(item);
                    }

                    item.Quantity += RecipeChoices.Choices.Single(r=>r.RecipeId==recipe.Id).OrderQuantity_Monday * ingredient.Quantity;                    
                }
            }
            
            OrderProductItems = OrderProductItems.OrderBy(o => o.Product.ProductType).ThenBy(o => o.Product.Name).ToList();                            

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Order.Id == 0)
            {
                // First save only creates order entry in the DB! Does not add any order items yet....
                Order.CreatedOn = DateTime.Now;
                Order.UpdatedOn = Order.CreatedOn;
                Order.CreatedBy = Helpers.User.GetUPN(HttpContext);
                Order.UpdatedBy = Helpers.User.GetUPN(HttpContext);
                orderData.Add(Order);
            }
            else
            {
                // Remove previous entries as new lines will be entered!
                orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id).ToList()
                    .ForEach(o=>  orderRecipeItemData.Delete(o.Id));
                orderRecipeItemData.Commit();

                // Create order item lines per day
                foreach (ChoiceViewModel item in RecipeChoices.Choices)
                {
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Monday, DayOfWeek.Monday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Tuesday, DayOfWeek.Tuesday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Wednesday, DayOfWeek.Wednesday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Thursday, DayOfWeek.Thursday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Friday, DayOfWeek.Friday);
                }

                Order.UpdatedOn = DateTime.Now;
                Order.UpdatedBy = Helpers.User.GetUPN(HttpContext);
                orderData.Update(Order);
            }

            orderData.Commit();
            return RedirectToPage("./Edit", new { id = Order.Id });

        }

        private void CreateOrderItemLines(int recipeId, int quantity, DayOfWeek day)
        {
            if (quantity < 1)
                return;

            OrderRecipeItem orderRecipeItem = new OrderRecipeItem();
            orderRecipeItem.OrderId = Order.Id;
            orderRecipeItem.Quantity = quantity;
            orderRecipeItem.RecipeId = recipeId;
            orderRecipeItem.Day = day;
            orderRecipeItemData.Add(orderRecipeItem);
            orderRecipeItemData.Commit();
        }
    }
}