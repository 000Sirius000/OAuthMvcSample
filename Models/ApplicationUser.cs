using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OAuthMvcSample.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50, ErrorMessage = "Ім'я користувача має бути до 50 символів.")]
    public override string UserName { get; set; } = default!;

    [Required]
    [StringLength(500, ErrorMessage = "Повне ім'я має бути до 500 символів.")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Телефон (UA)")]
    [RegularExpression(@"^\+?380\d{9}$", ErrorMessage = "Телефон має бути у форматі +380XXXXXXXXX.")]
    public string PhoneUa { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Email (RFC 822)")]
    [Rfc822Email]
    public string Rfc822Email { get; set; } = string.Empty;
}
