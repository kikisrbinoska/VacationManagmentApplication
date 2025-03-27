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
    public class BonusDaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BonusDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BonusDays
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BonusDays.Include(b => b.Employee).Include(b => b.HR);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> ViewBonusRequests()
        {
            var applicationDbContext = _context.BonusDays.Include(b => b.Employee).Include(b => b.HR);
            return View(await applicationDbContext.ToListAsync());
        }
       

        // GET: BonusDays/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusDays = await _context.BonusDays
                .Include(b => b.Employee)
                .Include(b => b.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bonusDays == null)
            {
                return NotFound();
            }

            return View(bonusDays);
        }


        // GET: BonusDays/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id");
            return View();
        }

        // POST: BonusDays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] BonusDays bonusDays)
        {
            if (ModelState.IsValid)
            {
                bonusDays.Id = Guid.NewGuid();
                _context.Add(bonusDays);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", bonusDays.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", bonusDays.HRId);
            return View(bonusDays);
        }
        // GET: BonusDays/AddNew
        public IActionResult AddNew(Guid employeeId)
        {
            ViewBag.EmployeeId = employeeId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] BonusDays bonusDays)
        {
            if (bonusDays == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return View(bonusDays);
            }

            if (bonusDays.StartDate > bonusDays.EndDate)
            {
                ModelState.AddModelError("", "The start date cannot be after the end date.");
                return View(bonusDays);
            }

            int requestedDays = (bonusDays.EndDate - bonusDays.StartDate).Days + 1;

            if (bonusDays.StartDate.Year != DateTime.Now.Year || bonusDays.EndDate.Year != DateTime.Now.Year)
            {
                ModelState.AddModelError("", "Bonus days must be used within the current year.");
                return View(bonusDays);
            }

            if (bonusDays.EndDate > new DateTime(DateTime.Now.Year, 12, 31))
            {
                ModelState.AddModelError("", "Bonus days cannot be taken after December 31st.");
                return View(bonusDays);
            }

            
            var employee = await _context.Employees.FindAsync(bonusDays.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("", "Employee not found.");
                return View(bonusDays);
            }

            if (requestedDays > employee.GetRemainingBonusDays())
            {
                ModelState.AddModelError("", "You do not have enough bonus days for this request.");
                return View(bonusDays);
            }

            if (ModelState.IsValid)
            {
                _context.Add(bonusDays);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewBonusRequests");
            }

            ViewBag.EmployeeId = bonusDays.EmployeeId;
            ViewBag.HRId = bonusDays.HRId;
            return View(bonusDays);
        }
        public IActionResult AddNewBonus(Guid HRId)
        {
            ViewBag.HRId = HRId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewBonus([Bind("Id,StartDate,EndDate,EmployeeId,HRId")] BonusDays bonusDays)
        {
            if (bonusDays == null)
            {
                ModelState.AddModelError("", "Invalid data submitted.");
                return View(bonusDays);
            }

            if (bonusDays.StartDate > bonusDays.EndDate)
            {
                ModelState.AddModelError("", "The start date cannot be after the end date.");
                return View(bonusDays);
            }

            int requestedDays = (bonusDays.EndDate - bonusDays.StartDate).Days + 1;

            if (bonusDays.StartDate.Year != DateTime.Now.Year || bonusDays.EndDate.Year != DateTime.Now.Year)
            {
                ModelState.AddModelError("", "Bonus days must be used within the current year.");
                return View(bonusDays);
            }

            if (bonusDays.EndDate > new DateTime(DateTime.Now.Year, 12, 31))
            {
                ModelState.AddModelError("", "Bonus days cannot be taken after December 31st.");
                return View(bonusDays);
            }


            var hr = await _context.HR.FindAsync(bonusDays.HRId);
            if (hr == null)
            {
                ModelState.AddModelError("", "HR not found.");
                return View(bonusDays);
            }

            if (requestedDays > hr.GetRemainingBonusDays())
            {
                ModelState.AddModelError("", "You do not have enough bonus days for this request.");
                return View(bonusDays);
            }

            if (ModelState.IsValid)
            {
                _context.Add(bonusDays);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = bonusDays.EmployeeId;
            ViewBag.HRId = bonusDays.HRId;
            return View(bonusDays);
        }



        // GET: BonusDays/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusDays = await _context.BonusDays.FindAsync(id);
            if (bonusDays == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", bonusDays.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", bonusDays.HRId);
            return View(bonusDays);
        }

        // POST: BonusDays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,StartDate,EndDate,EmployeeId,HRId")] BonusDays bonusDays)
        {
            if (id != bonusDays.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bonusDays);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BonusDaysExists(bonusDays.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", bonusDays.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", bonusDays.HRId);
            return View(bonusDays);
        }

        // GET: BonusDays/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bonusDays = await _context.BonusDays
                .Include(b => b.Employee)
                .Include(b => b.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bonusDays == null)
            {
                return NotFound();
            }

            return View(bonusDays);
        }

        // POST: BonusDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bonusDays = await _context.BonusDays.FindAsync(id);
            if (bonusDays != null)
            {
                _context.BonusDays.Remove(bonusDays);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BonusDaysExists(Guid id)
        {
            return _context.BonusDays.Any(e => e.Id == id);
        }
    }
}
