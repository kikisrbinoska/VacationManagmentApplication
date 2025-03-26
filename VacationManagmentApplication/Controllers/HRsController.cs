using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VacationManagmentApplication.Data;
using VacationManagmentApplication.Models;

namespace VacationManagmentApplication.Controllers
{
    public class HRsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HRsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HRs
        public async Task<IActionResult> Index()
        {
            return View(await _context.HR.ToListAsync());
        }

        // GET: HRs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hR = await _context.HR
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hR == null)
            {
                return NotFound();
            }

            return View(hR);
        }

        // GET: HRs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HRs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,PhoneNumber,Age")] HR hR)
        {
            if (ModelState.IsValid)
            {
                hR.Id = Guid.NewGuid();
                _context.Add(hR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hR);
        }

        // GET: HRs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hR = await _context.HR.FindAsync(id);
            if (hR == null)
            {
                return NotFound();
            }
            return View(hR);
        }

        // POST: HRs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Surname,PhoneNumber,Age")] HR hR)
        {
            if (id != hR.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HRExists(hR.Id))
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
            return View(hR);
        }

        // GET: HRs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hR = await _context.HR
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hR == null)
            {
                return NotFound();
            }

            return View(hR);
        }

        // POST: HRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var hR = await _context.HR.FindAsync(id);
            if (hR != null)
            {
                _context.HR.Remove(hR);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HRExists(Guid id)
        {
            return _context.HR.Any(e => e.Id == id);
        }
    }
}
