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
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public int OrderWindow { get; set; }

        public int? QuantityToPiece { get; set; }

        [Required]
        public bool IsActive { get; set; }
        // ALTER TABLE [Products] ADD [IsActive] bit NOT NULL DEFAULT 0;

        [Required]
        public bool DeliveryOnMonday { get; set; }
        [Required]
        public bool DeliveryOnTuesday { get; set; }
        [Required]
        public bool DeliveryOnWednesday { get; set; }
        [Required]
        public bool DeliveryOnThursday { get; set; }
        [Required]
        public bool DeliveryOnFriday { get; set; }
        [Required]
        public bool DeliveryOnSaturday { get; set; }
        [Required]
        public bool DeliveryOnSunday { get; set; }

        public int PieceMassOrVolume { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        
        public void SetFromViewModel(ProductViewModel productViewModel)
        {
            Name = productViewModel.Name;
            Quality = productViewModel.Quality;
            MeasuringUnit = productViewModel.MeasuringUnit;
            ProductType = productViewModel.ProductType;
            SupplierId = productViewModel.SupplierId;
            IsActive = productViewModel.IsActive;
            QuantityToPiece = productViewModel.QuantityToPiece;

            if (MeasuringUnit == MeasuringUnit.Pieces)
                Price = productViewModel.PriceEuroPart * 100 + productViewModel.PriceCentsPart;
            else
                Price = (float)(productViewModel.PriceEuroPart * 100 + productViewModel.PriceCentsPart) / 1000;

            OrderWindow = (int)productViewModel.ProductFreshness;

            DeliveryOnMonday = productViewModel.DeliveryOnMonday;
            DeliveryOnTuesday = productViewModel.DeliveryOnTuesday;
            DeliveryOnWednesday = productViewModel.DeliveryOnWednesday;
            DeliveryOnThursday = productViewModel.DeliveryOnThursday;
            DeliveryOnFriday = productViewModel.DeliveryOnFriday;
            DeliveryOnSaturday = productViewModel.DeliveryOnSaturday;
            DeliveryOnSunday = productViewModel.DeliveryOnSunday;
        }

        public ProductFreshness GetProductFreshness()
        {
            if (OrderWindow < 1)
                return ProductFreshness.OneDayBeforeOrder;
            if (OrderWindow > 5)
                return ProductFreshness.Indifferent;
            return (ProductFreshness)OrderWindow;
        }

        public ProductDeliveryDay GetProductDeliveryDay(OrderDay day)
        {

            if (day == OrderDay.Sunday)
            {
                if (DeliveryOnSaturday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }
            if (day == OrderDay.Saturday)
            {
                if (DeliveryOnFriday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }
            if (day == OrderDay.Friday)
            {
                if (DeliveryOnThursday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }
            if (day == OrderDay.Thursday)
            {
                if (DeliveryOnWednesday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }
            if (day == OrderDay.Wednesday)
            {
                if (DeliveryOnTuesday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }
            if (day == OrderDay.Tuesday)
            {
                if (DeliveryOnMonday)
                    return (ProductDeliveryDay)(day - 1);
                else
                    return GetProductDeliveryDay(day - 1);
            }

            if (DeliveryOnSunday)
                return ProductDeliveryDay.PreviousSunday;
            else if (DeliveryOnSaturday)
                return ProductDeliveryDay.PreviousSaturday;
            else
                return ProductDeliveryDay.PreviousFriday;
        }

    }

    public class ProductViewModel
    {
        public ProductViewModel()
        {
            IsActive = true;
            DeliveryOnMonday = false;
            DeliveryOnTuesday = false;
            DeliveryOnWednesday = false;
            DeliveryOnThursday = false;
            DeliveryOnFriday = false;
            DeliveryOnSaturday = false;
            DeliveryOnSunday = false;
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
            QuantityToPiece = product.QuantityToPiece ?? 0;

            var price = product.Price;

            if (MeasuringUnit != MeasuringUnit.Pieces)
                price = 1000 * price;

            PriceEuroPart = (int)price / 100;
            PriceCentsPart = (int)price % 100;

            ProductFreshness = product.GetProductFreshness();

            DeliveryOnMonday = product.DeliveryOnMonday;
            DeliveryOnTuesday = product.DeliveryOnTuesday;
            DeliveryOnWednesday = product.DeliveryOnWednesday;
            DeliveryOnThursday = product.DeliveryOnThursday;
            DeliveryOnFriday = product.DeliveryOnFriday;
            DeliveryOnSaturday = product.DeliveryOnSaturday;
            DeliveryOnSunday = product.DeliveryOnSunday;
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

        [Required]
        [Range(0, 999)]
        public int PriceEuroPart { get; set; }

        [Required]
        [Range(0, 99)]
        public int PriceCentsPart { get; set; }

        [Required]
        public ProductFreshness ProductFreshness { get; set; }

        public bool DeliveryOnMonday { get; set; }
        public bool DeliveryOnTuesday { get; set; }
        public bool DeliveryOnWednesday { get; set; }
        public bool DeliveryOnThursday { get; set; }
        public bool DeliveryOnFriday { get; set; }
        public bool DeliveryOnSaturday { get; set; }
        public bool DeliveryOnSunday { get; set; }

        [Range(0, 500)]
        public int? QuantityToPiece { get; set; }
    }
}
