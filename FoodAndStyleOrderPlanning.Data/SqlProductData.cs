using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
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

}
