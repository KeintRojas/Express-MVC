using KFD.Data.Repository.Interfaces;
using KFD.Models;

namespace KFD.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User user)
        {
            _db.users.Add(user);
        }
    }
}
