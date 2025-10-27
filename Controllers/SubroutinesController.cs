using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthMvcSample.Models;
using System.Security.Cryptography;
using System.Text;

namespace OAuthMvcSample.Controllers;

[Authorize]
public class SubroutinesController : Controller
{
    [HttpGet]
    public IActionResult Bmi() => View(new BmiInput());

    [HttpPost]
    public IActionResult Bmi(BmiInput vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var h = vm.HeightCm / 100.0;
        var bmi = vm.WeightKg / (h * h);
        vm.ResultBmi = Math.Round(bmi, 2);
        vm.Category = bmi switch
        {
            < 18.5 => "Недостатня маса",
            >= 18.5 and < 25 => "Норма",
            >= 25 and < 30 => "Надмірна маса",
            _ => "Ожиріння"
        };
        return View(vm);
    }

    [HttpGet]
    public IActionResult Temp() => View(new TempConvertInput());

    [HttpPost]
    public IActionResult Temp(TempConvertInput vm)
    {
        if (!ModelState.IsValid) return View(vm);
        double res = vm.From.ToUpper() switch
        {
            "C" when vm.To.ToUpper() == "F" => vm.Value * 9/5 + 32,
            "F" when vm.To.ToUpper() == "C" => (vm.Value - 32) * 5/9,
            _ => vm.Value
        };
        vm.Result = Math.Round(res, 2);
        return View(vm);
    }

    [HttpGet]
    public IActionResult Password() => View(new PasswordGenInput());

    [HttpPost]
    public IActionResult Password(PasswordGenInput vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var chars = new StringBuilder();
        if (vm.UseLower) chars.Append("abcdefghijklmnopqrstuvwxyz");
        if (vm.UseUpper) chars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (vm.UseDigits) chars.Append("0123456789");
        if (vm.UseSymbols) chars.Append("!@#$%^&*()-_=+[]{};:,.<>/?");

        if (chars.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Оберіть хоча б один тип символів.");
            return View(vm);
        }

        var res = new char[vm.Length];
        using var rng = RandomNumberGenerator.Create();
        var buf = new byte[4];

        for (int i = 0; i < vm.Length; i++)
        {
            rng.GetBytes(buf);
            var idx = BitConverter.ToUInt32(buf, 0) % (uint)chars.Length;
            res[i] = chars[(int)idx];
        }

        vm.Result = new string(res);
        return View(vm);
    }
}
