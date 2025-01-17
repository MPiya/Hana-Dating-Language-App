using Hana.Controllers;
using Hana.hubs;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hana.Models;
using Hana.Hana.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string from app settings
var connectionString = builder.Configuration.GetConnectionString("HanaContextConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Add HanaContext to the dependency injection container
builder.Services.AddDbContext<HanaContext>(options => options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddDefaultIdentity<UserIdentity>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<HanaContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//signalR chatHub
app.MapHub<ChatHub>("/chatHub");

// use this to map with Identity framework Razor pages
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
