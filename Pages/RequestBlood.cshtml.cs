using BloodBank.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BloodBank.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BloodBank.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class RequestBloodModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RequestBloodModel(BloodBankDbContext context , UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public string userName { get; set; }
        public ApplicationUser user { get; set; }



        public IActionResult OnGet()
        {
            


            return Page();
        }


        [BindProperty]
        public BloodRequest bloodRequest { get; set; }
       

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                userName = _userManager.GetUserName(User);
                user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    bloodRequest.DateOfRequest = DateTime.Now;
                    bloodRequest.Status = Status.Pending;
                    bloodRequest.AppUserId = user.Id;

                    _context.BloodRequests.Add(bloodRequest);
                    await _context.SaveChangesAsync();
                    await _emailSender.SendEmailAsync("sh.fcih15@gmail.com", "Blood request sent",
                      " some one request blood .");
                    return RedirectToPage("/RequestHistory");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
