using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;

namespace CalisthenicsStore.Services.Admin.Interfaces
{
    public interface IExerciseManagementService
    {

        Task<IEnumerable<ExerciseManagementIndexViewModel>> GetExerciseBoardDataAsync();

        Task AddExerciseAsync(ExerciseCreateViewModel model);
    }
}
