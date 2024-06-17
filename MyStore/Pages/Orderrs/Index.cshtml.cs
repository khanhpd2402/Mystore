using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
namespace Assignment2.Pages.Orders
{
    [Authorize(Policy = "StaffOnly")]
    public class IndexModel : PageModel
    {
        private readonly MyStoreContext _context;

        public IndexModel(MyStoreContext context)
        {
            _context = context;
        }

        public IList<MyStore.Models.Order> orders { get; set; } = default!;

        [BindProperty]
        public DateTime Date { get; set; }
        [BindProperty]
        public Staff Staff { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);


                Date = DateTime.Now.Date; // Thiết lập giá trị mặc định là ngày hôm nay
                orders = await _context.Orders
                    .Where(o => o.StaffId == staff.StaffId && o.OrderDate.Date == Date)
                                     .OrderByDescending(Date => Date)
                    .ToListAsync();

            }
            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);


                if (Date != null)
                {
                    orders = await _context.Orders
                        .Where(o => o.StaffId == staff.StaffId && o.OrderDate.Date == Date.Date)
                        .OrderByDescending(Date => Date)
                        .ToListAsync();
                }
                else
                {
                    DateTime today = DateTime.Today;
                    orders = await _context.Orders
                        .Where(o => o.StaffId == staff.StaffId && o.OrderDate.Date == today)
                        .ToListAsync();

                }
            }

            return Page();
        }
    }
}
