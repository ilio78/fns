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

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        public int SupplierId  { get; set; }

        public Supplier Supplier { get; set; }

        [Required]
        public bool IsActive { get; set; }
        // ALTER TABLE [Products] ADD [IsActive] bit NOT NULL DEFAULT 0;

        public ICollection<Ingredient> Ingredients { get; set; }

        public void SetFromViewModel(ProductViewModel productViewModel)
        {
            Name = productViewModel.Name;
            Quality = productViewModel.Quality;
            MeasuringUnit = productViewModel.MeasuringUnit;
            ProductType = productViewModel.ProductType;
            SupplierId = productViewModel.SupplierId;
            IsActive = productViewModel.IsActive;
        }
    }

    public class ProductViewModel
    {
        public ProductViewModel()
        {
            IsActive = true;
        }

        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Quality = product.Quality;
            MeasuringUnit = product.MeasuringUnit;
            ProductType = product.ProductType;
            SupplierId = product.SupplierId;
            IsActive = product.IsActive;
        }

        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Quality { get; set; }

        [Required]
        public MeasuringUnit MeasuringUnit { get; set; }

        [Required]
        public ProductType ProductType { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

}
