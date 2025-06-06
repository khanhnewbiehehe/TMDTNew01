using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using ThuongMaiDienTu.Areas.Customer.Services;
using ThuongMaiDienTu.Data;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICategoryService, ItemCategoryService>();
builder.Services.AddScoped<IProductService, ItemProductService>();
builder.Services.AddScoped<IFavouriteCustomerService, ItemFavouriteCustomerService>();
builder.Services.AddScoped<IInvoiceCustomerService, ItemInvoiceCustomerService>();
builder.Services.AddScoped<IProductTypeCustomerService, ItemProductTypeCustomerService>();
builder.Services.AddScoped<IVNPayService, VNPayService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    })
   .AddEntityFrameworkStores<AppDbContext>()
   .AddDefaultTokenProviders()
   .AddDefaultUI();

// Cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services);
}

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
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
