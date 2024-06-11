using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Pages.Report
{
    public class OrderDetailsModel : PageModel
    {
        private readonly MyStore.Models.MyStoreContext _context;

        public OrderDetailsModel(MyStore.Models.MyStoreContext context)
        {
            _context = context;
        }

        public IList<OrderDetail> OrderDetail { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderdetail = await _context.OrderDetails
                               .Include(od => od.Product)
                               .Where(m => m.OrderId == id)
                               .ToListAsync();
            if (orderdetail == null)
            {
                return NotFound();
            }
            else
            {
                OrderDetail = orderdetail;
            }
            return Page();
        }
    }
}
