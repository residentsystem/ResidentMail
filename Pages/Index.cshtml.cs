using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResidentMail.Models;

namespace ResidentMail.Pages
{
    public class IndexModel : PageModel
    {
        public string PageTitle = "Resident Mail";

        // Bind variable with properties of the Contact model class.
        [BindProperty]
        public Contact Contact { get; set; }

        public void OnGet()
        {
            
        } 

        public void OnPost()
        {
            
        }
    }
}