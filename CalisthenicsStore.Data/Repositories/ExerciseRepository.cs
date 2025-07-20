using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;

namespace CalisthenicsStore.Data.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise, Guid>, IExerciseRepository
    {
        public ExerciseRepository(CalisthenicsStoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
