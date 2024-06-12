using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly MyStore.Models.MyStoreContext _context;

        public IndexModel(MyStore.Models.MyStoreContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public int? UnitPrice { get; set; }
        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                Product = await _context.Products
                .Include(p => p.Category).ToListAsync();
            }
        }

        public async Task OnPostAsync()
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Kiểm tra xem Name và UnitPrice có giá trị không
            if (!string.IsNullOrEmpty(Name) && UnitPrice >= 0)
            {
                query = query.Where(p => p.ProductName.Contains(Name) && p.UnitPrice == UnitPrice);
            }
            else
            {
                // Kiểm tra xem Name có giá trị không
                if (!string.IsNullOrEmpty(Name))
                {
                    query = query.Where(p => p.ProductName.Contains(Name));
                }

                // Kiểm tra xem UnitPrice có giá trị không
                if (UnitPrice >= 0)
                {
                    query = query.Where(p => p.UnitPrice == UnitPrice);
                }
            }

            // Thực hiện tìm kiếm và gán kết quả cho Product
            Product = await query.ToListAsync();
        }

        
    }
}
