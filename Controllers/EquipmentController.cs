using FitnessTrackerCMS.Data;
using FitnessTrackerCMS.DTOs;
using FitnessTrackerCMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTrackerCMS.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipment
        public async Task<IActionResult> Index()
        {
            var equipment = await _context.Equipment
                .Select(e => new EquipmentListDto
                {
                    EquipmentId = e.EquipmentId,
                    Name = e.Name,
                    ExerciseCount = e.Exercises.Count
                })
                .ToListAsync();

            return View(equipment);
        }

        // GET: Equipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .Include(e => e.Exercises)
                .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                return NotFound();
            }

            var equipmentDto = new EquipmentDto
            {
                EquipmentId = equipment.EquipmentId,
                Name = equipment.Name,
                Description = equipment.Description,
                Exercises = equipment.Exercises.Select(e => new ExerciseDto
                {
                    ExerciseId = e.ExerciseId,
                    Name = e.Name,
                    MuscleGroup = e.MuscleGroup,
                    Difficulty = e.Difficulty
                }).ToList()
            };

            return View(equipmentDto);
        }

        // GET: Equipment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EquipmentDto equipmentDto)
        {
            if (ModelState.IsValid)
            {
                var equipment = new Equipment
                {
                    Name = equipmentDto.Name,
                    Description = equipmentDto.Description
                };

                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentDto);
        }

        // GET: Equipment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            var equipmentDto = new EquipmentDto
            {
                EquipmentId = equipment.EquipmentId,
                Name = equipment.Name,
                Description = equipment.Description
            };

            return View(equipmentDto);
        }

        // POST: Equipment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EquipmentDto equipmentDto)
        {
            if (id != equipmentDto.EquipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var equipment = await _context.Equipment.FindAsync(id);
                if (equipment == null)
                {
                    return NotFound();
                }

                equipment.Name = equipmentDto.Name;
                equipment.Description = equipmentDto.Description;

                _context.Update(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipmentDto);
        }

        // GET: Equipment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipment
                .FirstOrDefaultAsync(m => m.EquipmentId == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.EquipmentId == id);
        }
    }
}