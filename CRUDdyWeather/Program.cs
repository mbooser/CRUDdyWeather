using Microsoft.AspNetCore.Authentication.Cookies;
using CRUDdyWeather.Services;
using CRUDdyWeather.Models; // Import the namespace for LdapAuthService

var builder = WebApplication.CreateBuilder(args);

//Enable WatchDogAPI as a Singleton/Sentinal
builder.Services.AddSingleton<UrlCaller>();


// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure Authentication & Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Ensure authentication & authorization are applied
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

