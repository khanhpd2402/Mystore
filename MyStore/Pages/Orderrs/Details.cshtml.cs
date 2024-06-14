using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Assignment2.Pages.Order
{
    public class DetailsModel : PageModel
    {
        private readonly MyStoreContext _context;

        public DetailsModel(MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Staff Staff { get; set; }
        public IList<OrderDetail> orderdetails { get; set; } = default!;

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
                OrderDetail = new OrderDetail
                {
                    OrderId = id.Value // Gán giá trị OrderId từ tham số truyền vào từ URL
                };
               
                {
                    orderdetails = await _context.OrderDetails
                        .Include(x => x.Product)
                        .Where(o => o.OrderId == id)
                        .ToListAsync();

                

                }
            }
            return Page();
        }
    }
}