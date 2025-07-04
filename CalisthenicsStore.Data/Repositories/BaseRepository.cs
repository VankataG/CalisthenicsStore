using System.Linq.Expressions;
using CalisthenicsStore.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


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
            dbSet.Add(item);
        }
        public async Task AddAsync(TEntity item)
        {
            await dbSet.AddAsync(item);
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            dbSet.AddRange(items);
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await dbSet.AddRangeAsync(items);
        }

        public bool Delete(TEntity item)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public bool HardDelete(TEntity item)
        {
            dbSet.Remove(item);

            int updateCount = dbContext.SaveChanges();

            return updateCount > 0;
        }
        public async Task<bool> HardDeleteAsync(TEntity item)
        {
            dbSet.Remove(item);
            int updateCount = await dbContext.SaveChangesAsync();

            return updateCount > 0;
        }

        public bool Update(TEntity item)
        {
            try
            {
                dbSet.Attach(item);
                dbSet.Entry(item).State = EntityState.Modified;
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public async Task<bool> UpdateAsync(TEntity item)
        {
            try
            {
                dbSet.Attach(item);
                dbSet.Entry(item).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Count()
        {
            return dbSet.Count();
        }
        public Task<int> CountAsync()
        {
            return dbSet.CountAsync();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
           return dbContext.SaveChangesAsync();
        }
        
    }
}
