using System;
using System.Collections;
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

        public SelectList MeasuringUnit { get; set; }        
        public SelectList ProductType { get; set; }
        public IList<SelectListItem> Suppliers { get; set; }

        [BindProperty]
        public ProductViewModel ProductViewModel { get; set; }

        public string SupplierNotActiveMessage { get; set; }

        public EditModel(IData<Product> productData, IData<Supplier> supplierData, IHtmlHelper htmlHelper)
        {
            this.productData = productData;
            this.htmlHelper = htmlHelper;
            this.supplierData = supplierData;
        }

        private void LoadData()
        {
            MeasuringUnit = new SelectList((IEnumerable)LanguageResources.MeasuringUnitTranslations, "Key", "Value");
            ProductType = new SelectList((IEnumerable)LanguageResources.ProductTypeTranslations, "Key", "Value");
            var allSuppliers = supplierData.GetByName(null).Where(sup => sup.IsActive).OrderBy(s => s.Name).ToList();

            Suppliers = new List<SelectListItem>();
            foreach (Supplier s in allSuppliers)
            {
                Suppliers.Add(new SelectListItem(s.Name, s.Id.ToString()));
            }
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                var product = productData.GetById(id.Value);
                ProductViewModel = new ProductViewModel(product);
                if (product == null)
                    return RedirectToPage("./List");
            }
            else
            {
                ProductViewModel = new ProductViewModel();
            }

            LoadData();

            if (ProductViewModel.SupplierId > 0 &&
                    !Suppliers.Select(s => s.Value).Contains(ProductViewModel.SupplierId.ToString()))
                SupplierNotActiveMessage = " δεν ειναι πλέον διαθέσιμος. Επιλογή νέου:";

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Product product;

            if (ProductViewModel.Id > 0)
            {
                product = productData.GetById(ProductViewModel.Id);
                if (product == null)
                    return RedirectToPage("./List");

                product.SetFromViewModel(ProductViewModel);

                productData.Update(product);
            }
            else
            {
                product = new Product();
                product.SetFromViewModel(ProductViewModel);
                productData.Add(product);
            }
                
            productData.Commit();
            return RedirectToPage("./List");
        }
    }
}

