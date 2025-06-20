namespace FitnessTrackerCMS.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // (ExerciseEquipment junction table)
        public ICollection<Exercise> Exercises { get; set; }
    }
}
