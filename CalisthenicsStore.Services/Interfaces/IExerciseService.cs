using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.ViewModels.Exercise;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync();

        Task<IEnumerable<ExerciseViewModel>> GetExercisesByLevelAsync(DifficultyLevel level);
    }
}
