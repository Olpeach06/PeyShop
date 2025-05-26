using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Статус заказа")]
        public string Name { get; set; }

        public ICollection<Zakaz> Zakazy { get; set; }
    }
}
