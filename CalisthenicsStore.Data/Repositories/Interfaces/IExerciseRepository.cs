using CalisthenicsStore.Data.Models;

namespace CalisthenicsStore.Data.Repositories.Interfaces
{
    public interface IExerciseRepository 
        : IRepository<Exercise, int>, IAsyncRepository<Exercise, int>
    {

    }
}
