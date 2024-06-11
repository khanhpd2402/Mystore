using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Pages
{
    public class ListStaffModel : PageModel
    {
        private readonly MyStoreContext _context;

        public ListStaffModel(MyStoreContext context)
        {
            _context = context;
        }
        public IList<Staff> StaffList { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var staffs = from s in _context.Staffs
                         select s;

            if (!string.IsNullOrEmpty(SearchString))
            {
                staffs = staffs.Where(s => s.Name.Contains(SearchString));
            }

            StaffList = await staffs.ToListAsync();
            //StaffList = await _context.Staffs.AsNoTracking().ToListAsync();
        }
    }
}
