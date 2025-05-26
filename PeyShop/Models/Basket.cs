using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Basket
    {
        [Key]
        public int BasketId { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Количество товаров")]
        public int Quantity { get; set; }
    }
}
