namespace FitnessTrackerCMS.DTOs
{
    public class WorkoutDto
    {
        public int WorkoutId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public int? CaloriesEstimated { get; set; }
        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    }
}
