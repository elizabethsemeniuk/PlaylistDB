using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using playlist_lab;
using System.IO;
using ClosedXML.Excel;

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
            //return RedirectToAction("Index", "Tracks", new { id = jenres.Id, name = jenres.Name });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо
                                //відсутня, то створюємо нову
                                Jenres newjen;
                                var j = (from jen in _context.Jenres
                                         where jen.Name.Contains(worksheet.Name)
                                         select jen).ToList();
                                if (j.Count > 0)
                                {
                                    newjen = j[0];
                                }

                                     else
                                {
                                    newjen = new Jenres();

                                    newjen.Name = worksheet.Name;
                                 //   newjen.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Jenres.Add(newjen);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Tracks track = new Tracks();
                                        track.Name = row.Cell(1).Value.ToString();
                                       // track.Info = row.Cell(6).Value.ToString();
                                        track.Jenre = newjen;
                                        _context.Tracks.Add(track);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Artists artist;
                                                var a = (from art in _context.Artists
                                                         where art.Fullname.Contains(row.Cell(i).Value.ToString())
                                                         select art).ToList();
                                                if (a.Count > 0)
                                                {
                                                    artist = a[0];
                                                }
                                                 else
                                                {
                                                    artist = new Artists();
                                                    artist.Fullname = row.Cell(i).Value.ToString();
                                                    artist.Country = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(artist);
                                                }
                                                ArtistsTracks at = new ArtistsTracks();
                                                at.Track = track;
                                                at.Artist = artist;
                                                _context.ArtistsTracks.Add(at);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
                                    }
                                }
                            }

                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var jenres = _context.Jenres.Include("Tracks").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ
               // (писати лише вибрані)
                foreach (var j in jenres)
                {
                    var worksheet = workbook.Worksheets.Add(j.Name);
                    worksheet.Cell("A1").Value = "id";
                    worksheet.Cell("B1").Value = "name";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var tracks = j.Tracks.ToList();
                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < tracks.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = tracks[i].Name;
                        worksheet.Cell(i + 2, 7).Value = tracks[i].Id;
                        var ab = _context.ArtistsTracks.Where(a => a.TrackId == tracks[i].Id).Include("Artist").ToList();

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                        FileDownloadName = $"library_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}
