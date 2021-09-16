using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BloodBank.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace BloodBank.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

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

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

          
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

           
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.Where(u => u.Email == Input.Email).FirstOrDefault();
                if (user == null)
                {

                    ModelState.AddModelError(string.Empty, "Invalid Email Or Password.");
                    return Page();
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");
                        var roles = await _userManager.GetRolesAsync(user);
                        _logger.LogInformation("User logged in.");
                        if (user.EmailConfirmed)
                        {
                            if (roles.Contains("Admin"))
                            {

                                return LocalRedirect("/Admin/AdminHome");
                            }
                            else
                            {

                                _logger.LogInformation("User logged in.");
                                return RedirectToPage("/UserHome");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Please confirm your email.");
                            ModelState.AddModelError(string.Empty, "check your inbox");
                            return Page();
                        }
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt. ");
                        ModelState.AddModelError(string.Empty, "Please confirm your email.");
                        ModelState.AddModelError(string.Empty, "check your inbox");

                        return Page();
                    }

                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true

            }
            // I f we got this far, something failed, redisplay form
            return Page();
        }
    }
}
