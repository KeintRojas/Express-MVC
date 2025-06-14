using KFD.Data.Repository.Interfaces;

namespace KFD.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public IDishRepository Dish { get; private set; }
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Dish = new DishRepository(db);
            User = new UserRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
