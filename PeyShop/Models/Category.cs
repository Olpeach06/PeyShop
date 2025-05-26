using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Категория")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
