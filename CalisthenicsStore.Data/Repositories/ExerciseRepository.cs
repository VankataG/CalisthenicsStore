using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;

namespace CalisthenicsStore.Data.Repositories
{
    public class ExerciseRepository : BaseRepository<Exercise, int>, IExerciseRepository
    {
        public ExerciseRepository(CalisthenicsStoreDbContext dbContext) : base(dbContext)
        {
        }
    }
}
