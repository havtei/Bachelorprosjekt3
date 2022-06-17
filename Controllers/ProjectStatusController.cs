#nullable disable
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
    [Authorize(Roles = "Admin")]
    public class ProjectStatusController : Controller
    {
        private readonly BachelorprosjektContext _context;

        public ProjectStatusController(BachelorprosjektContext context)
        {
            _context = context;
        }

        // GET: ProjectStatus
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectStatus.ToListAsync());
        }

        // GET: ProjectStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStatus == null)
            {
                return NotFound();
            }

            return View(projectStatus);
        }

        // GET: ProjectStatus/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: ProjectStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status")] ProjectStatus projectStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectStatus);
        }

        // GET: ProjectStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus.FindAsync(id);
            if (projectStatus == null)
            {
                return NotFound();
            }
            return View(projectStatus);
        }

        // POST: ProjectStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] ProjectStatus projectStatus)
        {
            if (id != projectStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectStatusExists(projectStatus.Id))
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
            return View(projectStatus);
        }

        // GET: ProjectStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectStatus = await _context.ProjectStatus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectStatus == null)
            {
                return NotFound();
            }

            return View(projectStatus);
        }

        // POST: ProjectStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectStatus = await _context.ProjectStatus.FindAsync(id);
            _context.ProjectStatus.Remove(projectStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectStatusExists(int id)
        {
            return _context.ProjectStatus.Any(e => e.Id == id);
        }
    }
}
