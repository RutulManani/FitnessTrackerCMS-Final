using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTrackerCMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Excerciseequipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentExercise",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    ExercisesExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentExercise", x => new { x.EquipmentId, x.ExercisesExerciseId });
                    table.ForeignKey(
                        name: "FK_EquipmentExercise_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentExercise_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseEquipments",
                columns: table => new
                {
                    ExerciseEquipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseEquipments", x => x.ExerciseEquipmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentExercise_ExercisesExerciseId",
                table: "EquipmentExercise",
                column: "ExercisesExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentExercise");

            migrationBuilder.DropTable(
                name: "ExerciseEquipments");
        }
    }
}
