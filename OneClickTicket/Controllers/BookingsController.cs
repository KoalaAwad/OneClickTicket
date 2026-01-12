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
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var oneClickTicketsContext = _context.Booking.Include(b => b.Cinema).Include(b => b.Movie);
            return View(await oneClickTicketsContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Cinema)
                .Include(b => b.Movie)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Set<Cinema>(), "CinemaId", "Location");
            ViewData["MovieId"] = new SelectList(_context.Set<Movie>(), "MovieId", "Director");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,CinemaId,MovieId,BookingTime,NumberOfSeats")] Booking booking)
        {
            bool isValid = true;

            if (booking.CinemaId <= 0)
            {
                ModelState.AddModelError("CinemaId", "A valid cinema selection is required.");
                isValid = false;
            }

            if (booking.MovieId <= 0)
            {
                ModelState.AddModelError("MovieId", "A valid movie selection is required.");
                isValid = false;
            }

            if (booking.BookingTime <= DateTime.Now)
            {
                ModelState.AddModelError("BookingTime", "The booking time cannot be in the past.");
                isValid = false;
            }

            if (booking.NumberOfSeats <= 0)
            {
                ModelState.AddModelError("NumberOfSeats", "You must book at least one seat.");
                isValid = false;
            }

            if (isValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CinemaId"] = new SelectList(_context.Set<Cinema>(), "CinemaId", "Location", booking.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Set<Movie>(), "MovieId", "Director", booking.MovieId);
            return View(booking);
        }


        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CinemaId"] = new SelectList(_context.Set<Cinema>(), "CinemaId", "Location", booking.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Set<Movie>(), "MovieId", "Director", booking.MovieId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,CinemaId,MovieId,BookingTime,NumberOfSeats")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            bool isValid = true;

            if (booking.CinemaId <= 0)
            {
                ModelState.AddModelError("CinemaId", "Valid Cinema is required.");
                isValid = false;
            }

            if (booking.MovieId <= 0)
            {
                ModelState.AddModelError("MovieId", "Valid Movie is required.");
                isValid = false;
            }

            if (booking.BookingTime < DateTime.Now)
            {
                ModelState.AddModelError("BookingTime", "Booking time cannot be in the past.");
                isValid = false;
            }

            if (booking.NumberOfSeats <= 0)
            {
                ModelState.AddModelError("NumberOfSeats", "At least one seat must be booked.");
                isValid = false;
            }

            if (isValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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

            ViewData["CinemaId"] = new SelectList(_context.Set<Cinema>(), "CinemaId", "Location", booking.CinemaId);
            ViewData["MovieId"] = new SelectList(_context.Set<Movie>(), "MovieId", "Director", booking.MovieId);
            return View(booking);
        }


        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Booking == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Cinema)
                .Include(b => b.Movie)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Booking == null)
            {
                return Problem("Entity set 'OneClickTicketsContext.Booking'  is null.");
            }
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return (_context.Booking?.Any(e => e.BookingId == id)).GetValueOrDefault();
        }
    }
}
