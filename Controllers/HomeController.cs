using Microsoft.AspNetCore.Mvc;

namespace OAuthMvcSample.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
