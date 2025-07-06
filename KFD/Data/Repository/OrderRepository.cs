using KFD.Data.Repository.Interfaces;
using KFD.Models;

namespace KFD.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order order)
        {
            _db.orders.Update(order);
        }
    }
}
