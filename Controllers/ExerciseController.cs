using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.DTOs;
using FitnessTrackerCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessTrackerCMS.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercise
        public async Task<IActionResult> Index()
        {
            var exercises = await _context.Exercises
                .Select(e => new ExerciseListDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Difficulty = e.Difficulty,
                    EquipmentCount = e.ExerciseEquipments.Count
                })
                .ToListAsync();

            return View(exercises);
        }

        // GET: Exercise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.ExerciseEquipments)
                .ThenInclude(ee => ee.Equipment)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null)
            {
                return NotFound();
            }

            var exerciseDto = new ExerciseDto
            {
                ExerciseId = exercise.ExerciseId,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup,
                Difficulty = exercise.Difficulty,
                Equipment = exercise.ExerciseEquipments.Select(ee => new EquipmentDto
                {
                    EquipmentId = ee.Equipment.EquipmentId,
                    Name = ee.Equipment.Name,
                    Description = ee.Equipment.Description
                }).ToList()
            };

            return View(exerciseDto);
        }

        // GET: Exercise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exercise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,MuscleGroup,Difficulty")] ExerciseDto exerciseDto)
        {
            if (ModelState.IsValid)
            {
                var exercise = new Exercise
                {
                    Name = exerciseDto.Name,
                    MuscleGroup = exerciseDto.MuscleGroup,
                    Difficulty = exerciseDto.Difficulty
                };

                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseDto);
        }

        // GET: Exercise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }

            var exerciseDto = new ExerciseDto
            {
                ExerciseId = exercise.ExerciseId,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup,
                Difficulty = exercise.Difficulty
            };

            return View(exerciseDto);
        }

        // POST: Exercise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseId,Name,MuscleGroup,Difficulty")] ExerciseDto exerciseDto)
        {
            if (id != exerciseDto.ExerciseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var exercise = await _context.Exercises.FindAsync(id);
                if (exercise == null)
                {
                    return NotFound();
                }

                exercise.Name = exerciseDto.Name;
                exercise.MuscleGroup = exerciseDto.MuscleGroup;
                exercise.Difficulty = exerciseDto.Difficulty;

                _context.Update(exercise);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(exerciseDto);
        }

        // GET: Exercise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .FirstOrDefaultAsync(m => m.ExerciseId == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}