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
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        [Authorize]
        public async Task<IActionResult> Index(GenreType? genreFilter = null, string searchTerm = null, string sortOrder = "asc")
        {
            IQueryable<Movie> moviesQuery = _context.Movie;

            if (genreFilter.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.Genre == genreFilter.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(searchTerm));
            }

            if (sortOrder == "desc")
            {
                moviesQuery = moviesQuery.OrderByDescending(m => m.ReleaseDate);
            }
            else
            {
                moviesQuery = moviesQuery.OrderBy(m => m.ReleaseDate);
            }
            ViewBag.TotalMoviesCount = await moviesQuery.CountAsync();

            if (ViewBag.TotalMoviesCount > 0)
            {
                ViewBag.AverageMovieDuration = await moviesQuery.AverageAsync(m => m.Duration);
            }

            return _context.Movie != null ?
                   View(await moviesQuery.ToListAsync()) :
                   Problem("Entity set 'OneClickTicketsContext.Movie' is null.");
        }


        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            // Populate genres
            ViewData["Genres"] = Enum.GetValues(typeof(GenreType))
                                     .Cast<GenreType>()
                                     .Select(g => new SelectListItem
                                     {
                                         Text = g.ToString(),
                                         Value = g.ToString()
                                     }).ToList();

            return View();
        }


        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Duration,Genre,Director,ReleaseDate")] Movie movie)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                ModelState.AddModelError("Title", "Title is required.");
                isValid = false;
            }

            if (movie.Duration <= 0 || movie.Duration > 300)
            {
                ModelState.AddModelError("Duration", "Duration must be a positive number less than 300.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(movie.Director))
            {
                ModelState.AddModelError("Director", "Director is required.");
                isValid = false;
            }

            if (movie.ReleaseDate <= DateTime.MinValue || movie.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Invalid Release Date.");
                isValid = false;
            }

            if (isValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }


        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewData["Genres"] = Enum.GetValues(typeof(GenreType))
                                     .Cast<GenreType>()
                                     .Select(g => new SelectListItem
                                     {
                                         Text = g.ToString(),
                                         Value = g.ToString(),
                                         Selected = (g == movie.Genre)
                                     }).ToList();

            return View(movie);
        }


        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Duration,Genre,Director,ReleaseDate")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(movie.Title))
            {
                ModelState.AddModelError("Title", "Title is required.");
                isValid = false;
            }

            if (movie.Duration <= 0 || movie.Duration > 300)
            {
                ModelState.AddModelError("Duration", "Duration must be a positive number less than 300.");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(movie.Director))
            {
                ModelState.AddModelError("Director", "Director is required.");
                isValid = false;
            }

            if (movie.ReleaseDate <= DateTime.MinValue || movie.ReleaseDate > DateTime.Now)
            {
                ModelState.AddModelError("ReleaseDate", "Invalid Release Date.");
                isValid = false;
            }

            if (isValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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

            return View(movie);
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'OneClickTicketsContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return (_context.Movie?.Any(e => e.MovieId == id)).GetValueOrDefault();
        }
    }
}
