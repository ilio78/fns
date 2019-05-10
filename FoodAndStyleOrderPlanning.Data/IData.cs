using FoodAndStyleOrderPlanning.Core;
using System.Collections.Generic;
using System.Text;

namespace FoodAndStyleOrderPlanning.Data
{
  
    public interface IData<T> where T : class
    {
        IEnumerable<T> GetByName(string name);
        T GetById(int id);
        T Update(T item);
        T Add(T item);

        T Delete(int id);

        int GetCount();

        int Commit();
    }
}
