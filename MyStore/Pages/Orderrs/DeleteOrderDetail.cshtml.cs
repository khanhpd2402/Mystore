using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Assignment2.Pages.Order
{
    [Authorize(Policy = "StaffOnly")]
    public class DeleteOrderDetailModel : PageModel
    {
        private readonly MyStoreContext _context;

        public DeleteOrderDetailModel(MyStoreContext context)
        {
            _context = context;
        }
        public OrderDetail OrderDetail { get; set; } = default!;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.OrderDetails.FindAsync(id);
            if (orderdetail != null)
            {
                OrderDetail = orderdetail;
                _context.OrderDetails.Remove(OrderDetail);
                await _context.SaveChangesAsync();
            }
            int orderId = OrderDetail.OrderId;

            return RedirectToPage("./Details", new { id = orderId });
        }
    }
}
