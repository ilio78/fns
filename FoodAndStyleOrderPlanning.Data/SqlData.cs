using System;
using System.Collections.Generic;

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

        public abstract T Update(T item);
    }

}
