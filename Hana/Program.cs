using Hana.Controllers;
using Hana.DB;
using Hana.hubs;
using Microsoft.OpenApi.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hana.Models;
using Hana.Hana.Database.Data;


var builder = WebApplication.CreateBuilder(args);


// Retrieve the connection string from app settings
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'HanaContextConnection' not found.");

builder.Services.AddDbContext<HanaContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<UserProfile>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<HanaContext>();

//// Add HanaContext to the dependency injection container
//builder.Services.AddDbContext<HanaContext>(options => options.UseSqlServer(connectionString));


//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<HanaContext>();

//// remove RequireConfirmedAccount so the user doesnt need to confirm account. 
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<HanaContext>();



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddScoped<UserDb>();
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

app.UseAuthorization();
//signall chatHub
app.MapHub<ChatHub>("/chatHub");
// use this to map with Identiy framework Razor pages
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=index}");

app.Run();
