using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data;
using CalisthenicsStore.Services.Interfaces;
using CalisthenicsStore.ViewModels.Exercise;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsStore.Services
{
    public class ExerciseService(CalisthenicsStoreDbContext context) : IExerciseService
    {
        public async Task<IEnumerable<ExerciseViewModel>> GetAllExercisesAsync()
        {
            return await context
                .Exercises
                .AsNoTracking()
                .Select(e => new ExerciseViewModel()
                {
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
            return await context
                .Exercises
                .AsNoTracking()
                .Where(e => e.Level  == level)
                .Select(e => new ExerciseViewModel()
                {
                    Name = e.Name,
                    Description = e.Description,
                    ImageUrl = e.ImageUrl ?? "/images/no-image.jpg",
                    LevelEnum = e.Level,
                    Level = e.Level.ToString()
                })
                .ToListAsync();
        }
    }
}
