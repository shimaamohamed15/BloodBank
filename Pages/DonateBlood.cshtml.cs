using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BloodBank.Data;
using BloodBank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BloodBank.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class DonateBloodModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public DonateBloodModel(BloodBankDbContext context,UserManager<ApplicationUser> userManager, IEmailSender emailSender)
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
        public Donation Donation { get; set; }

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
                    Donation.DateOfDonation = DateTime.Now;
                    Donation.Status = Status.Pending;
                    Donation.AppUserId = user.Id;

                    _context.Donations.Add(Donation);
                    await _context.SaveChangesAsync();
                    await _emailSender.SendEmailAsync("sh.fcih15@gmail.com", "donation sent",
                       " some one donate blood .");

                    return RedirectToPage("/DonationHistory");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
