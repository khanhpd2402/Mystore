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

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var roleClaim = context.User.FindFirst("Role");
            if (roleClaim == null || roleClaim.Value != "1")
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Page not found");
                return;
            }
        }

        await _next(context);
    }
}
