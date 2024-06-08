using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TNS_Satellite_WebApp.Pages.Account
{
    public class SignedOutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
            }

            return Page();
        }
    }
}
