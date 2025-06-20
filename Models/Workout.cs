namespace FitnessTrackerCMS.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } // in minutes
        public int? CaloriesEstimated { get; set; }

        // Many-to-Many with Exercises (via WorkoutExercise junction table)
        public ICollection<Exercise> Exercises { get; set; }
    }
}
