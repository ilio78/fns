using FoodAndStyleOrderPlanning.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FoodAndStyleOrderPlanning.Data
{
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

        public override User Update(User user)
        {
            var entity = db.Users.Attach(user);
            entity.State = EntityState.Modified;
            return user;
        }

    }

}
