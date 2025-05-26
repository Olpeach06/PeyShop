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
    public class ZakazsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZakazsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zakazs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zakazy.ToListAsync());
        }

        // GET: Zakazs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazy
                .FirstOrDefaultAsync(m => m.ZakazId == id);
            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }

        // GET: Zakazs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zakazs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZakazId,Date")] Zakaz zakaz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zakaz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zakaz);
        }

        // GET: Zakazs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazy.FindAsync(id);
            if (zakaz == null)
            {
                return NotFound();
            }
            return View(zakaz);
        }

        // POST: Zakazs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZakazId,Date")] Zakaz zakaz)
        {
            if (id != zakaz.ZakazId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zakaz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZakazExists(zakaz.ZakazId))
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
            return View(zakaz);
        }

        // GET: Zakazs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakaz = await _context.Zakazy
                .FirstOrDefaultAsync(m => m.ZakazId == id);
            if (zakaz == null)
            {
                return NotFound();
            }

            return View(zakaz);
        }

        // POST: Zakazs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zakaz = await _context.Zakazy.FindAsync(id);
            if (zakaz != null)
            {
                _context.Zakazy.Remove(zakaz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZakazExists(int id)
        {
            return _context.Zakazy.Any(e => e.ZakazId == id);
        }
    }
}
