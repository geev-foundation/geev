using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeevAspNetCoreDemo.Pages
{
    public class AuditFilterPageDemoModel : PageModel
    {
        [BindProperty]
        public string Message { get; set; }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {

        }
    }
}