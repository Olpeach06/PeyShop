using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Роль")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
