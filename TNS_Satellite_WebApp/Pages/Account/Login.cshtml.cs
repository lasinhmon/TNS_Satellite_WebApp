using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.WebRequestMethods;

namespace TNS_Satellite_WebApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        public string ReturnUrl { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ReturnUrl = returnUrl;

        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var zUser = await AuthenticateUser(Input.UserName, Input.Password);

                if (!zUser.Successed)
                {
                    ModelState.AddModelError(string.Empty, zUser.Message);
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, zUser.UserKey),
                    new Claim("UserName", zUser.UserName),
                    new Claim("MemberKey", zUser.MemberKey)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }

            // Something failed. Redisplay the form.
            return Page();
        }

        private async Task<TNS.Auth.CheckUserLogin> AuthenticateUser(string userName, string password)
        {
            // For demonstration purposes, authenticate a user
            // with a static email address. Ignore the password.
            // Assume that checking the database takes 500ms

            await Task.Delay(500);

            return new TNS.Auth.CheckUserLogin(userName, password);

        }
    }
}
