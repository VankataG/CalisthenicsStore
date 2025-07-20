using CalisthenicsStore.Common.Enums;

namespace CalisthenicsStore.Data.Models
{
    public class Exercise
    {

        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public DifficultyLevel Level { get; set; }

        public bool IsDeleted { get; set; }
    }
}
