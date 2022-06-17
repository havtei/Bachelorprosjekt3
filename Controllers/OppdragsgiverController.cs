using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bachelorprosjekt.Data;
using Bachelorprosjekt.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bachelorprosjekt.Controllers
{
    [Authorize(Roles = "Admin,Fagansvarlig")]
    public class OppdragsgiverController : Controller
    {
        private readonly BachelorprosjektContext _context;

        public OppdragsgiverController(BachelorprosjektContext context)
        {
            _context = context;
        }

        // GET: Oppdragsgiver
        [Authorize(Roles = "Fagansvarlig")]
        public async Task<IActionResult> Index()
        {
              return _context.Oppdragsgiver != null ? 
                          View(await _context.Oppdragsgiver.ToListAsync()) :
                          Problem("Entity set 'BachelorprosjektContext.Oppdragsgiver'  is null.");
        }

        [Authorize(Roles = "Admin")]
        // GET: Oppdragsgiver/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Oppdragsgiver/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Description")] Oppdragsgiver oppdragsgiver)
        {

            oppdragsgiver.IsActive = true;
            oppdragsgiver.AntInnmeldteProsjekt = 0;
            if (ModelState.IsValid)
            {
                _context.Add(oppdragsgiver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oppdragsgiver);
        }

        // GET: Oppdragsgiver/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Oppdragsgiver == null)
            {
                return NotFound();
            }

            var oppdragsgiver = await _context.Oppdragsgiver.FindAsync(id);
            if (oppdragsgiver == null)
            {
                return NotFound();
            }
            return View(oppdragsgiver);
        }

        // POST: Oppdragsgiver/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,Description,IsActive,AntInnmeldteProsjekt")] Oppdragsgiver oppdragsgiver)
        {
            if (id != oppdragsgiver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oppdragsgiver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OppdragsgiverExists(oppdragsgiver.Id))
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
            return View(oppdragsgiver);
        }

        private bool OppdragsgiverExists(string id)
        {
          return (_context.Oppdragsgiver?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
