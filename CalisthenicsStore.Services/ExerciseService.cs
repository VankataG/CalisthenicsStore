using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data.Models;
using CalisthenicsStore.Data.Repositories.Interfaces;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository repository;

        public ExerciseService(IExerciseRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync()
        {
            return await repository
                .GetAllAttacked()
                .AsNoTracking()
                .Select(e => new ExerciseViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl ?? "/images/no-image.jpg",
                    LevelEnum = e.Level,
                    Level = e.Level.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ExerciseViewModel>> GetExercisesByLevelAsync(DifficultyLevel level)
        {
            return await repository
                .GetAllAttacked()
                .AsNoTracking()
                .Where(e => e.Level == level)
                .Select(e => new ExerciseViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl ?? "/images/no-image.jpg",
                    LevelEnum = e.Level,
                    Level = e.Level.ToString()
                })
                .ToListAsync();
        }

        public async Task<ExerciseViewModel?> GetExerciseDetailsAsync(Guid id)
        {
            Exercise? exercise = await repository.GetByIdAsync(id);

            if (exercise == null)
                return null;

            ExerciseViewModel model = new ExerciseViewModel()
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                ImageUrl = exercise.ImageUrl,
                LevelEnum = exercise.Level,
                Level = exercise.Level.ToString()
            };

            return model;

        }
    }
}
