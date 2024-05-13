using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis_Attempt_1.Data;
namespace Snackis_Attempt_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("SnackisContextConnection") ?? throw new InvalidOperationException("Connection string 'SnackisContextConnection' not found.");

            builder.Services.AddDbContext<SnackisContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<Areas.Identity.Data.SnackisUser>(options => 
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<SnackisContext>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
