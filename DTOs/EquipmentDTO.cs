using System.ComponentModel.DataAnnotations;

namespace FitnessTrackerCMS.DTOs
{
    public class EquipmentDto
    {
        public int EquipmentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    }
}
