using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snackis_Attempt_1.Areas.Identity.Data;

namespace Snackis_Attempt_1.Data;

public class SnackisContext : IdentityDbContext<SnackisUser>
{
    public SnackisContext(DbContextOptions<SnackisContext> options)
        : base(options)
    {
    }

    public DbSet<Snackis_Attempt_1.Models.Category> Categories { get; set; } = default;
    public DbSet<Snackis_Attempt_1.Models.Post> Posts { get; set; } = default; 
    public DbSet<Snackis_Attempt_1.Models.Comment> Comments { get; set; } = default;
    public DbSet<Snackis_Attempt_1.Models.PrivateMessage> PrivateMessages { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

	
}
