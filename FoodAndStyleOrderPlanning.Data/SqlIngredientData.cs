using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FoodAndStyleOrderPlanning.Data
{
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
            return db.Ingredients.ToListAsync().Result;
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

}
