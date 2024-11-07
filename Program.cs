using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ToDoListContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;               
            options.Password.RequireLowercase = false;           
            options.Password.RequireNonAlphanumeric = false;     
            options.Password.RequireUppercase = false;           
            options.Password.RequiredLength = 4;                  
            options.Password.RequiredUniqueChars = 1;            
        })
     .AddEntityFrameworkStores<ToDoListContext>()
     .AddDefaultTokenProviders();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

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
            pattern: "{controller=Tasks}/{action=Index}/{id?}");

        app.Run();
    }
}
