using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.Models;
using FitnessTrackerCMS.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerCMS.Controllers
{
    /// <summary>
    /// API controller for managing exercises
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExerciseDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all exercises with equipment count
        /// </summary>
        /// <returns>List of exercises with equipment count</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseListDto>>> GetExercises()
        {
            return await _context.Exercises
                .Select(e => new ExerciseListDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Difficulty = e.Difficulty,
                    EquipmentCount = e.Equipment.Count
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets specific exercise by ID with all related equipment
        /// </summary>
        /// <param name="id">Exercise ID</param>
        /// <returns>Exercise details with equipment</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDto>> GetExercise(int id)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Equipment)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return new ExerciseDto
            {
                ExerciseId = exercise.ExerciseId,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup,
                Difficulty = exercise.Difficulty,
                Equipment = exercise.Equipment.Select(eq => new EquipmentDto
                {
                    EquipmentId = eq.EquipmentId,
                    Name = eq.Name,
                    Description = eq.Description
                }).ToList()
            };
        }

        /// <summary>
        /// Creates new exercise
        /// </summary>
        /// <param name="exerciseDto">Exercise data</param>
        /// <returns>Created exercise</returns>
        [HttpPost]
        public async Task<ActionResult<ExerciseDto>> PostExercise(ExerciseDto exerciseDto)
        {
            var exercise = new Exercise
            {
                Name = exerciseDto.Name,
                MuscleGroup = exerciseDto.MuscleGroup,
                Difficulty = exerciseDto.Difficulty
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            exerciseDto.ExerciseId = exercise.ExerciseId;
            return CreatedAtAction("GetExercise", new { id = exercise.ExerciseId }, exerciseDto);
        }

        /// <summary>
        /// Updates existing exercise
        /// </summary>
        /// <param name="id">Exercise ID</param>
        /// <param name="exerciseDto">Updated exercise data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, ExerciseDto exerciseDto)
        {
            if (id != exerciseDto.ExerciseId)
            {
                return BadRequest();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            exercise.Name = exerciseDto.Name;
            exercise.MuscleGroup = exerciseDto.MuscleGroup;
            exercise.Difficulty = exerciseDto.Difficulty;

            _context.Entry(exercise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseExists(id))
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
        /// Deletes exercise
        /// </summary>
        /// <param name="id">Exercise ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Adds equipment to exercise
        /// </summary>
        /// <param name="exerciseId">Exercise ID</param>
        /// <param name="equipmentId">Equipment ID</param>
        /// <returns>No content</returns>
        [HttpPost("{exerciseId}/equipment/{equipmentId}")]
        public async Task<IActionResult> AddEquipmentToExercise(int exerciseId, int equipmentId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Equipment)
                .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            var equipment = await _context.Equipment.FindAsync(equipmentId);

            if (exercise == null || equipment == null)
            {
                return NotFound();
            }

            exercise.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Removes equipment from exercise
        /// </summary>
        /// <param name="exerciseId">Exercise ID</param>
        /// <param name="equipmentId">Equipment ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{exerciseId}/equipment/{equipmentId}")]
        public async Task<IActionResult> RemoveEquipmentFromExercise(int exerciseId, int equipmentId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Equipment)
                .FirstOrDefaultAsync(e => e.ExerciseId == exerciseId);

            var equipment = await _context.Equipment.FindAsync(equipmentId);

            if (exercise == null || equipment == null)
            {
                return NotFound();
            }

            exercise.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.ExerciseId == id);
        }
    }
}