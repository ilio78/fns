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
        public RecipeChoicesViewModel RecipeChoices { get; set; }

        public Dictionary<ProductDeliveryDay, Dictionary<Product, int>> ProductDeliveryPerDay { get; set; }

        public IList<OrderProductItem> OrderProductItems { get; set; }

        private readonly IData<Recipe> recipeData;
        private readonly IData<Order> orderData;
        private readonly IData<OrderRecipeItem> orderRecipeItemData;
        private readonly IData<Product> productData;

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
                Order.CreatedBy = "";
                Order.UpdatedBy = "";
                RecipeChoices = new RecipeChoicesViewModel();
                OrderProductItems = new List<OrderProductItem>();
                ProductDeliveryPerDay = new Dictionary<ProductDeliveryDay, Dictionary<Product, int>>();
                return Page();
            }

            Order = orderData.GetById(id.Value);
            if (Order == null)
                return RedirectToPage("./List");

            var Recipes = recipeData.GetByName(null).ToList();

            RecipeChoices = new RecipeChoicesViewModel();

            // RecipeChoicesViewModel only contains a list of ChoiceViewModel items. They are the rows of the recipe quantity table.
            int i = 1;
            foreach (Recipe r in Recipes)
            {
                RecipeChoices.Choices.Add(new ChoiceViewModel()
                {
                    Id = i++, Name = r.Name, RecipeResultingQuantity = String.Format("{0:n0}", r.ResultingQuantityInGrams),
                    RecipeId = r.Id, IsActive = r.IsActive
                });
            }

            // Get only recipe quantity data related to this order only - Even new orders must have been saved first so an OrderId will exist.
            var orderRecipes = orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id).ToList();

            foreach (var c in RecipeChoices.Choices)
            {
                // Scan each recipe to see if a quantity for a particular days is set. For a new order this will not find anything!
                foreach(var orderRecipe in orderRecipes.Where(o => o.RecipeId == c.RecipeId))
                {
                    switch (orderRecipe.Day)
                    {
                        case OrderDay.Monday:
                            c.OrderQuantity_Monday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Tuesday:
                            c.OrderQuantity_Tuesday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Wednesday:
                            c.OrderQuantity_Wednesday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Thursday:
                            c.OrderQuantity_Thursday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Friday:
                            c.OrderQuantity_Friday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Saturday:
                            c.OrderQuantity_Saturday = orderRecipe.Quantity;
                            break;
                        case OrderDay.Sunday:
                            c.OrderQuantity_Sunday = orderRecipe.Quantity;
                            break;
                    }
                }
            }

            // Now that we know the quantities for each recipe we can remove those that are inactive and have 0 total quantity.
            // If they have more than 0 quantity we still need to show them as they were previously chosen!
            RecipeChoices.Choices.RemoveAll(c => !c.IsActive && c.TotalWeekOrderQuantity == 0);


            OrderProductItems = new List<OrderProductItem>();

            foreach (Recipe recipe in Recipes)
            {
                // SingleOrDefault is used because some inactive recipes have been excluded!
                var choice = RecipeChoices.Choices.SingleOrDefault(r => r.RecipeId == recipe.Id);

                if (choice == null)
                    continue;

                // If the recipe does not have any quantities selected for any day continue...
                if (choice.TotalWeekOrderQuantity == 0)
                    continue;

                // For this recipe that we know that at least 1 quantity order for some day exists add its incredients * quantity 
                // to the total ingredient quantity list "OrderProductItems"
                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    OrderProductItem item = OrderProductItems.SingleOrDefault(o => o.Product.Id == ingredient.Product.Id);
                    if (item == null)
                    {
                        item = new OrderProductItem() { Product = ingredient.Product, Supplier = ingredient.Product.Supplier };
                        OrderProductItems.Add(item);
                    }
                    // At this point it does not matter which day the order for this recipe is place. We only care about the weekly total!
                    item.Quantity += choice.TotalWeekOrderQuantity * ingredient.Quantity;
                }
            }
            
            OrderProductItems = OrderProductItems.OrderBy(o => o.Product.ProductType).ThenBy(o => o.Product.Name).ToList();

            // *** At this point the first sub table for the total item order is completed *** OrderProductItems is not used anymore!

            // *** The more complex part of computing the order per day and supplier starts here ***
            
            // This is the end result: A map that for each day (key) has another map as value which has productId as key and quantity as value!
            Dictionary<OrderDay, Dictionary<int, int>> productQuantityOrderedPerDay = new Dictionary<OrderDay, Dictionary<int, int>>();

            foreach(OrderDay day in Enum.GetValues(typeof(OrderDay)))
            {
                productQuantityOrderedPerDay[day] = new Dictionary<int, int>();

                List<OrderRecipeItem> dayOrderRecipeItems = orderRecipes.Where(o => o.Day == day).ToList();

                foreach(var orderRecipeItem in dayOrderRecipeItems)
                {
                    foreach(var recipeIngredient in orderRecipeItem.Recipe.Ingredients)
                    {
                        productQuantityOrderedPerDay[day].TryGetValue(recipeIngredient.ProductId, out int currentIngredientQuantity);
                        productQuantityOrderedPerDay[day][recipeIngredient.ProductId] = currentIngredientQuantity + recipeIngredient.Quantity * orderRecipeItem.Quantity;
                    }
                }
            }

            //*** We now have an object that contains per day per product the quantities needed for this order ***
            
            // However we need a product delivery schedule per day depending on when the product is needed!

            ProductDeliveryPerDay = new Dictionary<ProductDeliveryDay, Dictionary<Product, int>>();

            var products = productData.GetByName(null).ToList();

            foreach (OrderDay day in Enum.GetValues(typeof(OrderDay)))
            {
                foreach(int productId in productQuantityOrderedPerDay[day].Keys)
                {
                    var product = products.Single(p => p.Id == productId);

                    //ProductDeliveryDay deliveryDay = ProductDeliveryDay.PreviousFriday;

                    // THIS IS NOW REMOVED!!! THIS IS REALLY CRUCIAL HERE!!! - MUST REVIEW THIS AGAIN!!!!
                    //if (product.OrderWindow > 0)
                    //    deliveryDay = (ProductDeliveryDay)Math.Max((int)day - product.OrderWindow, (int)ProductDeliveryDay.PreviousFriday);

                    // New logic is based on delivery day as define per product:
                    // We search for the previous available delivery day.
                    ProductDeliveryDay deliveryDay = product.GetProductDeliveryDay(day);

                    if (!ProductDeliveryPerDay.ContainsKey(deliveryDay))
                        ProductDeliveryPerDay[deliveryDay] = new Dictionary<Product, int>();

                    ProductDeliveryPerDay[deliveryDay].TryGetValue(product, out int currentProductOrder);
                    ProductDeliveryPerDay[deliveryDay][product] =
                        currentProductOrder + productQuantityOrderedPerDay[day][productId];


                }
            }

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
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Monday, OrderDay.Monday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Tuesday, OrderDay.Tuesday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Wednesday, OrderDay.Wednesday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Thursday, OrderDay.Thursday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Friday, OrderDay.Friday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Saturday, OrderDay.Saturday);
                    CreateOrderItemLines(item.RecipeId, item.OrderQuantity_Sunday, OrderDay.Sunday);
                }

                Order.UpdatedOn = DateTime.Now;
                Order.UpdatedBy = Helpers.User.GetUPN(HttpContext);
                orderData.Update(Order);
            }

            orderData.Commit();
            return RedirectToPage("./Edit", new { id = Order.Id });

        }

        private void CreateOrderItemLines(int recipeId, int quantity, OrderDay day)
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