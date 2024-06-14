using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using Newtonsoft.Json;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        
        // Kiểm tra xem người dùng đã đăng nhập chưa
        if (!context.Session.Keys.Contains("Staff"))
        {
            // Kiểm tra xem đường dẫn hiện tại có phải là đường dẫn đăng nhập không
            if (!context.Request.Path.StartsWithSegments("/"))
            {
                // Nếu không, chuyển hướng đến trang đăng nhập
                context.Response.Redirect("/Index");
                return;
            }
        }

        // Nếu đã đăng nhập hoặc đang ở trang đăng nhập, tiếp tục xử lý yêu cầu
        await _next(context);
    }
}
