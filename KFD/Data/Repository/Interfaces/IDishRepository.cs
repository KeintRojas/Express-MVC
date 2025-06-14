using KFD.Models;

namespace KFD.Data.Repository.Interfaces
{
    public interface IDishRepository : IRepository<Dish>
    {
        void Update(Dish dish);
    }
}
