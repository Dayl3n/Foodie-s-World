using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodiesWorld.Data;
using FoodiesWorld.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodiesWorld.Controllers
{
    public class VisitsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public VisitsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Visits
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Calendar", "Visits"); //Return to Calendar View
        }

        // Get: Visits/Calendar
        public async Task<IActionResult> Calendar()
        {
            var applicationDbContext = _context.Visit.Include(v => v.Restaurant).Include(v => v.User);

            //ViewData for select menu in the Calendar.cshtml
            ViewData["RestaurantCities"] = new SelectList(_context.Restaurant.Select(r => r.City).Distinct(), "City");
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Visits/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visit
                .Include(v => v.Restaurant)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // GET: Visits/Create
        public IActionResult Create(string restaurantId)
        {

            // Renders pre-filled data in visit creation form:
            Visit visit = new Visit
            {
                Id = Guid.NewGuid().ToString(),
                RestaurantId = restaurantId,
                UserId = _userManager.GetUserId(User),
                Date = DateTime.UtcNow
            };

            return View(visit);
        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,IsBooked,RestaurantId,UserId")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", visit.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", visit.UserId);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visit.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", visit.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", visit.UserId);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Date,IsBooked,RestaurantId,UserId")] Visit visit)
        {
            if (id != visit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", visit.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", visit.UserId);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visit
                .Include(v => v.Restaurant)
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var visit = await _context.Visit.FindAsync(id);
            if (visit != null)
            {
                _context.Visit.Remove(visit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitExists(string id)
        {
            return _context.Visit.Any(e => e.Id == id);
        }
    }
}
