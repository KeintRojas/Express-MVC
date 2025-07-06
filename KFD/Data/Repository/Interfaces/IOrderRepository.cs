using KFD.Models;

namespace KFD.Data.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
    }
}
