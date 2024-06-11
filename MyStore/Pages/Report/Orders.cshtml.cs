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

        public string CurrentFilter { get; set; }
        public OrdersModel(MyStore.Models.MyStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync(DateTime? startDate, DateTime? endDate, string searchString)
        {
            int role = 1;
            int staff = 2;
            CurrentFilter = searchString;

            // Use provided date range or set default values
            StartDate = startDate ?? DateTime.Today.AddMonths(-1);
            EndDate = endDate ?? DateTime.Today;

            IQueryable<Order> query = _context.Orders;

            if (role == 1)
            {
                query = query.Include(o => o.Staff)
                             .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate);
            }
            else
            {
                query = query.Include(o => o.Staff)
                             .Where(o => o.Staff.StaffId == staff)
                             .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int staffId))
                {
                    query = query.Where(s => s.Staff.StaffId == staffId || s.Staff.Name.Contains(searchString));
                }
                else
                {
                    query = query.Where(s => s.Staff.Name.Contains(searchString));
                }
            }

            Order = await query.ToListAsync();
        }
    }
}