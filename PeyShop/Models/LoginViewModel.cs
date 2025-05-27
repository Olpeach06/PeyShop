using System.ComponentModel.DataAnnotations;

namespace PeyShop.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите ваш e-mail.")]
        [EmailAddress(ErrorMessage = "Неправильный формат e-mail.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}