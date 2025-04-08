﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodiesWorld.Data;
using FoodiesWorld.Models;

namespace FoodiesWorld.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurant.ToListAsync();

            var ratings = await _context.Opinion
                .GroupBy(o => o.RestaurantId)
                .Select(g => new {
                    RestaurantId = g.Key,
                    AverageRating = g.Average(o => o.Rating)
                }).ToListAsync();

            var model = restaurants.Select(r => new RestaurantWithRatings
            {
                Restaurant = r,
                AvgRating = ratings.FirstOrDefault(x => x.RestaurantId == r.Id)?.AverageRating
            }).ToList();

            return View(model);
        }

        // GET: Restaurants/Search
        public async Task<IActionResult> Search()
        {
            var restaurants = await _context.Restaurant.ToListAsync();

            var ratings = await _context.Opinion
                .GroupBy(o => o.RestaurantId)
                .Select(g => new {
                    RestaurantId = g.Key,
                    AverageRating = g.Average(o => o.Rating)
                }).ToListAsync();

            var model = restaurants.Select(r => new RestaurantWithRatings
            {
                Restaurant = r,
                AvgRating = ratings.FirstOrDefault(x => x.RestaurantId == r.Id)?.AverageRating
            }).ToList();

            return View(model);
        }

        // PoST: Restaurants/Search
        [HttpPost]
        public async Task<IActionResult> SearchResult(string Phrase)
        {
            var restaurants = await _context.Restaurant.Where(x => x.Name.Contains(Phrase)).ToListAsync();

            var ratings = await _context.Opinion
                .GroupBy(o => o.RestaurantId)
                .Select(g => new {
                    RestaurantId = g.Key,
                    AverageRating = g.Average(o => o.Rating)
                }).ToListAsync();

            var model = restaurants.Select(r => new RestaurantWithRatings
            {
                Restaurant = r,
                AvgRating = ratings.FirstOrDefault(x => x.RestaurantId == r.Id)?.AverageRating
            }).ToList();

            return View("Search", model);
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,City,Address")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,City,Address")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurant.Remove(restaurant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(string id)
        {
            return _context.Restaurant.Any(e => e.Id == id);
        }
    }
}
