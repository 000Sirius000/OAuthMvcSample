using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OAuthMvcSample.Models;

namespace OAuthMvcSample.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;

    public AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn)
    {
        _users = users;
        _signIn = signIn;
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        if (!RegisterViewModel.IsPasswordStrong(vm.Password))
        {
            ModelState.AddModelError(nameof(vm.Password),
                "Пароль має містити щонайменше 1 цифру, 1 спецзнак та 1 велику літеру.");
            return View(vm);
        }

        var user = new ApplicationUser
        {
            UserName = vm.UserName,
            FullName = vm.FullName,
            PhoneUa = vm.PhoneUa,
            Rfc822Email = vm.Rfc822Email,
            Email = vm.Rfc822Email,
            PhoneNumber = vm.PhoneUa
        };

        var result = await _users.CreateAsync(user, vm.Password);
        if (result.Succeeded)
        {
            await _signIn.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var e in result.Errors)
            ModelState.AddModelError(string.Empty, e.Description);

        return View(vm);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _signIn.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
            return Redirect(returnUrl ?? Url.Action("Index", "Home")!);

        ModelState.AddModelError(string.Empty, "Невірні дані входу.");
        return View(vm);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _users.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");
        return View(new ProfileViewModel
        {
            FullName = user.FullName,
            PhoneUa = user.PhoneUa,
            Rfc822Email = user.Rfc822Email
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Profile(ProfileViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _users.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        // --- Оновлення даних користувача ---
        user.UserName = vm.UserName;
        user.FullName = vm.FullName;
        user.PhoneUa = vm.PhoneUa;
        user.Rfc822Email = vm.Rfc822Email;
        user.Email = vm.Rfc822Email;
        user.PhoneNumber = vm.PhoneUa;

        var updateResult = await _users.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var e in updateResult.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(vm);
        }

        // --- Зміна пароля, якщо заповнено поля ---
        if (!string.IsNullOrWhiteSpace(vm.CurrentPassword) && !string.IsNullOrWhiteSpace(vm.NewPassword))
        {
            var passResult = await _users.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
            if (!passResult.Succeeded)
            {
                foreach (var e in passResult.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(vm);
            }
        }

        ViewBag.Message = "Профіль успішно оновлено!";
        return View(vm);
    }
}
