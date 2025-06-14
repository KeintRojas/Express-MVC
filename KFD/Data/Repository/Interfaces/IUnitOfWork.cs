namespace KFD.Data.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IDishRepository Dish { get; }
        IUserRepository User { get; }
        void Save ();
    }
}
