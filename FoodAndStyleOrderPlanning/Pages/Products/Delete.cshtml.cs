using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IData<Product> productData;
        private readonly IData<Ingredient> ingredientData;
     
        public Product Product { get; set; }

        public DeleteModel(IData<Product> productData, IData<Ingredient> ingredientData)
        {
            this.productData = productData;
            this.ingredientData = ingredientData;
        }

        private bool CanDeleteProduct()
        {
            if (Product == null)
                return false;

            return !ingredientData.GetByName("").Select(i => i.ProductId).Contains(Product.Id);
        }

        public IActionResult OnGet(int id)
        {
            Product = productData.GetById(id);
            
            if (CanDeleteProduct())
                return Page();
            
            return RedirectToPage("./List");            
        }

        public IActionResult OnPost(int id)
        {
            Product = productData.Delete(id);

            if (CanDeleteProduct())
                productData.Commit();

            return RedirectToPage("./List");
        }
    }
}