using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Configuration;
using FashionForward.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// MySQL Connection
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("MySqlConnection");
builder.Services.AddScoped<MySqlConnection>(_ =>
{
    return new MySqlConnection(connectionString);
});
// ======

// Add Email Sender Service
builder.Services.AddScoped<EmailHelper>(_ =>
{
    return new EmailHelper(configuration);
});
// ======

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "verify",
    pattern: "Verify/{token:int}",
    defaults: new { controller = "Verify", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

