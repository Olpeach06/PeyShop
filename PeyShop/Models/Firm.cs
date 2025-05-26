using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Firm
    {
        [Key]
        public int FirmId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Производитель")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
