using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FoodAndStyleOrderPlanning.Data
{
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

}
