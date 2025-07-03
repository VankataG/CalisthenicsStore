namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity, TKey>
    {
        TEntity GetById(TKey id);

        TEntity SingleOrDefault(Func<TKey, bool> predicate);

        TEntity FirstOrDefault(Func<TKey, bool> predicate);

        IEnumerable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAttacked();

        void Add(TEntity item);

        void AddRange(IEnumerable<TEntity> items);

        bool Delete(TEntity item);

        bool Update(TEntity item);

        int Count(TEntity item);

        void SaveChanges();
    }
}
