using System.ComponentModel.DataAnnotations;

namespace OAuthMvcSample.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Ім'я користувача")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Запам'ятати мене")]
    public bool RememberMe { get; set; }
}
