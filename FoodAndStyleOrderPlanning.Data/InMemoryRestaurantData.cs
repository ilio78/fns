using FoodAndStyleOrderPlanning.Core;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
    public class InMemoryRestaurantData : IData<Restaurant>
    {
        List<Restaurant> restaurants;

        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant { Id = 1, Name = "Scott's Pizza", Location = "Maryland", Cuisine = CuisineType.Italian},
                new Restaurant { Id = 2, Name = "Cinnamon Club", Location = "London", Cuisine = CuisineType.None},
                new Restaurant { Id = 3, Name = "La Costa", Location = "California", Cuisine = CuisineType.Italian}
            };
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            restaurants.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            return 0;
        }

        public Restaurant Delete(int id)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == id);
            if (restaurant != null)
                restaurants.Remove(restaurant);

            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            return restaurants.SingleOrDefault(r => r.Id == id);
        }

        public int GetCount()
        {
            return restaurants.Count();
        }

        public IEnumerable<Restaurant> GetByName(string name)
        {
            return from r in restaurants
                   where string.IsNullOrWhiteSpace(name) || r.Name.ToLower().Contains(name.ToLower())
                   orderby r.Name
                   select r;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant == null)
                return null;

            restaurant.Name = updatedRestaurant.Name;
            restaurant.Location = updatedRestaurant.Location;
            restaurant.Cuisine = updatedRestaurant.Cuisine;
            return restaurant;
        }
    }
}
