using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyShop.Data;
using PeyShop.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace PeyShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Firm).Include(p => p.TypeOfPr);
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories; 

           return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Firm)
                .Include(p => p.TypeOfPr)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Ограничиваем доступ к созданию продуктов только администраторами
        [Authorize(Roles = "админ")] // пример для встроенных ролей
        public IActionResult Create()
        {
            CheckUserAccess(); // дополнительная проверка роли пользователя
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "Name");
            ViewData["TypeOfPrTypeId"] = new SelectList(_context.TypeOfPr, "TypeId", "Name");
            return View();
        }

        // POST: Products/Create
        // Для защиты от атак избыточной передачи данных включите конкретные свойства,
        // к которым хотите привязаться. Дополнительные сведения см.: https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "админ")] // ограничиваем создание продуктов только администраторами
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Quantity,Description,Image,CategoryId,TypeOfPrTypeId,FirmId")] Product product)
        {
            CheckUserAccess(); // дополнительная проверка роли пользователя
            if (!ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "Name", product.FirmId);
            ViewData["TypeOfPrTypeId"] = new SelectList(_context.TypeOfPr, "TypeId", "Name", product.TypeOfPrTypeId);
            return View(product);
        }

        // Ограничиваем доступ к редактированию товаров только администраторами
        [Authorize(Roles = "админ")] // ограничение на уровне атрибутов
        public async Task<IActionResult> Edit(int? id)
        {
            CheckUserAccess(); 
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "Name", product.FirmId);
            ViewData["TypeOfPrTypeId"] = new SelectList(_context.TypeOfPr, "TypeId", "Name", product.TypeOfPrTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // Чтобы защитить от атак чрезмерной отправки данных, разрешите привязывать к определенным свойствам.
        // Подробности см. здесь: https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "админ")] // разрешаем редактировать только администраторам
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Quantity,Description,Image,CategoryId,TypeOfPrTypeId,FirmId")] Product product)
        {
            CheckUserAccess(); 
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            ViewData["FirmId"] = new SelectList(_context.Firms, "FirmId", "Name", product.FirmId);
            ViewData["TypeOfPrTypeId"] = new SelectList(_context.TypeOfPr, "TypeId", "Name", product.TypeOfPrTypeId);
            return View(product);
        }

     
        [Authorize(Roles = "админ")] 
        public async Task<IActionResult> Delete(int? id)
        {
            CheckUserAccess(); 
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Firm)
                .Include(p => p.TypeOfPr)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "админ")] 
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CheckUserAccess(); 
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        /// <summary>
        /// Метод дополнительной проверки текущего пользователя.
        /// Проверяет роль пользователя и выводит уведомление, если роль неподходящая.
        /// </summary>
        private void CheckUserAccess()
        {
            var userRoleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.Equals(userRoleClaim, "2")) // предположим, что "2" означает обычного пользователя
            {
                TempData["ErrorMessage"] = "У вас нет прав доступа.";
                RedirectToAction(nameof(Index)); // переходим на индексную страницу товаров
            }
        }
    }
}