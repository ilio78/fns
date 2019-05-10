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
    public class ListModel : PageModel
    {
        [BindProperty]
        public IEnumerable<Order> Orders { get; set; }

        private readonly IData<Order> orderData;
        
        public ListModel(IData<Order> orderData)
        {
            this.orderData = orderData;
        }

        public void OnGet()
        {
            Orders = orderData.GetByName(null).OrderByDescending(o => o.CreatedOn);
        }
    }
}