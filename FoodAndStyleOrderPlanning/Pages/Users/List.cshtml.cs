using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAndStyleOrderPlanning.Core;
using FoodAndStyleOrderPlanning.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace FoodAndStyleOrderPlanning.Pages.Users
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private IData<User> data;

        public IEnumerable<User> Users { get; set; }

        public ListModel(IConfiguration config, IData<User> data)
        {
            this.config = config;
            this.data = data;
        }

        public void OnGet()
        {
            Users = data.GetByName(null).OrderBy(u=>u.Email);
        }
    }
}