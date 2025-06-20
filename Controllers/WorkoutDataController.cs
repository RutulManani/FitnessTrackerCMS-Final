using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.Models;
using FitnessTrackerCMS.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerCMS.Controllers
{
    /// <summary>
    /// API controller for managing workouts
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkoutDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all workouts with exercise count
        /// </summary>
        /// <returns>List of workouts with exercise count</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutListDto>>> GetWorkouts()
        {
            return await _context.Workouts
                .Select(w => new WorkoutListDto
                {
                    WorkoutId = w.WorkoutId,
                    Name = w.Name,
                    Date = w.Date,
                    Duration = w.Duration,
                    CaloriesEstimated = w.CaloriesEstimated,
                    ExerciseCount = w.Exercises.Count
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets specific workout by ID with all related exercises
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <returns>Workout details with exercises</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDto>> GetWorkout(int id)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.WorkoutId == id);

            if (workout == null)
            {
                return NotFound();
            }

            return new WorkoutDto
            {
                WorkoutId = workout.WorkoutId,
                Name = workout.Name,
                Date = workout.Date,
                Duration = workout.Duration,
                CaloriesEstimated = workout.CaloriesEstimated,
                Exercises = workout.Exercises.Select(e => new ExerciseDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Difficulty = e.Difficulty
                }).ToList()
            };
        }

        /// <summary>
        /// Creates new workout
        /// </summary>
        /// <param name="workoutDto">Workout data</param>
        /// <returns>Created workout</returns>
        [HttpPost]
        public async Task<ActionResult<WorkoutDto>> PostWorkout(WorkoutDto workoutDto)
        {
            var workout = new Workout
            {
                Name = workoutDto.Name,
                Date = workoutDto.Date,
                Duration = workoutDto.Duration,
                CaloriesEstimated = workoutDto.CaloriesEstimated
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            workoutDto.WorkoutId = workout.WorkoutId;
            return CreatedAtAction("GetWorkout", new { id = workout.WorkoutId }, workoutDto);
        }

        /// <summary>
        /// Updates existing workout
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <param name="workoutDto">Updated workout data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkout(int id, WorkoutDto workoutDto)
        {
            if (id != workoutDto.WorkoutId)
            {
                return BadRequest();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            workout.Name = workoutDto.Name;
            workout.Date = workoutDto.Date;
            workout.Duration = workoutDto.Duration;
            workout.CaloriesEstimated = workoutDto.CaloriesEstimated;

            _context.Entry(workout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes workout
        /// </summary>
        /// <param name="id">Workout ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds exercise to workout
        /// </summary>
        /// <param name="workoutId">Workout ID</param>
        /// <param name="exerciseId">Exercise ID</param>
        /// <returns>No content</returns>
        [HttpPost("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> AddExerciseToWorkout(int workoutId, int exerciseId)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.WorkoutId == workoutId);

            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (workout == null || exercise == null)
            {
                return NotFound();
            }

            workout.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Removes exercise from workout
        /// </summary>
        /// <param name="workoutId">Workout ID</param>
        /// <param name="exerciseId">Exercise ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{workoutId}/exercises/{exerciseId}")]
        public async Task<IActionResult> RemoveExerciseFromWorkout(int workoutId, int exerciseId)
        {
            var workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.WorkoutId == workoutId);

            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (workout == null || exercise == null)
            {
                return NotFound();
            }

            workout.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.WorkoutId == id);
        }
    }
}