using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodAndStyleOrderPlanning.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IData<User> data;

        public new User User { get; set; }

        public DeleteModel(IData<User> data)
        {
            this.data = data;
        }

        public IActionResult OnGet(int id)
        {
            User = data.GetById(id);
            if (User == null)
                return RedirectToPage("./List");
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var entity = data.Delete(id);
            data.Commit();
            if (entity == null)
            {
                return RedirectToPage("./List");
            }
            //TempData["Message"] = "Restaurant deleted";
            return RedirectToPage("./List");
        }
    }
}