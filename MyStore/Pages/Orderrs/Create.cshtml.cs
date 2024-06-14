using MyStore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SE1726_Razor.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly MyStoreContext _context;

        public CreateModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);


                try
                {
                    Order od = new Order();
                    od.OrderDate = DateTime.Now;
                    od.StaffId = staff.StaffId;
                    _context.Orders.Add(od);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu có
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm đơn hàng. Vui lòng thử lại sau.");
                    return Page();
                }
             
            }
           

        }

    }
}