using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
    public class SqlSupplierData : SqlData<Supplier>
    {
        public SqlSupplierData(FoodAndStyleOrderPlanningDbContext db) : base(db) { }

        public override IEnumerable<Supplier> GetByName(string name)
        {
            var query = from r in db.Suppliers.Include(p => p.Products)
                        where string.IsNullOrEmpty(name) || r.Name.ToLower().Contains(name)
                        orderby r.Name
                        select r;
            return query;
        }

        public override Supplier Update(Supplier item)
        {
            var entity = db.Suppliers.Attach(item);
            entity.State = EntityState.Modified;
            return item;
        }
    }
}
