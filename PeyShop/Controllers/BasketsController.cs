using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyShop.Data;
using PeyShop.Models;

namespace PeyShop.Controllers
{
    public class BasketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Baskets
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Baskets.ToListAsync());
        //}
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Получаем ID текущего пользователя
            var baskets = await _context.Baskets
                .Include(b => b.Product) // Загружаем связанный Product
                .Where(b => b.UserId == userId) // Фильтруем по текущему пользователю
                .ToListAsync();

            return View(baskets);
        }

        // GET: Baskets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets
                .FirstOrDefaultAsync(m => m.BasketId == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // GET: Baskets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Baskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BasketId,Quantity")] Basket basket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(basket);
        }

        // GET: Baskets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets.FindAsync(id);
            if (basket == null)
            {
                return NotFound();
            }
            return View(basket);
        }

        // POST: Baskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BasketId,Quantity")] Basket basket)
        {
            if (id != basket.BasketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketExists(basket.BasketId))
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
            return View(basket);
        }

        // GET: Baskets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basket = await _context.Baskets
                .FirstOrDefaultAsync(m => m.BasketId == id);
            if (basket == null)
            {
                return NotFound();
            }

            return View(basket);
        }

        // POST: Baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);
            if (basket != null)
            {
                _context.Baskets.Remove(basket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketExists(int id)
        {
            return _context.Baskets.Any(e => e.BasketId == id);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToBasket(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.Quantity < quantity)
            {
                TempData["ErrorMessage"] = "Товар недоступен или недостаточно на складе";
                return RedirectToAction("Index", "Products");
            }
            // Получаем текущего пользователя
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Проверяем, есть ли уже такой товар в корзине пользователя
            var existingBasketItem = await _context.Baskets
                .FirstOrDefaultAsync(b => b.ProductId == productId && b.UserId == userId);

            if (existingBasketItem != null)
            {
                // Если товар уже есть в корзине, увеличиваем количество
                existingBasketItem.Quantity += quantity;
            }
            else
            {
                // Если товара нет в корзине, добавляем новый
                var basketItem = new Basket
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity
                };
                _context.Baskets.Add(basketItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(int id)
        {
            var basketItem = await _context.Baskets.FindAsync(id);
            if (basketItem != null)
            {
                _context.Baskets.Remove(basketItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Получаем все товары в корзине пользователя
            var basketItems = await _context.Baskets
                .Include(b => b.Product)
                .Where(b => b.UserId == userId)
                .ToListAsync();
            foreach (var item in basketItems)
            {
                if (item.Product.Quantity < item.Quantity)
                {
                    TempData["ErrorMessage"] = $"Товара '{item.Product.Name}' недостаточно на складе";
                    return RedirectToAction(nameof(Index));
                }
            }
            if (!basketItems.Any())
            {
                TempData["ErrorMessage"] = "Ваша корзина пуста";
                return RedirectToAction(nameof(Index));
            }

            // Создаем новый заказ
            var order = new Zakaz
            {
                UserId = userId,
                Date = DateTime.Now,
                StatusId = 1
            };
            _context.Zakazy.Add(order);
            await _context.SaveChangesAsync();

            // Добавляем товары в заказ и обновляем количество на складе
            foreach (var item in basketItems)
            {
                var purchase = new Purchase
                {
                    ZakazId = order.ZakazId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    //Price = item.Product.Price
                };
                _context.Purchases.Add(purchase);

                // Уменьшаем количество товара на складе
                item.Product.Quantity -= item.Quantity;
                _context.Products.Update(item.Product);
            }

            // Очищаем корзину
            _context.Baskets.RemoveRange(basketItems);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ваш заказ оформлен!";
            return RedirectToAction(nameof(Index));
        }

    }
}
