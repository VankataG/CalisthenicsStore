using System.Linq.Expressions;
using CalisthenicsStore.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CalisthenicsStore.Data.Repositories
{
    public class BaseRepository<TEntity, TKey> 
        : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey> 
        where TEntity : class
    {

        private readonly CalisthenicsStoreDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public BaseRepository(CalisthenicsStoreDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
        }


        public TEntity? GetById(TKey id)
        {
            return dbSet.Find(id);
        }
        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await dbSet
                .FindAsync(id);
        }

        public TEntity? SingleOrDefault(Func<TEntity, bool> predicate)
        {
            return dbSet
                .SingleOrDefault(predicate);
        }
        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet
                .SingleOrDefaultAsync(predicate);
        }

        public TEntity? FirstOrDefault(Func<TEntity, bool> predicate)
        {
            return dbSet
                .FirstOrDefault(predicate);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return dbSet
                .FirstOrDefaultAsync(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet
                .ToList();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet
                .ToListAsync();
        }

        public IQueryable<TEntity> GetAllAttacked()
        {
            return dbSet
                .AsQueryable();
        }

        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }
        public Task AddAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }
        public Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TEntity item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public bool Update(TEntity item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public int Count(TEntity item)
        {
            throw new NotImplementedException();
        }
        public Task<int> CountAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
        
    }
}
