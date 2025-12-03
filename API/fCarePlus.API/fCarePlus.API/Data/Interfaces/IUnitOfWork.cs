using fCarePlus.API.Data.Entities;

namespace fCarePlus.API.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

        IGenericRepository<AccountsChart> Accounts { get; }

        Task<int> CompleteAsync();
    }
}