using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using Microsoft.AspNetCore.Authorization;
namespace MyStore.Pages
{
    [Authorize(Policy = "AdminOnly")]
    public class CreateStaffModel : PageModel
    {
        private readonly MyStoreContext _context;

        public CreateStaffModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Check for duplicate staff name
            var existingStaff = _context.Staffs.FirstOrDefault(s => s.Name == Staff.Name);
            if (existingStaff != null)
            {
                ModelState.AddModelError("Staff.Name", "A staff member with this name already exists.");
                return Page();
            }

            _context.Staffs.Add(Staff);
            await _context.SaveChangesAsync();

            return RedirectToPage("ListStaff");
        }
    }
}
