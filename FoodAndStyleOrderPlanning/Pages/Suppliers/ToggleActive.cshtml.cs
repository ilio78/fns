using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Suppliers
{
    public class ToggleActiveModel : PageModel
    {
        private IData<Supplier> data;

        public ToggleActiveModel(IData<Supplier> data)
        {
            this.data = data;
        }

        public IActionResult OnGet(int id)
        {
            Supplier supplier = data.GetById(id);
            if (supplier == null)
                return RedirectToPage("./List");

            supplier.IsActive = !supplier.IsActive;

            data.Update(supplier);
            data.Commit();

            return RedirectToPage("./List");
        }
    }
}