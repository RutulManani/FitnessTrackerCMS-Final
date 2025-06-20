using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackerCMS.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public string Difficulty { get; set; }

        // Navigation properties
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
        public ICollection<ExerciseEquipment> ExerciseEquipments { get; set; }

        [NotMapped]
        public ICollection<Workout> Workouts => WorkoutExercises?.Select(we => we.Workout).ToList();
        [NotMapped]
        public ICollection<Equipment> Equipment => ExerciseEquipments?.Select(ee => ee.Equipment).ToList();
    }
}
