using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Assignment2.Pages.Order
{
    public class DeleteModel : PageModel
    {
        private readonly MyStoreContext _context;

        public DeleteModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }
        [BindProperty]
        public MyStore.Models.Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");

            if (string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);

                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }
                else
                {
                    Order = order; // Gán giá trị của order cho Orders
                }
                return Page();
            }
        }

            public async Task<IActionResult> OnPostAsync(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    Order = order;
                    _context.Orders.Remove(Order);
                    await _context.SaveChangesAsync();
                }

                return RedirectToPage("./Index");
            }
        }
    }
