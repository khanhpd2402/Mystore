using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;
using Microsoft.AspNetCore.Authorization;
namespace MyStore.Pages
{
    [Authorize(Policy = "AdminOnly")]
    public class DeleteStaffModel : PageModel
    {
        private readonly MyStoreContext _context;

        public DeleteStaffModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Staff = await _context.Staffs.FindAsync(id);

            if (Staff == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Staff == null)
            {
                return NotFound();
            }

            _context.Staffs.Remove(Staff);
            await _context.SaveChangesAsync();

            return RedirectToPage("ListStaff");
        }
    }
}
