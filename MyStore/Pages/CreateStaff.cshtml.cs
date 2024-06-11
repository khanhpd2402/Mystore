using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;

namespace MyStore.Pages
{
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

            _context.Staffs.Add(Staff);
            await _context.SaveChangesAsync();

            return RedirectToPage("ListStaff");
        }
    }
}
