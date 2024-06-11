using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyStore.Models;
using Newtonsoft.Json;
namespace MyStore.Pages
{
    public class HomeModel : PageModel
    {
       

        public IActionResult OnGet()
        {
            string jsonStaff = HttpContext.Session.GetString("Staff");
           
            if(string.IsNullOrEmpty(jsonStaff))
            {
                return RedirectToPage("/Index");
            }
            else
            {
                Staff staff = JsonConvert.DeserializeObject<Staff>(jsonStaff);
                return Page();
            }
        }
    }
}