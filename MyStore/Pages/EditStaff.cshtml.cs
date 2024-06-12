using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Pages
{
    public class EditStaffModel : PageModel
    {
        private readonly MyStoreContext _context;

        public EditStaffModel(MyStoreContext context)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (await NameExists(Staff.Name, Staff.StaffId))
            {
                ModelState.AddModelError("Staff.Name", "The name already exists. Please choose a different name.");
                return Page();
            }

            var staffToUpdate = await _context.Staffs.FindAsync(Staff.StaffId);

            if (staffToUpdate == null)
            {
                return NotFound();
            }

            staffToUpdate.Name = Staff.Name;
            staffToUpdate.Password = Staff.Password;
            staffToUpdate.Role = Staff.Role;

            _context.Attach(staffToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(Staff.StaffId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("ListStaff");
        }

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.StaffId == id);
        }
        private async Task<bool> NameExists(string name, int id)
        {
            return await _context.Staffs.AnyAsync(e => e.Name == name && e.StaffId != id);
        }
    }
}
