using System.Linq.Expressions;
using CalisthenicsStore.Data.Repositories.Interfaces;


namespace CalisthenicsStore.Data.Repositories
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey>
    {
        public TEntity GetById(TKey id)
        {
            throw new NotImplementedException();
        }

        public TEntity SingleOrDefault(Func<TKey, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity FirstOrDefault(Func<TKey, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAllAttacked()
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TEntity item)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity item)
        {
            throw new NotImplementedException();
        }

        public int Count(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TKey, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefault(Expression<Func<TKey, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
