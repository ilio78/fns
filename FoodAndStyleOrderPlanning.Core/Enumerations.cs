namespace FoodAndStyleOrderPlanning.Core
{

    public static class Helpers
    {
        public const string DateTimeFormater = "d MMM yyyy H:mm";
        public const string DateFormater = "ddd, M MMM yyyy";
    }


    public enum CuisineType
    {
        None,
        Mexican,
        Italian,
        Indian
    }

    public enum MeasuringUnit
    {
        Grams,
        Millilitres,
        Pieces
    }

    public enum ProductFreshness
    {
        OneDayBeforeOrder = 1,
        TwoDaysBeforeOrder = 2,
        ThreeDaysBeforeOrder = 3,
        FourDaysBeforeOrder = 4,
        FiveDaysBeforeOrder = 5,
        Indifferent = 50
    }

    public enum RecipeType
    {
        Unknown = 0,
        Other = 1,
        OliveOil,
        Ospria,
        Soup,
        Pasta,
        Pies,
        GroundMeat,
        Beef,
        Pork,
        Sausage,
        Tartes,
        StreetFood,
        Salad,
        Sandwich
    }

    public enum ProductType
    {
        FruitAndVegetables_Fresh,
        Vegetables_Cut,
        Bakaliki,
        Meat,
        Fish,
        Pastry,
        Other,
        SpecialGroup1
    }


    public enum ProductDeliveryDay
    {
        PreviousFriday = -3,
        PreviousSaturday = -2,
        PreviousSunday = -1,
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6
    }

    public enum OrderDay
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6
    }

}
