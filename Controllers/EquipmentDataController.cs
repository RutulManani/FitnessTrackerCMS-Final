using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.Models;
using FitnessTrackerCMS.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerCMS.Controllers
{
    /// <summary>
    /// API controller for managing fitness equipment
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentDataController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all equipment with exercise count
        /// </summary>
        /// <returns>List of equipment with exercise count</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentListDto>>> GetEquipment()
        {
            return await _context.Equipment
                .Select(e => new EquipmentListDto
                {
                    EquipmentId = e.EquipmentId,
                    Name = e.Name,
                    ExerciseCount = e.Exercises.Count
                })
                .ToListAsync();
        }

        /// <summary>
        /// Gets specific equipment by ID with all related exercises
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <returns>Equipment details with exercises</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDto>> GetEquipment(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Exercises)
                .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return new EquipmentDto
            {
                EquipmentId = equipment.EquipmentId,
                Name = equipment.Name,
                Description = equipment.Description,
                Exercises = equipment.Exercises.Select(ex => new ExerciseDto
                {
                    ExerciseId = ex.ExerciseId,
                    Name = ex.Name,
                    MuscleGroup = ex.MuscleGroup,
                    Difficulty = ex.Difficulty
                }).ToList()
            };
        }

        /// <summary>
        /// Creates new equipment
        /// </summary>
        /// <param name="equipmentDto">Equipment data</param>
        /// <returns>Created equipment</returns>
        [HttpPost]
        public async Task<ActionResult<EquipmentDto>> PostEquipment(EquipmentDto equipmentDto)
        {
            var equipment = new Equipment
            {
                Name = equipmentDto.Name,
                Description = equipmentDto.Description
            };

            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            equipmentDto.EquipmentId = equipment.EquipmentId;
            return CreatedAtAction("GetEquipment", new { id = equipment.EquipmentId }, equipmentDto);
        }

        /// <summary>
        /// Updates existing equipment
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <param name="equipmentDto">Updated equipment data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, EquipmentDto equipmentDto)
        {
            if (id != equipmentDto.EquipmentId)
            {
                return BadRequest();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            equipment.Name = equipmentDto.Name;
            equipment.Description = equipmentDto.Description;

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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
        /// Deletes equipment
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets exercises for specific equipment
        /// </summary>
        /// <param name="id">Equipment ID</param>
        /// <returns>List of exercises</returns>
        [HttpGet("{id}/exercises")]
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetExercisesForEquipment(int id)
        {
            var equipment = await _context.Equipment
                .Include(e => e.Exercises)
                .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment.Exercises.Select(e => new ExerciseDto
            {
                ExerciseId = e.ExerciseId,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup,
                Difficulty = e.Difficulty
            }).ToList();
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.EquipmentId == id);
        }
    }
}