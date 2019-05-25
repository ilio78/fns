using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Suppliers
{
    public class DeleteModel : PageModel
    {
        private readonly IData<Supplier> data;

        public Supplier Entity { get; set; }

        public DeleteModel(IData<Supplier> data)
        {
            this.data = data;
        }

        private bool CanDelete()
        {
            return Entity != null && (Entity.Products == null || Entity.Products.Count == 0);
        }

        public IActionResult OnGet(int id)
        {
            Entity = data.GetById(id);
            if (!CanDelete())
                return RedirectToPage("./List");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            Entity = data.Delete(id);
            data.Commit();
            if (!CanDelete())
                return RedirectToPage("./List");

            TempData["Message"] = "Deleted";
            return RedirectToPage("./List");

        }
    }
}