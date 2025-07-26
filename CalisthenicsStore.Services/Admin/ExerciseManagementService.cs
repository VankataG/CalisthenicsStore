using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Admin.Interfaces;
using CalisthenicsStore.ViewModels.Admin.ExerciseManagement;
using CalisthenicsStore.ViewModels.Exercise;
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

        public async Task AddExerciseAsync(ExerciseCreateViewModel model)
        {
            Exercise newExercise = new Exercise()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl ?? "/images/no-image.jpg",
                Level = model.Level
            };

            await exerciseRepository.AddAsync(newExercise);
        }
    }
}
