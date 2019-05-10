using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FoodAndStyleOrderPlanning.Core
{
    public class Restaurant
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Location { get; set; }
        public int Id { get; set; }
        public CuisineType Cuisine { get; set; }
    }
}
