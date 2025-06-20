using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.DTOs;
using FitnessTrackerCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTrackerCMS.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Workout
        public async Task<IActionResult> Index()
        {
            var workouts = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .Select(w => new WorkoutListDto
                {
                    WorkoutId = w.WorkoutId,
                    Name = w.Name,
                    Date = w.Date,
                    Duration = w.Duration,
                    CaloriesEstimated = w.CaloriesEstimated,
                    ExerciseCount = w.WorkoutExercises.Count
                })
                .ToListAsync();

            return View(workouts);
        }

        // GET: Workout/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.WorkoutId == id);

            if (workout == null)
            {
                return NotFound();
            }

            var workoutDto = new WorkoutDto
            {
                WorkoutId = workout.WorkoutId,
                Name = workout.Name,
                Date = workout.Date,
                Duration = workout.Duration,
                CaloriesEstimated = workout.CaloriesEstimated,
                Exercises = workout.WorkoutExercises
                    .Select(we => new ExerciseDto
                    {
                        ExerciseId = we.Exercise.ExerciseId,
                        Name = we.Exercise.Name,
                        MuscleGroup = we.Exercise.MuscleGroup,
                        Difficulty = we.Exercise.Difficulty
                    }).ToList()
            };

            return View(workoutDto);
        }

        // GET: Workout/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workout/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Date,Duration,CaloriesEstimated")] WorkoutDto workoutDto)
        {
            if (ModelState.IsValid)
            {
                var workout = new Workout
                {
                    Name = workoutDto.Name,
                    Date = workoutDto.Date,
                    Duration = workoutDto.Duration,
                    CaloriesEstimated = workoutDto.CaloriesEstimated
                };

                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workoutDto);
        }

        // GET: Workout/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }

            var workoutDto = new WorkoutDto
            {
                WorkoutId = workout.WorkoutId,
                Name = workout.Name,
                Date = workout.Date,
                Duration = workout.Duration,
                CaloriesEstimated = workout.CaloriesEstimated
            };

            return View(workoutDto);
        }

        // POST: Workout/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkoutId,Name,Date,Duration,CaloriesEstimated")] WorkoutDto workoutDto)
        {
            if (id != workoutDto.WorkoutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var workout = await _context.Workouts.FindAsync(id);
                    if (workout == null)
                    {
                        return NotFound();
                    }

                    workout.Name = workoutDto.Name;
                    workout.Date = workoutDto.Date;
                    workout.Duration = workoutDto.Duration;
                    workout.CaloriesEstimated = workoutDto.CaloriesEstimated;

                    _context.Update(workout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workoutDto);
        }

        // GET: Workout/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);
            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Workout/AddExercise
        [HttpPost]
        public async Task<IActionResult> AddExercise(int workoutId, int exerciseId)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(w => w.WorkoutId == workoutId);

            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (workout == null || exercise == null)
            {
                return NotFound();
            }

            if (!workout.WorkoutExercises.Any(we => we.ExerciseId == exerciseId))
            {
                workout.WorkoutExercises.Add(new WorkoutExercise
                {
                    WorkoutId = workoutId,
                    ExerciseId = exerciseId
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = workoutId });
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.WorkoutId == id);
        }
    }
}