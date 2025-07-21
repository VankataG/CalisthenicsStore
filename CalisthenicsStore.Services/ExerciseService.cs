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
                .GetAllAttached()
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
                .GetAllAttached()
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

        //Create
        public async Task AddExerciseAsync(ExerciseInputModel model)
        {
            Exercise newExercise = new Exercise()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl ?? "/images/no-image.jpg",
                Level = model.Level
            };

            await repository.AddAsync(newExercise);
        }

        //Edit
        public async Task<ExerciseInputModel?> GetEditableExerciseAsync(Guid id)
        {
            ExerciseInputModel? editableExercise = await this.repository
                .GetAllAttached()
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new ExerciseInputModel()
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

        public async Task EditExerciseAsync(ExerciseInputModel model)
        {
            Exercise? editableExercise = await this.repository
                .GetAllAttached()
                .SingleOrDefaultAsync(e => e.Id == model.Id);

            if (editableExercise != null)
            {
                editableExercise.Name = model.Name;
                editableExercise.Description = model.Description;
                editableExercise.ImageUrl = model.ImageUrl;
                editableExercise.Level = model.Level;

                await this.repository.UpdateAsync(editableExercise);
            }
        }

        

    }
}
