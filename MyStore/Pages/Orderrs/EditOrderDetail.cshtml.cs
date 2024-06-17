using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
namespace Assignment2.Pages.Order
{
    [Authorize(Policy = "StaffOnly")]
    public class EditOrderDetail : PageModel
    {
        private readonly MyStoreContext _context;

        public EditOrderDetail(MyStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Staff Staff { get; set; }



        [BindProperty]
        public OrderDetail OrderDetail { get; set; } = default!;

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

                var orderdetail = await _context.OrderDetails.FirstOrDefaultAsync(m => m.OrderDetailId == id);
                if (orderdetail == null)
                {
                    return NotFound();
                }
                OrderDetail = orderdetail;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
                      _context.Attach(OrderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(OrderDetail.OrderDetailId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Get the OrderId for redirection
            int orderId = OrderDetail.OrderId;

            // Redirect to the Details page of the order with the specified id
            return RedirectToPage("./Details", new { id = orderId });
        }

        // Helper method to check if the OrderDetail exists
        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }

    }
}
