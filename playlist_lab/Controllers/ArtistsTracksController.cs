using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using playlist_lab;

namespace playlist_lab.Controllers
{
    public class ArtistsTracksController : Controller
    {
        private readonly DBPlaylistContext _context;

        public ArtistsTracksController(DBPlaylistContext context)
        {
            _context = context;
        }

        // GET: ArtistsTracks
        public async Task<IActionResult> Index()
        {
            var dBPlaylistContext = _context.ArtistsTracks.Include(a => a.Track).Include(a => a.Artist);
            return View(await dBPlaylistContext.ToListAsync());
        }

        // GET: ArtistsTracks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsTracks = await _context.ArtistsTracks
                .Include(a => a.Artist)
                .Include(a => a.Track)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistsTracks == null)
            {
                return NotFound();
            }

            return View(artistsTracks);
        }

        // GET: ArtistsTracks/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Fullname");
            ViewData["TrackId"] = new SelectList(_context.Tracks, "Id", "Name");
            return View();
        }

        // POST: ArtistsTracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TrackId,ArtistId")] ArtistsTracks artistsTracks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistsTracks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Fullname", artistsTracks.ArtistId);
            ViewData["TrackId"] = new SelectList(_context.Tracks, "Id", "Name", artistsTracks.TrackId);
            return View(artistsTracks);
        }

        // GET: ArtistsTracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsTracks = await _context.ArtistsTracks.FindAsync(id);
            if (artistsTracks == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Fullname", artistsTracks.ArtistId);
            ViewData["TrackId"] = new SelectList(_context.Tracks, "Id", "Name", artistsTracks.TrackId);
            return View(artistsTracks);
        }

        // POST: ArtistsTracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TrackId,ArtistId")] ArtistsTracks artistsTracks)
        {
            if (id != artistsTracks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistsTracks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsTracksExists(artistsTracks.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Fullname", artistsTracks.ArtistId);
            ViewData["TrackId"] = new SelectList(_context.Tracks, "Id", "Name", artistsTracks.TrackId);
            return View(artistsTracks);
        }

        // GET: ArtistsTracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsTracks = await _context.ArtistsTracks
                .Include(a => a.Artist)
                .Include(a => a.Track)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistsTracks == null)
            {
                return NotFound();
            }

            return View(artistsTracks);
        }

        // POST: ArtistsTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistsTracks = await _context.ArtistsTracks.FindAsync(id);
            _context.ArtistsTracks.Remove(artistsTracks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistsTracksExists(int id)
        {
            return _context.ArtistsTracks.Any(e => e.Id == id);
        }
    }
}
