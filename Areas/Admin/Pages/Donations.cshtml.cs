using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BloodBank.Data;
using BloodBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BloodBank.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class DonationsModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly IEmailSender _emailSender;

        public DonationsModel(BloodBankDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public IList<Donation> Donation { get;set; }
        public Donation donation { get; set; }
        public BloodStock Stock { get; set; }
        public async Task OnGetAsync()
        {
            Donation = await _context.Donations.Where(d=>d.Status==Status.Pending)
                .Include(d => d.AppUser).ToListAsync();
        }
        public IActionResult OnPostApprove(int? id)
        {
            donation = _context.Donations.Where(d => d.Id == id).FirstOrDefault();

            if (donation != null)
            {
                donation.Status = Status.Approved;
                Stock = _context.BloodStocks.Where(s => s.BloodGroup == donation.BloodGroup).FirstOrDefault();
                Stock.Quantity += donation.Quantity;
                _context.Donations.Update(donation);
                _context.BloodStocks.Update(Stock);
                _context.SaveChanges();
                var userId = _context.Donations.Where(d => d.Id == id).FirstOrDefault().AppUserId;
                var userEmail = _context.Users.Where(u => u.Id == userId).FirstOrDefault().Email;
                _emailSender.SendEmailAsync(userEmail, "Approved Donation",
                     " your Donation is Approved .");
                return LocalRedirect("/Admin/DonationHistory");
            }

            return Page();
            }

        public IActionResult OnPostReject(int? id)
        {
            donation = _context.Donations.Where(d => d.Id == id).FirstOrDefault();

            if (donation != null)
            {
                donation.Status = Status.Rejected;
                _context.Attach(donation).State = EntityState.Modified;
                _context.Donations.Update(donation);
                _context.SaveChanges();
                var userId = _context.Donations.Where(d => d.Id == id).FirstOrDefault().AppUserId;
                var userEmail = _context.Users.Where(u => u.Id == userId).FirstOrDefault().Email;

                _emailSender.SendEmailAsync(userEmail, "Rejected Donation",
                    " your Donation is Rejected .");
                return LocalRedirect("/Admin/DonationHistory");
            }

            return Page();
        }

    }
}
