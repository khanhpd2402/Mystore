using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;
using MyStore.Pages;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MyStoreContext>();  

// Thêm dịch vụ cần thiết cho session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true; // Chỉ truy cập session thông qua HTTP
    options.Cookie.IsEssential = true; // Bắt buộc session cookie
});

//thieu o day
builder.Services.AddDbContext<MyStoreContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
//wtf
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "1"));
    options.AddPolicy("StaffOnly", policy =>
        policy.RequireClaim("Role", "0"));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MyCookieAuthenticationScheme";
    options.DefaultChallengeScheme = "MyCookieAuthenticationScheme";
})
.AddCookie("MyCookieAuthenticationScheme", options =>
{
    options.LoginPath = "/Index"; // Đường dẫn đến trang đăng nhập
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie (tuỳ chọn)
    options.SlidingExpiration = true; // Cho phép gia hạn thời gian tồn tại khi người dùng tương tác (tuỳ chọn)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); // Sử dụng session
//app.UseMiddleware<AuthenticationMiddleware>(); // Sử dụng Middleware kiểm tra đăng nhập ở đây
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
