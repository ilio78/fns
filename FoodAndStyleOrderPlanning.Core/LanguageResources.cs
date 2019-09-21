using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoodAndStyleOrderPlanning.Core
{
    public static class LanguageResources
    {
        public static Dictionary<string, string> ProductTypeTranslations;
        public static Dictionary<string, string> MeasuringUnitTranslations;

        //<td>@LanguageResources.MeasuringUnitTranslations.First(p => p.Key == ((int) r.MeasuringUnit).ToString()).Value</td>
        //<td>@LanguageResources.ProductTypeTranslations.First(p => p.Key == ((int) r.ProductType).ToString()).Value</td>

        public static string GetMeasuringUnitTranslation(MeasuringUnit measuringUnit)
        {
            return LanguageResources.MeasuringUnitTranslations.First(p => p.Key == ((int)measuringUnit).ToString()).Value;
        }

        public static string GetProductTypeTranslation(ProductType productType)
        {
            return LanguageResources.ProductTypeTranslations.First(p => p.Key == ((int)productType).ToString()).Value;
        }


        static LanguageResources()
        {
            ProductTypeTranslations = new Dictionary<string, string>();
            ProductTypeTranslations.Add(((int)ProductType.FruitAndVegetables_Fresh).ToString(), "Φρούτα & Λαχανικά - Φρέσκα");
            ProductTypeTranslations.Add(((int)ProductType.Vegetables_Cut).ToString(), "Λαχανικά - Κομμένα");
            ProductTypeTranslations.Add(((int)ProductType.Bakaliki).ToString(), "Μπακαλική");
            ProductTypeTranslations.Add(((int)ProductType.Meat).ToString(), "Κρέατα");
            ProductTypeTranslations.Add(((int)ProductType.Fish).ToString(), "Ψάρι/Θαλασσινά");
            ProductTypeTranslations.Add(((int)ProductType.Pastry).ToString(), "Ζαχαροπλαστική");
            ProductTypeTranslations.Add(((int)ProductType.Other).ToString(), "Διάφορα");
            
            MeasuringUnitTranslations = new Dictionary<string, string>();
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Grams).ToString(), "Γραμμάρια");
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Millilitres).ToString(), "Χιλιοστόλιτρα");
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Pieces).ToString(), "Τεμάχια");
        }
}

    public static class OrderingConfiguration
    {
        public static Dictionary<ProductType, List<OrderDay>> ProductTypeNonDeliveryDays;

        static OrderingConfiguration()
        {
            ProductTypeNonDeliveryDays[ProductType.FruitAndVegetables_Fresh] = new List<OrderDay>();
            ProductTypeNonDeliveryDays[ProductType.Vegetables_Cut] = new List<OrderDay>() { OrderDay.Sunday };
            ProductTypeNonDeliveryDays[ProductType.Bakaliki] = new List<OrderDay>() { OrderDay.Sunday };
            ProductTypeNonDeliveryDays[ProductType.Meat] = new List<OrderDay>() { OrderDay.Sunday };
            ProductTypeNonDeliveryDays[ProductType.Fish] = new List<OrderDay>() { OrderDay.Sunday };
            ProductTypeNonDeliveryDays[ProductType.Pastry] = new List<OrderDay>() { OrderDay.Sunday };
            ProductTypeNonDeliveryDays[ProductType.Other] = new List<OrderDay>() { OrderDay.Sunday };
        }
    }
}
