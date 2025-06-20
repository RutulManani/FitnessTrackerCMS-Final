using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.DTOs;
using FitnessTrackerCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
                .Include(w => w.Exercises)
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
                Exercises = workout.Exercises.Select(e => new ExerciseDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Difficulty = e.Difficulty
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
    }
}