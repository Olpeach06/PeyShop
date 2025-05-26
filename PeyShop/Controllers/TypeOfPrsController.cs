using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyShop.Data;
using PeyShop.Models;

namespace PeyShop.Controllers
{
    public class TypeOfPrsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeOfPrsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TypeOfPrs
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOfPr.ToListAsync());
        }

        // GET: TypeOfPrs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPr = await _context.TypeOfPr
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (typeOfPr == null)
            {
                return NotFound();
            }

            return View(typeOfPr);
        }

        // GET: TypeOfPrs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfPrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,Name")] TypeOfPr typeOfPr)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfPr);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfPr);
        }

        // GET: TypeOfPrs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPr = await _context.TypeOfPr.FindAsync(id);
            if (typeOfPr == null)
            {
                return NotFound();
            }
            return View(typeOfPr);
        }

        // POST: TypeOfPrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,Name")] TypeOfPr typeOfPr)
        {
            if (id != typeOfPr.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfPr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfPrExists(typeOfPr.TypeId))
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
            return View(typeOfPr);
        }

        // GET: TypeOfPrs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfPr = await _context.TypeOfPr
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (typeOfPr == null)
            {
                return NotFound();
            }

            return View(typeOfPr);
        }

        // POST: TypeOfPrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfPr = await _context.TypeOfPr.FindAsync(id);
            if (typeOfPr != null)
            {
                _context.TypeOfPr.Remove(typeOfPr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfPrExists(int id)
        {
            return _context.TypeOfPr.Any(e => e.TypeId == id);
        }
    }
}
