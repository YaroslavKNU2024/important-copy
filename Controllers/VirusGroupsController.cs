#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VirusAppFinal;

namespace VirusAppFinal.Controllers
{
    public class VirusGroupsController : Controller
    {
        private readonly VirusBaseContext _context;

        public VirusGroupsController(VirusBaseContext context) {
            _context = context;
        }

        // GET: viruses
        public async Task<IActionResult> Index()
        {
            return View(await _context.VirusGroups.ToListAsync());
        }

        // GET: viruses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var virusGroup = await _context.VirusGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (virusGroup == null)
                return NotFound();
            return RedirectToAction("Index", "Viruses", new {id = virusGroup.Id, name = virusGroup.GroupName});
            //return View(virus);
        }

        // GET: viruses/Createx
        public IActionResult Create() {
            return View();
        }

        // POST: viruses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, GroupName, GroupInfo, DateDiscovered")] VirusGroup group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // GET: viruses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var group = await _context.VirusGroups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, GroupName, GroupInfo, DateDiscovered")] VirusGroup group)
        {
            if (id != group.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(group.Id))
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
            return View(group);
        }

        // GET: viruses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.VirusGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: viruses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _context.VirusGroups.FindAsync(id);
            var albums = _context.Viruses.Where(c => c.GroupId == id);
            /*if (albums.Any())
            {
                foreach (var album in albums)
                {
                    var albumId = album.Id;
                    var songs = _context.Songs.Where(c => c.AlbumId == albumId);
                    foreach (var song in songs)
                    {
                        var songId = song.Id;
                        var artistssongs = _context.ArtistsSongs.Where(c => c.SongId == songId);
                        foreach (var artsong in artistssongs)
                        {
                            _context.Remove(artsong);
                        }
                        var playlistsongs = _context.PlaylistsSongs.Where(c => c.SongId == songId);
                        foreach (var playsong in playlistsongs)
                        {
                            _context.Remove(playsong);
                        }
                        _context.Remove(song);
                    }
                    _context.Albums.Remove(album);
                }
            }*/
            _context.VirusGroups.Remove(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return _context.VirusGroups.Any(e => e.Id == id);
        }
    }
}
