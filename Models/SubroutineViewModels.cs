using System.ComponentModel.DataAnnotations;

namespace OAuthMvcSample.Models;

public class BmiInput
{
    [Required, Range(20, 300)]
    [Display(Name = "Вага (кг)")]
    public double WeightKg { get; set; }

    [Required, Range(100, 250)]
    [Display(Name = "Зріст (см)")]
    public double HeightCm { get; set; }

    public double? ResultBmi { get; set; }
    public string? Category { get; set; }
}

public class TempConvertInput
{
    [Required]
    [Display(Name = "Значення")]
    public double Value { get; set; }

    [Required]
    [Display(Name = "Звідки")]
    public string From { get; set; } = "C";

    [Required]
    [Display(Name = "Куди")]
    public string To { get; set; } = "F";

    public double? Result { get; set; }
}

public class PasswordGenInput
{
    [Required, Range(8, 32)]
    [Display(Name = "Довжина")]
    public int Length { get; set; } = 12;

    [Display(Name = "Великі літери")]
    public bool UseUpper { get; set; } = true;

    [Display(Name = "Малі літери")]
    public bool UseLower { get; set; } = true;

    [Display(Name = "Цифри")]
    public bool UseDigits { get; set; } = true;

    [Display(Name = "Спецсимволи")]
    public bool UseSymbols { get; set; } = true;

    public string? Result { get; set; }
}
