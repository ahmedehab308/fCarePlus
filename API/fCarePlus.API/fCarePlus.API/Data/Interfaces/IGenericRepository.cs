using System.Linq.Expressions;

namespace fCarePlus.API.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<T?> GetByIdAsync(long id);

        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}