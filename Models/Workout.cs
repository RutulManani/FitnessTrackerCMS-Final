using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackerCMS.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; } // in minutes
        public int? CaloriesEstimated { get; set; }

        // Navigation property for junction table
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}