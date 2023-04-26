using Artisan.Data;
using Artisan.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Server;

public class Startup
{
    private readonly IConfiguration _cfg;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration cfg, IWebHostEnvironment env)
    {
        _cfg = cfg;
        _env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        var connectionString = _cfg.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddCustomOAuth(_cfg)
            .AddIdentityServerJwt();

        services.AddDataProtection()
            .PersistKeysToDbContext<ApplicationDbContext>();

        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddSwaggerGen();
    }
    
    public void Configure(IApplicationBuilder app)
    {
        MigrateDatabase(app.ApplicationServices);
        
        if (_env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Artisan API V1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseAuthentication();

        app.UseEndpoints(e =>
        {
            e.MapRazorPages();
            e.MapControllers();
            e.MapFallbackToFile("index.html");
        });
    }

    public void MigrateDatabase(IServiceProvider services)
    {
        using var ctx = services.GetRequiredService<ApplicationDbContext>();
        ctx.Database.Migrate();
    }
}