
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeyShop.Models;

namespace PeyShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Таблицы пользователей и ролей
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;

        // Корзины покупателей
        public DbSet<Basket> Baskets { get; set; } = default!;

        // Категории продуктов
        public DbSet<Category> Categories { get; set; } = default!;

        // Продукты
        public DbSet<Product> Products { get; set; } = default!;

        // Производители товаров
        public DbSet<Firm> Firms { get; set; } = default!;

        // Статусы заказов
        public DbSet<Status> Statuses { get; set; } = default!;

        // Заказы
        public DbSet<Zakaz> Zakazy { get; set; } = default!;

        // Покупки
        public DbSet<Purchase> Purchases { get; set; } = default!;

        // Типы товаров
        public DbSet<TypeOfPr> TypeOfPr { get; set; } = default!;
    }
}
