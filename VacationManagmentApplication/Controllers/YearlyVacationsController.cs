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
    public class YearlyVacationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YearlyVacationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: YearlyVacations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.YearlyVacations.Include(y => y.Employee).Include(y => y.HR);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> RequestYearlyVacation()
        {
            var applicationDbContext = _context.YearlyVacations.Include(y => y.Employee).Include(y => y.HR);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult AddNew(Guid employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] YearlyVacation yearlyVacation)
        {
            if (!ModelState.IsValid)
                return View(yearlyVacation);

            if (yearlyVacation.StartDate > yearlyVacation.EndDate)
            {
                ModelState.AddModelError("", "The start date cannot be after the end date.");
                return View(yearlyVacation);
            }

            int requestedDays = (yearlyVacation.EndDate - yearlyVacation.StartDate).Days + 1;
            if (requestedDays < 1)
            {
                ModelState.AddModelError("", "The date range must be at least one day.");
                return View(yearlyVacation);
            }

            _context.Add(yearlyVacation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RequestYearlyVacation));
        }
        public IActionResult AddNewVacation(Guid HRId)
        {
            ViewBag.HRId = HRId;
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewVacation([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] YearlyVacation yearlyVacation)
        {
            if (!ModelState.IsValid)
                return View(yearlyVacation);

            if (yearlyVacation.StartDate > yearlyVacation.EndDate)
            {
                ModelState.AddModelError("", "The start date cannot be after the end date.");
                return View(yearlyVacation);
            }

            int requestedDays = (yearlyVacation.EndDate - yearlyVacation.StartDate).Days + 1;
            if (requestedDays < 1)
            {
                ModelState.AddModelError("", "The date range must be at least one day.");
                return View(yearlyVacation);
            }

            _context.Add(yearlyVacation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: YearlyVacations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearlyVacation = await _context.YearlyVacations
                .Include(y => y.Employee)
                .Include(y => y.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearlyVacation == null)
            {
                return NotFound();
            }

            return View(yearlyVacation);
        }

        // GET: YearlyVacations/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id");
            return View();
        }

        // POST: YearlyVacations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] YearlyVacation yearlyVacation)
        {
            if (ModelState.IsValid)
            {
                yearlyVacation.Id = Guid.NewGuid();
                _context.Add(yearlyVacation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", yearlyVacation.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", yearlyVacation.HRId);
            return View(yearlyVacation);
        }

        // GET: YearlyVacations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearlyVacation = await _context.YearlyVacations.FindAsync(id);
            if (yearlyVacation == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", yearlyVacation.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", yearlyVacation.HRId);
            return View(yearlyVacation);
        }

        // POST: YearlyVacations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,StartDate,EndDate,EmployeeId,HRId")] YearlyVacation yearlyVacation)
        {
            if (id != yearlyVacation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yearlyVacation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearlyVacationExists(yearlyVacation.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", yearlyVacation.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", yearlyVacation.HRId);
            return View(yearlyVacation);
        }

        // GET: YearlyVacations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearlyVacation = await _context.YearlyVacations
                .Include(y => y.Employee)
                .Include(y => y.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearlyVacation == null)
            {
                return NotFound();
            }

            return View(yearlyVacation);
        }

        // POST: YearlyVacations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var yearlyVacation = await _context.YearlyVacations.FindAsync(id);
            if (yearlyVacation != null)
            {
                _context.YearlyVacations.Remove(yearlyVacation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearlyVacationExists(Guid id)
        {
            return _context.YearlyVacations.Any(e => e.Id == id);
        }
    }
}
