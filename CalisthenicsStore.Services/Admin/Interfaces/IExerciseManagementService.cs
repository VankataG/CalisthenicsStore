using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IExerciseManagementService
    {

        Task<IEnumerable<ExerciseManagementIndexViewModel>> GetExerciseBoardDataAsync();

        Task<bool> AddExerciseAsync(ExerciseCreateViewModel model);

        //Edit
        Task<ExerciseCreateViewModel?> GetEditableExerciseAsync(Guid id);

        Task<bool> EditExerciseAsync(ExerciseCreateViewModel model);
    }
}
