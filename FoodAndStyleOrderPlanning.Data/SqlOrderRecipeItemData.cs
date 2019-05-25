using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FoodAndStyleOrderPlanning.Data
{
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

}
