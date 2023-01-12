using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebUI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

// For Entity Framework
builder.Services.AddDbContext<CustomIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnStr")));

builder.Services.AddIdentityCore<WebUI.Entities.CustomIdentityUser>(options =>
{
    options.User.RequireUniqueEmail = false;
}).AddRoles<IdentityRole>().AddSignInManager().AddEntityFrameworkStores<CustomIdentityDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();
builder.Services.AddSession();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
           .AddCookie(IdentityConstants.ApplicationScheme, o =>
           {
               o.LoginPath = new PathString("/Home/Login");
               o.Events = new CookieAuthenticationEvents
               {
                   OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
               };
           })
           .AddCookie(IdentityConstants.ExternalScheme, o =>
           {
               o.Cookie.Name = IdentityConstants.ExternalScheme;
               o.ExpireTimeSpan = TimeSpan.FromMinutes(30);
           });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();



app.UseHttpsRedirection();
//app.UseMvc();
app.UseStaticFiles();
app.UseFileServer();
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Register}/{id?}");

//app.MapControllers();

app.Run();