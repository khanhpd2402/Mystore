using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Pages.Report
{
    public class OrdersModel : PageModel
    {
        private readonly MyStore.Models.MyStoreContext _context;

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }

        public OrdersModel(MyStore.Models.MyStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (StartDate == DateTime.MinValue && EndDate == DateTime.MinValue)
            {
                // Nếu không có ngày bắt đầu và kết thúc được cung cấp, lấy tất cả đơn hàng
                StartDate = DateTime.Today.AddMonths(-1);
                EndDate = DateTime.Today;
                if (_context.Orders != null)
                {
                    Order = await _context.Orders
                    .Include(o => o.Staff)
                    .ToListAsync();
                }
            }
            else
            {
                // Nếu có ngày bắt đầu và kết thúc, lọc theo khoảng ngày
                if (_context.Orders != null)
                {
                    Order = await _context.Orders
                    .Include(o => o.Staff)
                    .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate)
                    .ToListAsync();
                }
            }
        }
    }
}