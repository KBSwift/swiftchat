using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SwiftChat.Data;
using SwiftChat.Hubs; // Access ChatHub from SwiftChat.Hubs namespace
using SwiftChat.Interfaces;
using SwiftChat.Models.Configurations;
using SwiftChat.Models.Entities;
using SwiftChat.Services; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Adds MVC controllers with views
builder.Services.AddControllersWithViews();

// Gmail Configurations
var gmailSettings = builder.Configuration.GetSection("GmailEmailSettings").Get<GmailEmailSettings>();
if (gmailSettings == null)
{
	throw new InvalidOperationException("Gmail settings could not be loaded.");
}

builder.Services.AddSingleton(gmailSettings);

builder.Services.AddScoped<IEmailService, GmailEmailService>();

var gmailService = await GmailServiceHelper.GetGmailServiceAsync(gmailSettings);
builder.Services.AddSingleton(gmailService);

// Configure Entity Framework Core to use MySQL database
// Original implementation before using ENV VARIABLES for safety to upload publicly
/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 31))));*/

// Latest Implementation using ENV VARIABLES. Now updated for MS SQL Server instead of MySQL
var connectionString = Environment.GetEnvironmentVariable("SWIFTCHAT_CONNECTION_STRING") ?? // will attempt using ENV VAR first
    builder.Configuration.GetConnectionString("DefaultConnection"); // will break unless proper MS SQL Server connection string is added to appsettings.json

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add ASP.NET Core Identity using the default IdentityUser
// Original Core Application implementation
/*builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();*/

// Modifying to relax password requirements for ease
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Password settings
    options.Password.RequireDigit = false; // No digit required
    options.Password.RequireLowercase = false; // No lowercase letter required
    options.Password.RequireNonAlphanumeric = false; // No non-alphanumeric character required
    options.Password.RequireUppercase = false; // No uppercase letter required
    options.Password.RequiredLength = 3; // Minimum length of 3 characters
    options.Password.RequiredUniqueChars = 0; // Unique character check
    options.SignIn.RequireConfirmedAccount = true; // Adding email confirmation since these settings may have ovewritten this feature
})
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Adding SignalR services
builder.Services.AddSignalR();

builder.Logging.AddConsole(); // Log to console
var app = builder.Build();

// Configuring HTTP request pipeline

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Adding authentication and authorization middleware to the pipeline
app.UseAuthentication();
app.UseAuthorization();

// Configuring the routes for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Maps the SignalR ChatHub to the specified endpoint ("/chatHub")
app.MapHub<ChatHub>("/chatHub");

// Adding new Razor Pages, which is required for Identity pages (login, logout, register, forgotpassword)
app.MapRazorPages(); // This enables routing for Razor Pages, including those used by Identity

app.Run();
