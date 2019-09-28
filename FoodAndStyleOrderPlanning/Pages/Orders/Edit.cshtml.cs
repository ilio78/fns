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
       
            int i = 1;
            foreach (Recipe r in Recipes)
            {
                RecipeChoices.Choices.Add(new ChoiceViewModel()
                {
                    Id = i++, Name = r.Name, RecipeResultingQuantity = String.Format("{0:n0}", r.ResultingQuantityInGrams),
                    RecipeId = r.Id, IsActive = r.IsActive
                });
            }

            var alreadySelected = orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id).ToList();

            foreach (var c in RecipeChoices.Choices)
            {
                foreach(var orderRecipeItem in alreadySelected.Where(o => o.RecipeId == c.RecipeId))
                {
                    switch (orderRecipeItem.Day)
                    {
                        case OrderDay.Monday:
                            c.OrderQuantity_Monday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Tuesday:
                            c.OrderQuantity_Tuesday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Wednesday:
                            c.OrderQuantity_Wednesday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Thursday:
                            c.OrderQuantity_Thursday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Friday:
                            c.OrderQuantity_Friday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Saturday:
                            c.OrderQuantity_Saturday = orderRecipeItem.Quantity;
                            break;
                        case OrderDay.Sunday:
                            c.OrderQuantity_Sunday = orderRecipeItem.Quantity;
                            break;
                    }
                }
            }

            RecipeChoices.Choices.RemoveAll(c => !c.IsActive && c.TotalWeekOrderQuantity == 0);

            OrderProductItems = new List<OrderProductItem>();

            foreach (Recipe recipe in Recipes)
            {
                var choice = RecipeChoices.Choices.SingleOrDefault(r => r.RecipeId == recipe.Id);

                if (choice == null)
                    continue;

                if (choice.OrderQuantity_Monday + choice.OrderQuantity_Tuesday + choice.OrderQuantity_Wednesday +
                    choice.OrderQuantity_Thursday + choice.OrderQuantity_Friday + choice.OrderQuantity_Saturday + choice.OrderQuantity_Sunday == 0)
                    continue;

                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    OrderProductItem item = OrderProductItems.SingleOrDefault(o => o.Product.Id == ingredient.Product.Id);
                    if (item == null)
                    {
                        item = new OrderProductItem() { Product = ingredient.Product, Supplier = ingredient.Product.Supplier };
                        OrderProductItems.Add(item);
                    }

                    item.Quantity += choice.OrderQuantity_Monday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Tuesday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Wednesday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Thursday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Friday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Saturday * ingredient.Quantity;
                    item.Quantity += choice.OrderQuantity_Sunday * ingredient.Quantity;
                }
            }
            
            OrderProductItems = OrderProductItems.OrderBy(o => o.Product.ProductType).ThenBy(o => o.Product.Name).ToList();

            Dictionary<OrderDay, Dictionary<int, int>> productQuantityOrderedPerDay = new Dictionary<OrderDay, Dictionary<int, int>>();

            foreach(OrderDay day in (OrderDay[])Enum.GetValues(typeof(OrderDay)))
            {
                productQuantityOrderedPerDay[day] = new Dictionary<int, int>();

                List<OrderRecipeItem> items = orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id && o.Day == day).ToList();
                foreach(var dayOrder in items)
                {
                    foreach(var recipeIngredient in dayOrder.Recipe.Ingredients)
                    {
                        int currentIngredientQuantity = 0;
                        productQuantityOrderedPerDay[day].TryGetValue(recipeIngredient.Id, out currentIngredientQuantity);
                        productQuantityOrderedPerDay[day][recipeIngredient.ProductId] = currentIngredientQuantity + recipeIngredient.Quantity * dayOrder.Quantity;
                    }
                }
            }

            ProductDeliveryPerDay = new Dictionary<ProductDeliveryDay, Dictionary<Product, int>>();

            var products = productData.GetByName(null).ToList();

            foreach (OrderDay day in (OrderDay[])Enum.GetValues(typeof(OrderDay)))
            {
                foreach(int productId in productQuantityOrderedPerDay[day].Keys)
                {
                    var product = products.Single(p => p.Id == productId);

                    ProductDeliveryDay deliveryDay = ProductDeliveryDay.PreviousFriday;

                    if (product.OrderWindow > 0)
                        deliveryDay = (ProductDeliveryDay)Math.Max((int)day - product.OrderWindow, (int)ProductDeliveryDay.PreviousFriday);
                   
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