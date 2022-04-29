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
    public class VariantsController : Controller
    {
        private readonly VirusBaseContext _context;
        private Variant vr = new Variant();
        public VariantsController(VirusBaseContext context) {
            _context = context;
        }

        // GET: Variants
        public async Task<IActionResult> Index()
        {

            return View(await _context.Variants.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variant = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variant == null)
                return NotFound();

            return View(variant);
            //return RedirectToAction("Index", "Viruses", new { id = variant.Id, name = variant.VariantName });
        }

        // GET: /Create
        public IActionResult Create() {

            return View();
        }

        // POST: /Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VariantName,VariantOrigin,VariantDateDiscovered, VirusId")] Variant variant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(variant);
                vr = variant;
                await _context.SaveChangesAsync();
                //await Adder(variant.Id, variant);
                //return RedirectToAction("Index", "CountriesVariants");
                return RedirectToAction(nameof(Index));
            }
            return View(variant);
        }

        public async Task<IActionResult> ListDetails(int? id)
        {
            if (id is null)
            {
                return RedirectToPage("Index", "Variants");
            }

            var variant = await _context.Variants.FirstOrDefaultAsync(a => a.Id == id);
            if (variant is null)
            {
                return RedirectToPage("Index", "Variants");
            }

            var x = await _context.CountriesVariants.Where(c => c.VariantId == id).ToListAsync();
            //if (playsongs.Count == 0) return View(await _context.Songs.Where(a => a.Id <= 5).ToListAsync());

            List<int> countriesIds = new();
            List<string> timeAdded = new();
            foreach (var g in x)
            {
                countriesIds.Add(g.CountryId);
                //timeAdded.Add(g.TimeSongAdded.Date.ToShortDateString());
            }

            var countries = await _context.Countries
                .Where(c => countriesIds.Contains(c.Id)).ToListAsync();

            ViewBag.VariantId = id;
            ViewBag.countries = countries;
            ViewBag.VariantName = variant.VariantName;
            //ViewBag.TimeAdded = timeAdded;
            //ViewBag.iteratorTime = 0;
            //ViewBag.LinkToImage = playlist.PhotoLink;
            return View(countries);
        }


        // GET: Variants/Edit/5
        public async Task<IActionResult> Edit(int? id, int? virusId)
        {
            if (id == null)
                return NotFound();

            var variant = await _context.Variants.FindAsync(id);
            variant.Virus = await _context.Viruses.FindAsync(variant.VirusId);
            ViewBag.VirusId = variant.VirusId;
            if (virusId != null) {
                ViewBag.VirusName = _context.Viruses.Where(f => f.Id == variant.VirusId).FirstOrDefault().VirusName;
                ViewBag.VirusId = virusId;
            }
            else {
                ViewBag.VirusName = null;
                ViewData["VirusId"] = new SelectList(_context.Viruses, "Id", "VirusName", virusId);
            }
            if (variant == null) {
                return NotFound();
            }
            return View(variant);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VariantName,VariantOrigin,VariantDateDiscovered,VirusId")] Variant variant) {
            if (id != variant.Id)
                return NotFound();
            variant.Virus = await _context.Viruses.FindAsync(variant.VirusId);
            ModelState.ClearValidationState(nameof(variant.Virus));
            TryValidateModel(variant.Virus, nameof(variant.Virus));
            if (ModelState.IsValid) {
                try {
                    _context.Update(variant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!VariantExists(variant.Id))
                        return NotFound();
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VirusId"] = new SelectList(_context.Viruses, "Id", "VirusName", variant.VirusId);
            return View(variant);
            //if (ModelState.IsValid)
            //{
            //    try {
            //        _context.Update(variant);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ArtistExists(variant.Id))
            //            return NotFound();
            //        else {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(variant);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Variants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null) {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variant = await _context.Variants.FindAsync(id);
            _context.Variants.Remove(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VariantExists(int id)
        {
            return _context.Variants.Any(e => e.Id == id);
        }
    }
}