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

    //public enum ServingType
    //{
    //    Kilograms,
    //    Servings
    //}
}
