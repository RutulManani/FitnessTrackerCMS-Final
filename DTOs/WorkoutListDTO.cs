namespace FitnessTrackerCMS.DTOs
{
    public class WorkoutListDto
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int? CaloriesEstimated { get; set; }
        public int ExerciseCount { get; set; }
    }
}
