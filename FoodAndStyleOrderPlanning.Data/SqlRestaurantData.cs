using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{


    public class SqlRestaurantData : IData<Restaurant>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlRestaurantData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Restaurant Add(Restaurant newRestaurant)
        {
            db.Add(newRestaurant);
            return newRestaurant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Restaurant Delete(int id)
        {
            var restaurant = GetById(id);
            if (restaurant != null)
                db.Restaurants.Remove(restaurant);
            return restaurant;
        }

        public Restaurant GetById(int id)
        {
            return db.Restaurants.Find(id);
        }

        public int GetCount()
        {
            return db.Restaurants.Count();
        }

        public IEnumerable<Restaurant> GetByName(string name)
        {
            var query = from r in db.Restaurants
                        where string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }

}
