using Microsoft.EntityFrameworkCore;
using VHZ_App.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// Add services to the container.
builder.Services.AddRazorPages();

// Добавляем кэш для сессии — обязательно!
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  // Чтобы куки работали без согласия
});

builder.Services.AddDbContext<VhzContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();  // Используем сессию перед авторизацией и endpoint-ами
app.UseAuthorization();

app.MapRazorPages();

app.Run();
