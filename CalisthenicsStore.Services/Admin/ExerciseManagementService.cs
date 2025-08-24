using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services.Admin
{
    public class ExerciseManagementService : IExerciseManagementService
    {
        private readonly IExerciseRepository exerciseRepository;

        public ExerciseManagementService(IExerciseRepository exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task<IEnumerable<ExerciseManagementIndexViewModel>> GetExerciseBoardDataAsync()
        {
            IEnumerable<ExerciseManagementIndexViewModel> allExercises = await exerciseRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Select(e => new ExerciseManagementIndexViewModel()
                {
                    Id = e.Id.ToString(),
                    Name = e.Name,
                    Level = e.Level.ToString(),
                    IsDeleted = e.IsDeleted
                })
                .ToArrayAsync();

            return allExercises;
        }

        public async Task<bool> AddExerciseAsync(ExerciseCreateViewModel model)
        {
            Exercise newExercise = new Exercise()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl ?? "/images/no-image.jpg",
                Level = model.Level
            };

            return await exerciseRepository.AddAsync(newExercise);
        }

        public async Task<ExerciseCreateViewModel?> GetEditableExerciseAsync(Guid id)
        {
            ExerciseCreateViewModel? editableExercise = await this.exerciseRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new ExerciseCreateViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl,
                    Level = e.Level
                })
                .SingleOrDefaultAsync();

            return editableExercise;
        }

        public async Task<bool> EditExerciseAsync(ExerciseCreateViewModel model)
        {
            bool result = false;

            Exercise? editableExercise = await this.exerciseRepository
                .GetAllAttached()
                .SingleOrDefaultAsync(e => e.Id == model.Id);

            if (editableExercise != null)
            {
                editableExercise.Name = model.Name;
                editableExercise.Description = model.Description;
                editableExercise.ImageUrl = model.ImageUrl;
                editableExercise.Level = model.Level;

                result = await this.exerciseRepository.UpdateAsync(editableExercise);
            }

            return result;
        }

        public async Task<Tuple<bool, string>> DeleteOrRestoreAsync(Guid id)
        {
            bool result = false;
            string action = string.Empty;
            Exercise? exercise = await exerciseRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(e => e.Id == id);

            if (exercise != null)
            {
                action = exercise.IsDeleted ? "restore" : "delete";

                exercise.IsDeleted = !exercise.IsDeleted;
                await exerciseRepository.SaveChangesAsync();
                result = true;
            }

            return new Tuple<bool, string>(result, action);
        }
    }
}
