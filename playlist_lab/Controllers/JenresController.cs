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
    public class JenresController : Controller
    {
        private readonly DBPlaylistContext _context;

        public JenresController(DBPlaylistContext context)
        {
            _context = context;
        }

        // GET: Jenres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jenres.ToListAsync());
        }

        // GET: Jenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenres = await _context.Jenres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jenres == null)
            {
                return NotFound();
            }

             return View(jenres);
        }

        // GET: Jenres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Jenres jenres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jenres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jenres);
        }

        // GET: Jenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenres = await _context.Jenres.FindAsync(id);
            if (jenres == null)
            {
                return NotFound();
            }
            return View(jenres);
        }

        // POST: Jenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Jenres jenres)
        {
            if (id != jenres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jenres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JenresExists(jenres.Id))
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
            return View(jenres);
        }

        // GET: Jenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jenres = await _context.Jenres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jenres == null)
            {
                return NotFound();
            }

            return View(jenres);
        }

        // POST: Jenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jenres = await _context.Jenres.FindAsync(id);
            _context.Jenres.Remove(jenres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JenresExists(int id)
        {
            return _context.Jenres.Any(e => e.Id == id);
        }
    }
}
