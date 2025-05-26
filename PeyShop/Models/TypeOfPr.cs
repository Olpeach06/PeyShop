using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class TypeOfPr
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Тип товара")]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
