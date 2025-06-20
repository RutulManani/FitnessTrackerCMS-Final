using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTrackerCMS.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property
        public ICollection<ExerciseEquipment> ExerciseEquipments { get; set; }

        [NotMapped]
        public ICollection<Exercise> Exercises => ExerciseEquipments?.Select(ee => ee.Exercise).ToList();
    }
}