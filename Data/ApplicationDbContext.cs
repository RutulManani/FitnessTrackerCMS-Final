using FitnessTrackerCMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ExerciseEquipment> ExerciseEquipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many between Exercise and Equipment
            modelBuilder.Entity<ExerciseEquipment>()
                .HasKey(ee => new { ee.ExerciseId, ee.EquipmentId });

            modelBuilder.Entity<ExerciseEquipment>()
                .HasOne(ee => ee.Exercise)
                .WithMany(e => e.ExerciseEquipments)
                .HasForeignKey(ee => ee.ExerciseId);

            modelBuilder.Entity<ExerciseEquipment>()
                .HasOne(ee => ee.Equipment)
                .WithMany(e => e.ExerciseEquipments)
                .HasForeignKey(ee => ee.EquipmentId);

            // Configure many-to-many between Workout and Exercise
            modelBuilder.Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutId, we.ExerciseId });

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);
        }
    }
}