using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Zakaz
    {
        [Key]
        public int ZakazId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Дата заказа")]
        public DateTime Date { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
