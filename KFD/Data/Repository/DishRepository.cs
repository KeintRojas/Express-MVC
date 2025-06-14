using KFD.Data.Repository.Interfaces;
using KFD.Models;

namespace KFD.Data.Repository
{
    public class DishRepository : Repository<Dish>, IDishRepository
    {
        private ApplicationDbContext _db;
        public DishRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Dish dish)
        {
           _db.dishes.Update(dish);
        }
    }
}
