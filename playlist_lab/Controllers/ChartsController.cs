using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace playlist_lab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBPlaylistContext _context;
        public ChartsController(DBPlaylistContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var jenres = _context.Jenres.Include(b => b.Tracks).ToList();
            List<object> jenTracks = new List<object>();
            jenTracks.Add(new[] { "Jenre", "Tracks quantity" });
            foreach (var j in jenres)
            {
                jenTracks.Add(new object[] { j.Name, j.Tracks.Count() });

            }
            return new JsonResult(jenTracks);
        }
    }
}