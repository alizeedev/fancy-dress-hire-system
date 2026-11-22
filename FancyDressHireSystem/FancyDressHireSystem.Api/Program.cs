using Microsoft.AspNetCore.Authentication.Cookies;
using FancyDressHireSystem.Core.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//database:connection string comes from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("GrandAtelier")
    ?? throw new InvalidOperationException("Connection string 'GrandAtelier' not found.");
builder.Services.AddSingleton(new DatabaseConnection(connectionString));
builder.Services.AddSingleton<UserRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
// Default page = login.html (ref: Microsoft Learn, "Static files in ASP.NET Core")
var defaultFiles = new DefaultFilesOptions();
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add("login.html");
app.UseDefaultFiles(defaultFiles);
app.UseStaticFiles();
// Login with cookies (ref: Microsoft Learn, "Cookie authentication")
// .NET 10 returns 401 for API calls when not logged in, so no extra code needed
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
