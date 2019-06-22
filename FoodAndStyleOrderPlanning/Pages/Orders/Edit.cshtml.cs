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
                RecipeChoices = new RecipeChoicesViewModel();
                OrderProductItems = new List<OrderProductItem>();
                return Page();
            }

            Order = orderData.GetById(id.Value);
            if (Order == null)
            {
                return RedirectToPage("./List");
            }

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
                var orderRecipeItem = alreadySelected.FirstOrDefault(o => o.RecipeId == c.RecipeId);
                if (orderRecipeItem == null)
                    continue;

                c.Quantity = orderRecipeItem.Quantity;

            }

            List<int> recipeIdsWithQuantityOverZero = RecipeChoices.Choices.Where(r => r.Quantity > 0).Select(r => r.RecipeId).ToList();

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

                    item.Quantity += RecipeChoices.Choices.Single(r=>r.RecipeId==recipe.Id).Quantity * ingredient.Quantity;                    
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
                Order.CreatedOn = DateTime.Now;
                Order.UpdatedOn = Order.CreatedOn;
                Order.CreatedBy = HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                Order.UpdatedBy = HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];
                orderData.Add(Order);
            }
            else
            {
                var alreadySelected = orderRecipeItemData.GetByName(null).Where(o => o.OrderId == Order.Id).ToList();

                foreach (var item in alreadySelected.ToList())
                    orderRecipeItemData.Delete(item.Id);

                orderRecipeItemData.Commit();

                foreach (ChoiceViewModel item in RecipeChoices.Choices)
                {
                    if (item.Quantity == 0)
                        continue;

                    OrderRecipeItem orderRecipeItem = new OrderRecipeItem();
                    orderRecipeItem.OrderId = Order.Id;
                    orderRecipeItem.Quantity = item.Quantity;
                    orderRecipeItem.RecipeId = item.RecipeId;

                    orderRecipeItemData.Add(orderRecipeItem);
                    orderRecipeItemData.Commit();
                }

                Order.UpdatedOn = DateTime.Now;

                if (string.IsNullOrWhiteSpace(HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"]))
                    Order.UpdatedBy = "unknown";
                else
                    Order.UpdatedBy = HttpContext.Request.Headers["X-MS-CLIENT-PRINCIPAL-NAME"];

                orderData.Update(Order);
            }

            orderData.Commit();
            return RedirectToPage("./Edit", new { id = Order.Id });

        }

    }
}