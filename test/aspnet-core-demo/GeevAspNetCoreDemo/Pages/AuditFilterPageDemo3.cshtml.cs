using Geev.Auditing;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeevAspNetCoreDemo.Pages
{
    public class AuditFilterPageDemo3Model : PageModel
    {
        [DisableAuditing]
        public void OnGet()
        {

        }

        [DisableAuditing]
        public void OnPost()
        {

        }
    }
}