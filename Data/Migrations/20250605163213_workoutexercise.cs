using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTrackerCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Workoutexercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseWorkout",
                columns: table => new
                {
                    ExercisesExerciseId = table.Column<int>(type: "int", nullable: false),
                    WorkoutsWorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWorkout", x => new { x.ExercisesExerciseId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_ExerciseWorkout_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseWorkout_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    WorkoutExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => x.WorkoutExerciseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseWorkout_WorkoutsWorkoutId",
                table: "ExerciseWorkout",
                column: "WorkoutsWorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseWorkout");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");
        }
    }
}
