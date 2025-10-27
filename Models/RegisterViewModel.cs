using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OAuthMvcSample.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    [Display(Name = "Ім'я користувача")]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    [Display(Name = "Повне ім'я")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Телефон (UA)")]
    [RegularExpression(@"^\+?380\d{9}$", ErrorMessage = "Телефон має бути у форматі +380XXXXXXXXX.")]
    public string PhoneUa { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Email (RFC 822)")]
    [Rfc822Email]
    public string Rfc822Email { get; set; } = string.Empty;

    [Required]
    [StringLength(16, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Пароль і підтвердження мають співпадати.")]
    [Display(Name = "Підтвердження пароля")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public static bool IsPasswordStrong(string password)
    {
        var hasDigit = Regex.IsMatch(password, @"\d");
        var hasUpper = Regex.IsMatch(password, @"[A-ZА-ЯЇІЄҐ]");
        var hasSign  = Regex.IsMatch(password, @"[^\w\s]");
        return hasDigit && hasUpper && hasSign;
    }
}
