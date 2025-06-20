namespace FitnessTrackerCMS.DTOs
{
    public class ExerciseListDto
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public string Difficulty { get; set; }
        public int EquipmentCount { get; set; }
    }
}
