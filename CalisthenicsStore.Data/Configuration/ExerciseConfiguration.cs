using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalisthenicsStore.Data.Configuration
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> entity)
        {
            entity
                .HasKey(e => e.Id);

            entity
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity
                .Property(e => e.Description)
                .HasMaxLength(500);

            entity
                .Property(e => e.ImageUrl)
                .HasMaxLength(500);

            entity
                .HasData(this.SeedExercises());
        }

        private List<Exercise> SeedExercises()
        {
            List<Exercise> exercises = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = 1,
                    Name = "Push-up",
                    Description = "A push-up is a common calisthenics exercise that involves lowering the body by bending the arms and then pushing back up to the starting position, primarily working the chest, shoulders, and triceps. It's a full-body exercise that can be modified to suit different fitness levels.",
                    ImageUrl = "https://static.wikia.nocookie.net/parkour/images/e/e0/Push_Up.jpg/revision/latest/scale-to-width-down/483?cb=20141122161108",
                    Level = DifficultyLevel.Beginner
                },
                new Exercise()
                {
                    Id = 2,
                    Name = "Pull-up",
                    Description = "Pull-up (exercise): This is a strength training exercise where you lift your body weight by pulling yourself upwards against gravity, typically using a bar. It primarily works the muscles in your back, arms, and core.",
                    ImageUrl = "https://mikereinold.com/wp-content/uploads/rookie-mistakes-the-pullup-main.jpg",
                    Level = DifficultyLevel.Beginner
                },
                new Exercise()
                {
                    Id = 3,
                    Name = "Muscle-up",
                    Description = "A muscle-up is an advanced calisthenics exercise that combines a pull-up and a dip, requiring both pulling and pushing movements of the upper body. It involves transitioning from a hanging position below a bar or rings to a position above, with arms extended. Muscle-ups are known for building upper body strength and are considered a critical survival skill by some.",
                    ImageUrl = "https://i0.wp.com/workoutlabs.com/wp-content/uploads/watermarked/Muscle_Up.png?w=1360",
                    Level = DifficultyLevel.Intermediate
                },
                new Exercise()
                {
                    Id = 4,
                    Name = "Front Lever",
                    Description = "The front lever is a challenging calisthenics exercise where the body is held in a horizontal, straight-arm position, parallel to the ground, with the front of the body facing upwards. It requires immense strength, particularly in the core, back, and shoulders, as well as full-body tension and control.",
                    ImageUrl = "https://calisthenics.com/wp-content/uploads/2025/01/full-front-lever.jpg",
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = 5,
                    Name = "Planche",
                    Description = "The planche is an advanced bodyweight exercise, primarily used in gymnastics and calisthenics, where the body is held parallel to the ground, supported only by straight arms. It requires significant strength, particularly in the shoulders, biceps, and core, as well as excellent balance. The planche is an isometric hold, meaning the body remains stationary while under tension.",
                    ImageUrl = "https://static.wikia.nocookie.net/street-workout/images/5/56/Leeplanche.jpg/revision/latest?cb=20221209193627",
                    Level = DifficultyLevel.Insane
                },

            };

            return exercises;
        }
    }
}
