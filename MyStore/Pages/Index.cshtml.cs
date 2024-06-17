using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyStore.Pages
{
    public class loginModel : PageModel
    {
        private MyStoreContext _context;
        public loginModel(MyStoreContext context)
        {
            _context = context;

        }
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(string ReturnUrl = null)
        {
            var staff = _context.Staffs.FirstOrDefault(x => x.Name == Username && x.Password == Password);
            if (staff != null)
            {
                // Convert object to JSON
                string jsonStaff = JsonConvert.SerializeObject(staff);
                // Tạo session cho người dùng
                HttpContext.Session.SetString("Staff", jsonStaff);

                var claims = new[]
                {
            new Claim(ClaimTypes.Name, staff.Name),
            new Claim("Role", staff.Role.ToString()) // Thêm claim cho role
        };

                var identity = new ClaimsIdentity(claims, "MyCookieAuthenticationScheme");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuthenticationScheme", principal);

                // Chuyển hướng người dùng đến trang mà họ đã yêu cầu ban đầu (nếu có)

                return RedirectToPage("/Home"); // Chuyển hướng về trang chủ nếu không có returnUrl
            }

            // Trả về lỗi nếu thông tin đăng nhập không đúng
            ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            return Page();
        }


        public async Task <IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync("MyCookieAuthenticationScheme");
            // Xóa session khi người dùng đăng xuất
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
