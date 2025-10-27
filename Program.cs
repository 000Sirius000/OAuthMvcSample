using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Data;
using OAuthMvcSample.Models;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore; // <-- ВАЖЛИВО!
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// EF + SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db");
    options.UseOpenIddict();
});

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.Password.RequireDigit = true;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireLowercase = false;
        opt.Lockout.AllowedForNewUsers = false;
        opt.User.RequireUniqueEmail = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/Account/Login";
    opt.AccessDeniedPath = "/Account/Login";
});

// OpenIddict (мінімальна конфігурація, яка точно є в 5.x)
builder.Services.AddOpenIddict()
    .AddCore(opt =>
    {
        opt.UseEntityFrameworkCore()
           .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(opt =>
    {
        opt.AllowAuthorizationCodeFlow()
           .RequireProofKeyForCodeExchange();

        opt.SetAuthorizationEndpointUris("/connect/authorize")
           .SetTokenEndpointUris("/connect/token");
        // (Userinfo та Introspection — опустимо для простоти/сумісності)

        // Реєструємо скопи (включно з openid):
        opt.RegisterScopes(Scopes.OpenId, Scopes.Email, Scopes.Profile);

        opt.UseAspNetCore()
           .EnableAuthorizationEndpointPassthrough()
           .EnableTokenEndpointPassthrough()
           .EnableStatusCodePagesIntegration();

        opt.AddEphemeralEncryptionKey()
           .AddEphemeralSigningKey();

        opt.DisableAccessTokenEncryption();
    })
    .AddValidation(opt =>
    {
        opt.UseLocalServer();
        opt.UseAspNetCore();
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Авто-міграції + сидінг клієнта
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    const string clientId = "mvc-client";
    if (await manager.FindByClientIdAsync(clientId) is null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = "Demo MVC Client",
            RedirectUris = { new Uri("https://localhost:5001/signin-oidc") },
            PostLogoutRedirectUris = { new Uri("https://localhost:5001/signout-callback-oidc") },
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.ResponseTypes.Code,
                // Права на скопи: додаємо email/profile (для openid окремої константи в Permissions немає — це ОК)
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();