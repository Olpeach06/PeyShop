using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        public int ZakazId { get; set; }
        public Zakaz Zakaz { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Количество товаров")]
        public int Quantity { get; set; }
    }
}
