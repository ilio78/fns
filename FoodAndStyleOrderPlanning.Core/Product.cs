using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FoodAndStyleOrderPlanning.Core
{
    public class Product
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Quality { get; set; }

        [Required]
        public MeasuringUnit MeasuringUnit { get; set; }

         public ProductType ProductType { get; set; }

        [Required]
        public int SupplierId  { get; set; }

        [Required]
        public Supplier Supplier { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }


    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    //public class ProductType
    //{
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }

    //    [Required]
    //    public string Name { get; set; }
    //}

}
