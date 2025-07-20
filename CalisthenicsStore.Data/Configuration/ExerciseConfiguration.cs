using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using CalisthenicsStore.Common.Enums;
using CalisthenicsStore.Data.Models;
using static CalisthenicsStore.Common.Constants.Exercise;


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
                .HasMaxLength(NameMaxLength);

            entity
                .Property(e => e.Description)
                .HasMaxLength(DescriptionMaxLength);

            entity
                .Property(e => e.ImageUrl)
                .HasMaxLength(ImageUrlMaxLength);

            entity
                .Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity
                .HasData(this.SeedExercises());

            entity
                .HasQueryFilter(e => e.IsDeleted == false);
        }

        private List<Exercise> SeedExercises()
        {
            List<Exercise> exercises = new List<Exercise>()
            {
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Push-up",
                    Description = "A push-up is a common calisthenics exercise that involves lowering the body by bending the arms and then pushing back up to the starting position, primarily working the chest, shoulders, and triceps. It's a full-body exercise that can be modified to suit different fitness levels.",
                    ImageUrl = "https://hips.hearstapps.com/hmg-prod/images/press-up-1583236041.jpg",
                    Level = DifficultyLevel.Beginner
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Pull-up",
                    Description = "Pull-up (exercise): This is a strength training exercise where you lift your body weight by pulling yourself upwards against gravity, typically using a bar. It primarily works the muscles in your back, arms, and core.",
                    ImageUrl = "https://mikereinold.com/wp-content/uploads/rookie-mistakes-the-pullup-main.jpg",
                    Level = DifficultyLevel.Beginner
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Muscle-up",
                    Description = "A muscle-up is an advanced calisthenics exercise that combines a pull-up and a dip, requiring both pulling and pushing movements of the upper body. It involves transitioning from a hanging position below a bar or rings to a position above, with arms extended. Muscle-ups are known for building upper body strength and are considered a critical survival skill by some.",
                    ImageUrl = "https://i0.wp.com/workoutlabs.com/wp-content/uploads/watermarked/Muscle_Up.png?w=1360",
                    Level = DifficultyLevel.Intermediate
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Front Lever",
                    Description = "The front lever is a challenging calisthenics exercise where the body is held in a horizontal, straight-arm position, parallel to the ground, with the front of the body facing upwards. It requires immense strength, particularly in the core, back, and shoulders, as well as full-body tension and control.",
                    ImageUrl = "https://calisthenics.com/wp-content/uploads/2025/01/full-front-lever.jpg",
                    Level = DifficultyLevel.Advanced
                },
                new Exercise()
                {
                    Id = Guid.NewGuid(),
                    Name = "Planche",
                    Description = "The planche is an advanced bodyweight exercise, primarily used in gymnastics and calisthenics, where the body is held parallel to the ground, supported only by straight arms. It requires significant strength, particularly in the shoulders, biceps, and core, as well as excellent balance. The planche is an isometric hold, meaning the body remains stationary while under tension.",
                    ImageUrl = "https://cdn.shopify.com/s/files/1/0568/6280/2107/files/full_planche_1.jpg",
                    Level = DifficultyLevel.Insane
                },

            };

            return exercises;
        }
    }
}
