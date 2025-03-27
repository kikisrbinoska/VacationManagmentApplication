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
    public class MedicalCaresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicalCaresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MedicalCares
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MedicalCares.Include(m => m.Employee).Include(m => m.HR);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> RequestMedicalCare()
        {
            var applicationDbContext = _context.MedicalCares.Include(m => m.Employee).Include(m => m.HR);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MedicalCares/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCare = await _context.MedicalCares
                .Include(m => m.Employee)
                .Include(m => m.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalCare == null)
            {
                return NotFound();
            }

            return View(medicalCare);
        }

        // GET: MedicalCares/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id");
            return View();
        }

        // POST: MedicalCares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,Reason,InvoicePath,EmployeeId,HRId")] MedicalCare medicalCare)
        {
            if (ModelState.IsValid)
            {
                medicalCare.Id = Guid.NewGuid();
                _context.Add(medicalCare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", medicalCare.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", medicalCare.HRId);
            return View(medicalCare);
        }
        // GET: MedicalCares/AddNew
        public IActionResult AddNew(Guid employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return View();
        }


        // POST: MedicalCares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew([Bind("Id,StartDate,EndDate,Reason,EmployeeId,HRId")] MedicalCare medicalCare, IFormFile invoiceFile)
        {
            if (ModelState.IsValid)
            {

                if (medicalCare.StartDate > medicalCare.EndDate)
                {
                    ModelState.AddModelError("", "The start date cannot be after the end date.");
                    return View(medicalCare);
                }


                int requestedDays = (medicalCare.EndDate - medicalCare.StartDate).Days + 1;

                if (requestedDays < 1)
                {
                    ModelState.AddModelError("", "The date range must be at least one day.");
                    return View(medicalCare);
                }


                if (medicalCare.StartDate.Year != DateTime.Now.Year || medicalCare.EndDate.Year != DateTime.Now.Year)
                {
                    ModelState.AddModelError("", "Medical care days must be used in the current year.");
                    return View(medicalCare);
                }


                if (medicalCare.EndDate > new DateTime(DateTime.Now.Year, 12, 31))
                {
                    ModelState.AddModelError("", "Medical care cannot be taken after 31.12.");
                    return View(medicalCare);
                }

                if (invoiceFile != null && invoiceFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(invoiceFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid file type. Only PDF, JPG, and PNG are allowed.");
                        return View(medicalCare);
                    }

                    var fileName = Path.GetFileName(invoiceFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await invoiceFile.CopyToAsync(stream);
                    }

                    medicalCare.InvoicePath = fileName;
                }

                _context.Add(medicalCare);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(RequestMedicalCare));
            }

            ViewBag.EmployeeId = medicalCare.EmployeeId;
            ViewBag.HRId = medicalCare.HRId;
            return View(medicalCare);
        }
        // GET: MedicalCares/AddNew
        public IActionResult AddNewMedicalCare(Guid HRId)
        {
            ViewBag.HRId = HRId;
            return View();
        }


        // POST: MedicalCares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNewMedicalCare([Bind("Id,StartDate,EndDate,Reason,EmployeeId,HRId")] MedicalCare medicalCare, IFormFile invoiceFile)
        {
            if (ModelState.IsValid)
            {

                if (medicalCare.StartDate > medicalCare.EndDate)
                {
                    ModelState.AddModelError("", "The start date cannot be after the end date.");
                    return View(medicalCare);
                }


                int requestedDays = (medicalCare.EndDate - medicalCare.StartDate).Days + 1;

                if (requestedDays < 1)
                {
                    ModelState.AddModelError("", "The date range must be at least one day.");
                    return View(medicalCare);
                }


                if (medicalCare.StartDate.Year != DateTime.Now.Year || medicalCare.EndDate.Year != DateTime.Now.Year)
                {
                    ModelState.AddModelError("", "Medical care days must be used in the current year.");
                    return View(medicalCare);
                }


                if (medicalCare.EndDate > new DateTime(DateTime.Now.Year, 12, 31))
                {
                    ModelState.AddModelError("", "Medical care cannot be taken after 31.12.");
                    return View(medicalCare);
                }

                if (invoiceFile != null && invoiceFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(invoiceFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Invalid file type. Only PDF, JPG, and PNG are allowed.");
                        return View(medicalCare);
                    }

                    var fileName = Path.GetFileName(invoiceFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await invoiceFile.CopyToAsync(stream);
                    }

                    medicalCare.InvoicePath = fileName;
                }

                _context.Add(medicalCare);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = medicalCare.EmployeeId;
            ViewBag.HRId = medicalCare.HRId;
            return View(medicalCare);
        }
        // GET: MedicalCares/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCare = await _context.MedicalCares.FindAsync(id);
            if (medicalCare == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", medicalCare.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", medicalCare.HRId);
            return View(medicalCare);
        }

        // POST: MedicalCares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,StartDate,EndDate,Reason,InvoicePath,EmployeeId,HRId")] MedicalCare medicalCare)
        {
            if (id != medicalCare.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalCare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalCareExists(medicalCare.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", medicalCare.EmployeeId);
            ViewData["HRId"] = new SelectList(_context.HR, "Id", "Id", medicalCare.HRId);
            return View(medicalCare);
        }

        // GET: MedicalCares/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalCare = await _context.MedicalCares
                .Include(m => m.Employee)
                .Include(m => m.HR)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicalCare == null)
            {
                return NotFound();
            }

            return View(medicalCare);
        }

        // POST: MedicalCares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var medicalCare = await _context.MedicalCares.FindAsync(id);
            if (medicalCare != null)
            {
                _context.MedicalCares.Remove(medicalCare);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalCareExists(Guid id)
        {
            return _context.MedicalCares.Any(e => e.Id == id);
        }
    }
}
