using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.ViewModels.Exercise;

namespace CalisthenicsStore.Services.Interfaces
{
    public interface IExerciseService
    {
        //Read
        Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync();

        Task<IEnumerable<ExerciseViewModel>> GetExercisesByLevelAsync(DifficultyLevel level);

        Task<ExerciseViewModel?> GetExerciseDetailsAsync(Guid id);


        //Create
        Task AddExerciseAsync(ExerciseInputModel inputModel);

        //Edit
        Task<ExerciseInputModel?> GetEditableExerciseAsync(Guid id);

        Task EditExerciseAsync(ExerciseInputModel model);

    }
}
