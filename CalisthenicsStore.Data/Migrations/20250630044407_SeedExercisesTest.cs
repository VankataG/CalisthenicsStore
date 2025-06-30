using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CalisthenicsStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedExercisesTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Description", "ImageUrl", "Level", "Name" },
                values: new object[,]
                {
                    { 1, "A push-up is a common calisthenics exercise that involves lowering the body by bending the arms and then pushing back up to the starting position, primarily working the chest, shoulders, and triceps. It's a full-body exercise that can be modified to suit different fitness levels.", "https://static.wikia.nocookie.net/parkour/images/e/e0/Push_Up.jpg/revision/latest/scale-to-width-down/483?cb=20141122161108", 0, "Push-up" },
                    { 2, "Pull-up (exercise): This is a strength training exercise where you lift your body weight by pulling yourself upwards against gravity, typically using a bar. It primarily works the muscles in your back, arms, and core.", "https://mikereinold.com/wp-content/uploads/rookie-mistakes-the-pullup-main.jpg", 0, "Pull-up" },
                    { 3, "A muscle-up is an advanced calisthenics exercise that combines a pull-up and a dip, requiring both pulling and pushing movements of the upper body. It involves transitioning from a hanging position below a bar or rings to a position above, with arms extended. Muscle-ups are known for building upper body strength and are considered a critical survival skill by some.", "https://i0.wp.com/workoutlabs.com/wp-content/uploads/watermarked/Muscle_Up.png?w=1360", 1, "Muscle-up" },
                    { 4, "The front lever is a challenging calisthenics exercise where the body is held in a horizontal, straight-arm position, parallel to the ground, with the front of the body facing upwards. It requires immense strength, particularly in the core, back, and shoulders, as well as full-body tension and control.", "https://calisthenics.com/wp-content/uploads/2025/01/full-front-lever.jpg", 2, "Front Lever" },
                    { 5, "The planche is an advanced bodyweight exercise, primarily used in gymnastics and calisthenics, where the body is held parallel to the ground, supported only by straight arms. It requires significant strength, particularly in the shoulders, biceps, and core, as well as excellent balance. The planche is an isometric hold, meaning the body remains stationary while under tension.", "https://static.wikia.nocookie.net/street-workout/images/5/56/Leeplanche.jpg/revision/latest?cb=20221209193627", 3, "Planche" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
