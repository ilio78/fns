using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
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

}
