using System.Linq.Expressions;
using System.Reflection;
using CalisthenicsStore.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CalisthenicsStore.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> 
        : IRepository<TEntity, TKey>, IAsyncRepository<TEntity, TKey> 
        where TEntity : class
    {

        protected readonly CalisthenicsStoreDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        protected BaseRepository(CalisthenicsStoreDbContext dbContext)
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

        public IQueryable<TEntity> GetAllAttached()
        {
            return dbSet
                .AsQueryable();
        }

        public bool Add(TEntity item)
        {
            try
            {
                dbSet.Add(item);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public async Task<bool> AddAsync(TEntity item)
        {
            try
            {
                await dbSet.AddAsync(item);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            dbSet.AddRange(items);
            dbContext.SaveChanges();
        }
        public async Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            await dbSet.AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
        }

        public bool Delete(TEntity item)
        {
            this.PerformSoftDelete(item);

            return this.Update(item);
        }
        public Task<bool> DeleteAsync(TEntity item)
        {
            this.PerformSoftDelete(item);

            return this.UpdateAsync(item);
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

        private void PerformSoftDelete(TEntity item)
        {
            PropertyInfo? isDeletedProperty = GetIsDeletedProperty(item);

            if (isDeletedProperty == null)
            {
                throw new InvalidOperationException();  //TODO: Add ExceptionMessages const
            }

            isDeletedProperty.SetValue(item, true);
        }
        private PropertyInfo? GetIsDeletedProperty(TEntity item)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(pi => pi.Name == "IsDeleted" && pi.PropertyType == typeof(bool));
        }
    }
}
