using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;
using Newtonsoft.Json;

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

        public async Task<IActionResult> OnGetAsync(DateTime? startDate, DateTime? endDate, string searchString)
        {

            var sessionData = HttpContext.Session.GetString("Staff");
            if (sessionData == null)
            {
                return RedirectToPage("/Index");
            }

            var currentUser = JsonConvert.DeserializeObject<Staff>(sessionData);
            int role = currentUser.Role;
            int staffId = currentUser.StaffId;

            CurrentFilter = searchString;

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
                             .Where(o => o.Staff.StaffId == staffId)
                             .Where(o => o.OrderDate >= StartDate && o.OrderDate <= EndDate);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchStaffId))
                {
                    query = query.Where(s => s.Staff.StaffId == searchStaffId || s.Staff.Name.Contains(searchString));
                }
                else
                {
                    query = query.Where(s => s.Staff.Name.Contains(searchString));
                }
            }

            Order = await query.ToListAsync();

            return Page();
        }
    }
}
