using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class User
    {
        [Key]
        public int UsersId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public int RoleId { get;set;}
      
        public Role Role { get; set; }

        public ICollection<Basket> Baskets { get; set; }

        public ICollection<Zakaz> Zakazy { get; set; }
    }
}
