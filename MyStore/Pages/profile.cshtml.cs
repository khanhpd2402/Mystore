﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Pages
{
    public class profileModel : PageModel
    {
        private readonly MyStoreContext _context;

        public profileModel(MyStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Staff Staff { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }
        [BindProperty]
        public string ErrorMessage2 { get; set; }

        public IActionResult OnGet()
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }

            Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);
            if (staff != null)
            {
                Staff = _context.Staffs.FirstOrDefault(x => x.StaffId == staff.StaffId);
            }

            return Page();
        }

        // Phương thức xử lý POST cho form Edit Username
        public async Task<IActionResult> OnPostAsync(string usernameNew)
        {
            //get session
            string jsonStaff = HttpContext.Session.GetString("Staff");
            //check xem session co null khong
            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);
            var staffToUpdate = await _context.Staffs.FindAsync(staff.StaffId);
            
            if (staffToUpdate == null)
            {
                return NotFound();
            }
            Staff = staffToUpdate;
           
            // Validate that the Name property is not null or empty
            if (string.IsNullOrEmpty(usernameNew))
            {
                ErrorMessage = "Name is required.";
                return Page();
            }
            // Kiểm tra xem username mới có trùng với username của bất kỳ người dùng nào khác không
            var existingStaff = await _context.Staffs
                .FirstOrDefaultAsync(x => x.Name == usernameNew.Trim() && x.StaffId != staff.StaffId);

            if (existingStaff != null)
            {
                ErrorMessage = "Username is already taken. Please choose a different username.";
                return Page();
            }

            staffToUpdate.Name = usernameNew;
            staffToUpdate.Password = staff.Password;
            staffToUpdate.Role = staff.Role;
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

            return RedirectToPage("/Index");
        }

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.StaffId == id);
        }

        // Phương thức xử lý POST cho form Change Password
        public async Task<IActionResult> OnPostChangePasswordAsync(string oldPassword, string newPassword1, string newPassword2)
        {

            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }

            var currentStaff = JsonConvert.DeserializeObject<Staff>(jsonStaff);
            if (currentStaff != null)
            {
                Staff = await _context.Staffs.FindAsync(currentStaff.StaffId);
            }

            if (newPassword1 != newPassword2)
            {
                ErrorMessage2 = "The new passwords do not match.";
                return Page();
            } 
            if (string.IsNullOrEmpty(newPassword1) || string.IsNullOrEmpty(newPassword2))
            {
                ErrorMessage2 = "The new passwords is required.";
                return Page();
            }

            if (Staff.Password != oldPassword)
            {
                ErrorMessage2 = "The old password is incorrect.";
                return Page();
            }

            Staff.Password = newPassword1;
            _context.Staffs.Update(Staff);
            await _context.SaveChangesAsync();

            // Xóa session khi người dùng thành công
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
