using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OAuthMvcSample.Models;

namespace OAuthMvcSample.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(u => u.FullName).HasMaxLength(500).IsRequired();
            b.Property(u => u.UserName).HasMaxLength(50).IsRequired();
            b.Property(u => u.PhoneUa).IsRequired();
            b.Property(u => u.Rfc822Email).IsRequired();
        });
    }
}
