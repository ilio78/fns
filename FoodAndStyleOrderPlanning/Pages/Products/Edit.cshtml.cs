using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAndStyleOrderPlanning.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IData<Product> productData;
        private readonly IData<Supplier> supplierData;

        private IHtmlHelper htmlHelper;

        public IEnumerable<SelectListItem> MeasuringUnit { get; set; }
        public IEnumerable<SelectListItem> ProductType { get; set; }
        public IList<SelectListItem> SupplierOptions { get; set; }

        [BindProperty]
        public Product Product { get; set; }

        public EditModel(IData<Product> productData, IData<Supplier> supplierData, IHtmlHelper htmlHelper)
        {
            this.productData = productData;
            this.htmlHelper = htmlHelper;
            this.supplierData = supplierData;
        }

        private void LoadData()
        {
            MeasuringUnit = htmlHelper.GetEnumSelectList<MeasuringUnit>();
            ProductType = htmlHelper.GetEnumSelectList<ProductType>();
            var suppliers = supplierData.GetByName(null).OrderBy(s => s.Name).ToList();
            SupplierOptions = new List<SelectListItem>();
            foreach (Supplier s in suppliers)
            {
                SupplierOptions.Add(new SelectListItem(s.Name, s.Id.ToString()));
            }
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
                Product = new Product();
            else
                Product = productData.GetById(id.Value);

            if (Product == null)
                return RedirectToPage("./NotFound");

            LoadData();

            return Page();

        }

        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //    return Page();
            Product.Supplier = supplierData.GetById(Product.SupplierId);

            if (Product.Id > 0)
                productData.Update(Product);
            else
                productData.Add(Product);


            productData.Commit();
            return RedirectToPage("./List");
        }
    }
}