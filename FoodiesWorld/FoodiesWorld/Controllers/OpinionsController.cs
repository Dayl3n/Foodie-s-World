using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodiesWorld.Data;
using FoodiesWorld.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security;
using Microsoft.AspNetCore.Identity;

namespace FoodiesWorld.Controllers
{
    public class OpinionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OpinionsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Opinions
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Search", "Restaurants");
        }

        // Get: Opinions/AllRevies (only admins)

        [Authorize(Roles="Admin")] 
        public async Task<IActionResult> AllRevies(int pg=1)
        {
            const int pageSize = 5; //Setting number of revies in one view
            if (pg < 1)
                pg = 1;

            int reviesCount = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Count();

            var RouteData = new Dictionary<string, string>();

            var pager = new Pager(reviesCount, pg, "Opinions", "AllRevies", RouteData, pageSize);


            int revsSkip = (pg - 1) * pageSize;
            var revies = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Skip(revsSkip).Take(pageSize);
            this.ViewBag.Pager = pager;

            return View("Index",revies);
        }

        public async Task<IActionResult> ShowOpinions(int pg = 1)
        {
            const int pageSize = 5; //Setting number of revies in one view
            if (pg < 1)
                pg = 1;

            int reviesCount = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Where(u => u.UserId == _userManager.GetUserId(User)).Count();

            var RouteData = new Dictionary<string, string>();

            var pager = new Pager(reviesCount, pg, "Opinions", "ShowOpinions", RouteData, pageSize);


            int revsSkip = (pg - 1) * pageSize;
            var revies = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Where( u => u.UserId==_userManager.GetUserId(User)).Skip(revsSkip).Take(pageSize);
            this.ViewBag.Pager = pager;

            return View("Index", revies);
        }


        //Get Opinions/SearchOpinion?resurantId=?
        public async Task<IActionResult> SearchOpinion(string restaurantId,int pg=1)
        {

            const int pageSize = 5; //Setting number of revies in one view
            if (pg < 1)
                pg = 1;

            int reviesCount = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Where(x => x.RestaurantId == restaurantId).Count();

            var RouteData = new Dictionary<string, string>
                {
                    { "restaurantId", restaurantId } //additional argument
                };

            var pager = new Pager(reviesCount, pg,"Opinions","SearchOpinion", RouteData, pageSize);

            int recSkip = (pg - 1) * pageSize;
            this.ViewBag.Pager = pager;

            var applicationDbContext = _context.Opinion.Include(o => o.Restaurant).Include(o => o.User).Where(x=> x.RestaurantId==restaurantId).Skip(recSkip).Take(pageSize);
            return View("index",await applicationDbContext.ToListAsync());
        }

        // GET: Opinions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion
                .Include(o => o.Restaurant)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // GET: Opinions/Create
        public IActionResult Create(string restaurantId)
        {

            Opinion opinion = new Opinion
            {
                Id = Guid.NewGuid().ToString(),
                RestaurantId = restaurantId,
                UserId = _userManager.GetUserId(User),
                Date = DateTime.UtcNow.AddHours(2)
            };


            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", restaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Email");
            return View(opinion);
        }

        // POST: Opinions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RestaurantId,UserId,Rating,Description,Date")] Opinion opinion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opinion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", opinion.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", opinion.UserId);
            return RedirectToAction("Search", "Restaurants");
        }

        // GET: Opinions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion.FindAsync(id);
            if (opinion == null)
            {
                return NotFound();
            }
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", opinion.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", opinion.UserId);
            return View(opinion);
        }

        // POST: Opinions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,RestaurantId,UserId,Rating,Description")] Opinion opinion)
        {
            if (id != opinion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opinion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpinionExists(opinion.Id))
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
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", opinion.RestaurantId);
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Id", opinion.UserId);
            return View(opinion);
        }

        // GET: Opinions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opinion = await _context.Opinion
                .Include(o => o.Restaurant)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opinion == null)
            {
                return NotFound();
            }

            return View(opinion);
        }

        // POST: Opinions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var opinion = await _context.Opinion.FindAsync(id);
            if (opinion != null)
            {
                _context.Opinion.Remove(opinion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpinionExists(string id)
        {
            return _context.Opinion.Any(e => e.Id == id);
        }
    }
}
