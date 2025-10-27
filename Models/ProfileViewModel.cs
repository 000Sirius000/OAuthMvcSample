using System.ComponentModel.DataAnnotations;

namespace OAuthMvcSample.Models;

public class ProfileViewModel
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

    [DataType(DataType.Password)]
    [Display(Name = "Поточний пароль")]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Новий пароль")]
    public string? NewPassword { get; set; }
}