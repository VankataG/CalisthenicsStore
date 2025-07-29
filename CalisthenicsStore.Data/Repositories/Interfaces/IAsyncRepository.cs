using System.Linq.Expressions;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IAsyncRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> AddAsync(TEntity item);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        Task<bool> DeleteAsync(TEntity item);

        Task<bool> HardDeleteAsync(TEntity item);

        Task<bool> UpdateAsync(TEntity item);

        Task<int> CountAsync();

        Task SaveChangesAsync();
    }
}
