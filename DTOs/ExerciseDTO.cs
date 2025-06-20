namespace FitnessTrackerCMS.DTOs
{
    public class ExerciseDto
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
        public string Difficulty { get; set; }
        public List<EquipmentDto> Equipment { get; set; } = new List<EquipmentDto>();
    }
}
