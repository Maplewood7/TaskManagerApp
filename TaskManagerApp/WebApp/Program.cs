using WebApp.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Antiforgery;


namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddSignInManager().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<ITaskRepo, TaskRepo>();
            builder.Services.AddScoped<TaskService>();
            builder.Services.AddScoped<ITaskTagRepo, TaskTagRepo>();
            builder.Services.AddScoped<TaskTagService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<UserStateService>();

            builder.Services.AddCascadingAuthenticationState();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDBContext>();
                context.Database.Migrate();
            }

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
            app.UseAntiforgery();

            app.MapPost("/account/login", async (
                HttpContext context,
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

                try
                {
                    await antiforgery.ValidateRequestAsync(context);
                }
                catch (AntiforgeryValidationException)
                {
                    return Results.BadRequest("Invalid antiforgery token.");
                }

                var form = await context.Request.ReadFormAsync();
                var email = form["Email"].ToString();
                var password = form["Password"].ToString();
                var user = await userManager.FindByEmailAsync(email);

                if (user is null)
                {
                    return Results.Redirect("/login?error=1");
                }

                var result = await signInManager.PasswordSignInAsync(
                    user,
                    password,
                    isPersistent: true,
                    lockoutOnFailure: false);

                return result.Succeeded
                    ? Results.Redirect("/")
                    : Results.Redirect("/login?error=1");
            });

            app.MapPost("/account/logout", async (
                HttpContext context,
                SignInManager<IdentityUser> signInManager) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

                try
                {
                    await antiforgery.ValidateRequestAsync(context);
                }
                catch (AntiforgeryValidationException)
                {
                    return Results.BadRequest("Invalid antiforgery token.");
                }

                await signInManager.SignOutAsync();
                return Results.Redirect("/");
            });

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
