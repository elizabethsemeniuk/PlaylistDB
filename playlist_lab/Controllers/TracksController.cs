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
    public class TracksController : Controller
    {
        private readonly DBPlaylistContext _context;

        public TracksController(DBPlaylistContext context)
        {
            _context = context;
        }

        // GET: Tracks
        public async Task<IActionResult> Index()
        {
           // if (id == null) return RedirectToAction("Jenres", "Index");
            // search tracks by jenres
            //ViewBag.JenresId = id;
            //ViewBag.JenresName = name;
           // var tracksByJenres = _context.Tracks.Where(b => b.JenreId == id).Include(b => b.Jenre).Include(t => t.Album);
            var DBPlaylistContext = _context.Tracks.Include(t => t.Album).Include(t => t.Jenre);
            return View(await DBPlaylistContext.ToListAsync());
        }
            // GET: Tracks/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracks = await _context.Tracks
                .Include(t => t.Album)
                .Include(t => t.Jenre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tracks == null)
            {
                return NotFound();
            }

            return View(tracks);
        }

        // GET: Tracks/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name");
            ViewData["JenreId"] = new SelectList(_context.Jenres, "Id", "Name");
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JenreId,AlbumId,Name")] Tracks tracks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tracks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", tracks.AlbumId);
            ViewData["JenreId"] = new SelectList(_context.Jenres, "Id", "Name", tracks.JenreId);
            return View(tracks);
        }

        // GET: Tracks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracks = await _context.Tracks.FindAsync(id);
            if (tracks == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", tracks.AlbumId);
            ViewData["JenreId"] = new SelectList(_context.Jenres, "Id", "Name", tracks.JenreId);
            return View(tracks);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JenreId,AlbumId,Name")] Tracks tracks)
        {
            if (id != tracks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tracks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TracksExists(tracks.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Name", tracks.AlbumId);
            ViewData["JenreId"] = new SelectList(_context.Jenres, "Id", "Name", tracks.JenreId);
            return View(tracks);
        }

        // GET: Tracks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tracks = await _context.Tracks
                .Include(t => t.Album)
                .Include(t => t.Jenre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tracks == null)
            {
                return NotFound();
            }

            return View(tracks);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tracks = await _context.Tracks.FindAsync(id);
            _context.Tracks.Remove(tracks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TracksExists(int id)
        {
            return _context.Tracks.Any(e => e.Id == id);
        }
    }
}
