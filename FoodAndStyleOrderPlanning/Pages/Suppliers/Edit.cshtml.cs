using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodAndStyleOrderPlanning.Pages.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly IData<Supplier> data;
        
        [BindProperty]
        public Supplier Supplier { get; set; }

        public EditModel(IData<Supplier> data, IHtmlHelper htmlHelper)
        {
            this.data = data;
        }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
                Supplier = new Supplier();
            else
                Supplier = data.GetById(id.Value);

            if (Supplier == null)
                return RedirectToPage("./NotFound");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Supplier.Id > 0)
                data.Update(Supplier);
            else
                data.Add(Supplier);

            data.Commit();
            return RedirectToPage("./List");
        }
    }
}