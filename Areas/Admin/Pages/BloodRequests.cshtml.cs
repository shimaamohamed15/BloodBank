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
    public class BloodRequestsModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly IEmailSender _emailSender;

        public BloodRequestsModel(BloodBankDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public IList<BloodRequest> BloodRequest { get;set; }
        public BloodRequest bloodRequest { get; set; }
        public BloodStock stock { get; set; }

        public async Task OnGetAsync()
        {
            BloodRequest = await _context.BloodRequests.Where(r => r.Status == Status.Pending)
                .Include(b => b.AppUser).ToListAsync();
        }
        public IActionResult OnPostApprove(int? id)
        {
            bloodRequest = _context.BloodRequests.Where(r => r.Id == id).FirstOrDefault();

            if (bloodRequest != null)
            {
                stock = _context.BloodStocks.Where(s => s.BloodGroup == bloodRequest.BloodGroup).FirstOrDefault();
                if (stock.Quantity > bloodRequest.Quantity)
                {
                    bloodRequest.Status = Status.Approved;
                    stock.Quantity -= bloodRequest.Quantity;
                    _context.BloodRequests.Update(bloodRequest);
                    _context.BloodStocks.Update(stock);
                    _context.SaveChanges();
                    var userId = _context.BloodRequests.Where(b => b.Id == id).FirstOrDefault().AppUserId;
                    var userEmail = _context.Users.Where(u => u.Id == userId).FirstOrDefault().Email;

                    _emailSender.SendEmailAsync(userEmail, "Approved Request",
                      " your request is Approved .");
                    return LocalRedirect("/Admin/RequestHistory");
                }
                else
                {
                    return NotFound("The Quantity is less than requested quantity ");
                }

            }
            
            return NotFound();
        }

        public IActionResult OnPostReject(int? id)
        {
            bloodRequest = _context.BloodRequests.Where(r => r.Id == id).FirstOrDefault();

            if (bloodRequest != null)
            {
                bloodRequest.Status = Status.Rejected;
                _context.BloodRequests.Update(bloodRequest);
                _context.SaveChanges();
                var userId = _context.BloodRequests.Where(b => b.Id == id).FirstOrDefault().AppUserId;
                var userEmail = _context.Users.Where(u => u.Id == userId).FirstOrDefault().Email;

                _emailSender.SendEmailAsync(userEmail, "Rejected Request",
                      " your request is Rejected .");
                return LocalRedirect("/Admin/RequestHistory");
            }

            return Page();
        }

    }
}
