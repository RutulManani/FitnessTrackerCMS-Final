using FitnessTrackerCMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTrackerCMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get data for dashboard
            ViewBag.TotalWorkouts = await _context.Workouts.CountAsync();
            ViewBag.TotalExercises = await _context.Exercises.CountAsync();
            ViewBag.TotalEquipment = await _context.Equipment.CountAsync();
            ViewBag.RecentWorkouts = await _context.Workouts
                .Where(w => w.Date >= DateTime.Now.AddDays(-7))
                .CountAsync();

            ViewBag.RecentWorkoutsList = await _context.Workouts
                .OrderByDescending(w => w.Date)
                .Take(5)
                .Select(w => new {
                    w.Name,
                    w.Date,
                    w.Duration,
                    ExerciseCount = w.WorkoutExercises.Count
                })
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}