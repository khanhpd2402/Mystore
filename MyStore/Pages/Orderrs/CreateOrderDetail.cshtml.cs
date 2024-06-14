using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Assignment2.Pages.Order
{
    public class CreateOrderDetailModel : PageModel
    {
        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;

        private readonly MyStoreContext _context;

        public CreateOrderDetailModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }
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


                _context.OrderDetails.Add(OrderDetail);
                await _context.SaveChangesAsync();

                // Lấy OrderId của chi tiết đơn hàng vừa được thêm vào
                int orderId = OrderDetail.OrderId;

                // Chuyển hướng đến trang chi tiết đơn hàng với id của chi tiết đơn hàng vừa được thêm vào
                return RedirectToPage("./Details", new { id = orderId });
            }

        }
    }
}