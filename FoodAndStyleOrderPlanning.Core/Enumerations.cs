namespace FoodAndStyleOrderPlanning.Core
{

    public static class Helpers
    {
        public const string DateTimeFormater = "d MMM yyyy H:mm";
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

    public enum ProductType
    {
        FruitAndVegetables_Fresh,
        Vegetables_Cut,
        Bakaliki,
        Meat,
        Fish,
        Pastry,
        Other
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
