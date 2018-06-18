using System.ComponentModel.DataAnnotations;

namespace ProfileSelect.Models
{
    public class ResetStudentPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} должен содержать как минимум {2} символов", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
