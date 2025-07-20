using System.ComponentModel.DataAnnotations;
using CalisthenicsStore.Common.Enums;

namespace CalisthenicsStore.ViewModels.Exercise
{
    public class ExerciseViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public DifficultyLevel LevelEnum { get; set; }

        [Required]
        public string Level { get; set; } = null!;
    }
}
