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
            ProductTypeTranslations.Add(((int)ProductType.FruitAndVegetables_Cut).ToString(), "Φρούτα & Λαχανικά - Κομμένα");
            ProductTypeTranslations.Add(((int)ProductType.Groseries).ToString(), "Μαναβική");
            ProductTypeTranslations.Add(((int)ProductType.Meat).ToString(), "Κρέατα");
            ProductTypeTranslations.Add(((int)ProductType.Fish).ToString(), "Ψάρι/Θαλασσινά");
            ProductTypeTranslations.Add(((int)ProductType.Pastry).ToString(), "Ζαχαροπλαστική");
            ProductTypeTranslations.Add(((int)ProductType.Other).ToString(), "Διάφορα");
            ProductTypeTranslations.OrderBy(p => p.Value).ToList();

            MeasuringUnitTranslations = new Dictionary<string, string>();
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Grams).ToString(), "Γραμμάρια");
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Millilitres).ToString(), "Χιλιοστόλιτρα");
            MeasuringUnitTranslations.Add(((int)MeasuringUnit.Pieces).ToString(), "Τεμάχια");
            MeasuringUnitTranslations.OrderBy(p => p.Value).ToList();
        }
}
}
