using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OneClickTicket.Data;
using OneClickTicket.Models;

namespace OneClickTicket.Controllers
{
    public class CinemasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CinemasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cinemas
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return _context.Cinema != null ?
                        View(await _context.Cinema.ToListAsync()) :
                        Problem("Entity set 'OneClickTicketsContext.Cinema'  is null.");
        }

        // GET: Cinemas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cinema == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinema
                .FirstOrDefaultAsync(m => m.CinemaId == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        // GET: Cinemas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cinemas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CinemaId,Name,Location,NumberOfHalls,Website,ContactNumber")] Cinema cinema)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(cinema.Location))
            {
                ModelState.AddModelError("Location", "Location is required.");
                isValid = false;
            }

            if (cinema.NumberOfHalls <= 0)
            {
                ModelState.AddModelError("NumberOfHalls", "Number of Halls must be a positive number.");
                isValid = false;
            }


            if (isValid)
            {
                _context.Add(cinema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cinema);
        }


        // GET: Cinemas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cinema == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinema.FindAsync(id);
            if (cinema == null)
            {
                return NotFound();
            }
            return View(cinema);
        }

        // POST: Cinemas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CinemaId,Name,Location,NumberOfHalls,Website,ContactNumber")] Cinema cinema)
        {
            if (id != cinema.CinemaId)
            {
                return NotFound();
            }

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(cinema.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(cinema.Location))
            {
                ModelState.AddModelError("Location", "Location is required.");
                isValid = false;
            }

            if (cinema.NumberOfHalls <= 0)
            {
                ModelState.AddModelError("NumberOfHalls", "Number of Halls must be a positive number.");
                isValid = false;
            }

            if (isValid)
            {
                try
                {
                    _context.Update(cinema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaExists(cinema.CinemaId))
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

            return View(cinema);
        }


        // GET: Cinemas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cinema == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinema
                .FirstOrDefaultAsync(m => m.CinemaId == id);
            if (cinema == null)
            {
                return NotFound();
            }

            return View(cinema);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cinema == null)
            {
                return Problem("Entity set 'OneClickTicketsContext.Cinema'  is null.");
            }
            var cinema = await _context.Cinema.FindAsync(id);
            if (cinema != null)
            {
                _context.Cinema.Remove(cinema);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaExists(int id)
        {
            return (_context.Cinema?.Any(e => e.CinemaId == id)).GetValueOrDefault();
        }
    }
}
