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
        public  IActionResult OnPost()
        {
          
            var staff = _context.Staffs.FirstOrDefault(x => x.Name == Username && x.Password == Password);
            if (staff != null)
            {
                // Convert object to JSON
                string jsonStaff = JsonConvert.SerializeObject(staff);
                // Tạo session cho người dùng
                HttpContext.Session.SetString("Staff", jsonStaff);
                return RedirectToPage("/Home");
            }

            // Trả về lỗi nếu thông tin đăng nhập không đúng
            ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            return Page();
        }

        public IActionResult OnGetLogout()
        {
            // Xóa session khi người dùng đăng xuất
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
