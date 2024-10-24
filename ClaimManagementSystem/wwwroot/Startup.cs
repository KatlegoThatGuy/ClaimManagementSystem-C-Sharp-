using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure Entity Framework with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            .AddEntityFrameworkStores<ClaimsApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddControllersWithViews();
            services.AddRazorPages();
            Password settings
             options.Password.RequireDigit = true;
             options.Password.RequireLowercase = true;
             options.Password.RequireUppercase = true;
             options.Password.RequireNonAlphanumeric = true;
             options.Password.RequiredLength = 6;
             options.Password.RequiredUniqueChars = 1;
            
            .AddEntityFrameworkStores<ClaimsApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Configure application cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Set your custom login path
                options.AccessDeniedPath = "/Account/AccessDenied"; // Optional: set access denied path
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
   

            // Lockout settings
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            // User settings
            options.User.RequireUniqueEmail = true; // Ensure emails are unique

            // Sign-in Settings
            options.SignIn.RequireConfirmedAccount = true; // Optional: Require email confirmation for sign-in
        })
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); // Enable authentication middleware
        app.UseAuthorization();  // Enable authorization middleware

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapAreaControllerRoute(
                name: "identity",
                areaName: "Identity",
                pattern: "Identity/{controller=Account}/{action=LoginNew}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            endpoints.MapRazorPages(); // Enable Razor Pages for Identity and other Razor pages
        });
    }
}