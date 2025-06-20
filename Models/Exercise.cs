namespace FitnessTrackerCMS.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public string Difficulty { get; set; }

        // (WorkoutExercise junction table)
        public ICollection<Workout> Workouts { get; set; }

        // (ExerciseEquipment junction table)
        public ICollection<Equipment> Equipment { get; set; }


    }
}
