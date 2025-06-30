using CalisthenicsStore.ViewModels.Exercise;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync();
    }
}
