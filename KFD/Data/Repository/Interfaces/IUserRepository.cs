using KFD.Models;

namespace KFD.Data.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
    }
}
