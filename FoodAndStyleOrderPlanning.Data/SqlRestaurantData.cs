using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
    
    public abstract class SqlData<T> : IData<T> where T : class
    {
        protected readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public T Add(T item)
        {
            db.Add(item);
            return item;   
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public T Delete(int id)
        {
            T t = GetById(id);
            if (t!=null)
                db.Remove(t);
            return t;
        }

        public T GetById(int id)
        {
            return db.Find<T>(id);
        }

        public abstract IEnumerable<T> GetByName(string name);
        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public T Update(T item)
        {
            throw new NotImplementedException();
        }
    }

    public class SqlUserData : SqlData<User>
    {
        public SqlUserData(FoodAndStyleOrderPlanningDbContext db) : base(db) { }

        public override IEnumerable<User> GetByName(string name)
        {
            var query = from r in db.Users
                        where string.IsNullOrEmpty(name) || r.Email.ToLower().Contains(name.ToLower())
                        orderby r.Email
                        select r;
            return query;
        }
    }


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

    public class SqlOrderData : IData<Order>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlOrderData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Order Add(Order item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Order Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Order GetById(int id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<Order> GetByName(string name)
        {
            return db.Orders.Include(o => o.OrderRecipeItems);
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public Order Update(Order item)
        {
            var entity = db.Orders.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }


    public class SqlOrderRecipeItemData : IData<OrderRecipeItem>
    {

        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlOrderRecipeItemData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public OrderRecipeItem Add(OrderRecipeItem item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public OrderRecipeItem Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                db.OrderRecipeItems.Remove(item);
            return item;
        }

        public OrderRecipeItem GetById(int id)
        {
            return db.OrderRecipeItems.Find(id);
        }

        public IEnumerable<OrderRecipeItem> GetByName(string name)
        {
            return db.OrderRecipeItems;
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public OrderRecipeItem Update(OrderRecipeItem item)
        {
            var entity = db.OrderRecipeItems.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }



    public class SqlIngredientData : IData<Ingredient>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlIngredientData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Ingredient Add(Ingredient item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Ingredient Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                db.Ingredients.Remove(item);
            return item;
        }

        public Ingredient GetById(int id)
        {
            return db.Ingredients.Find(id);
        }

        public IEnumerable<Ingredient> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public Ingredient Update(Ingredient item)
        {
            var entity = db.Ingredients.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }

    public class SqlProductData : IData<Product>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlProductData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Product Add(Product item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Product Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                db.Products.Remove(item);
            return item;
        }

        public Product GetById(int id)
        {
            return db.Products.Find(id);
        }

        public int GetCount()
        {
            return db.Products.Count();
        }

        public IEnumerable<Product> GetByName(string name)
        {
            var query = from r in db.Products.Include(p=>p.Supplier)
                        where string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Product Update(Product item)
        {
            var entity = db.Products.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }


    public class SqlRecipeData : IData<Recipe>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlRecipeData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Recipe Add(Recipe item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Recipe Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
                db.Recipes.Remove(item);
            return item;
        }

        public Recipe GetById(int id)
        {
            return db.Recipes.Include(r => r.Ingredients).ThenInclude(p => p.Product).FirstOrDefault(i => i.Id == id);
        }

        public int GetCount()
        {
            return db.Recipes.Count();
        }

        public IEnumerable<Recipe> GetByName(string name)
        {
            var query = from r in db.Recipes.Include(r => r.Ingredients).ThenInclude(p => p.Product)
                        where string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public Recipe Update(Recipe item)
        {
            var entity = db.Recipes.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }

    public class SqlSupplierData : IData<Supplier>
    {
        private readonly FoodAndStyleOrderPlanningDbContext db;

        public SqlSupplierData(FoodAndStyleOrderPlanningDbContext db)
        {
            this.db = db;
        }

        public Supplier Add(Supplier item)
        {
            db.Add(item);
            return item;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Supplier Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Supplier GetById(int id)
        {
            return db.Suppliers.FirstOrDefault(i => i.Id == id);
        }

        public IEnumerable<Supplier> GetByName(string name)
        {
            var query = from r in db.Suppliers
                        where string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }


        public Supplier Update(Supplier item)
        {
            var entity = db.Suppliers.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }

}
