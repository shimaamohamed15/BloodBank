using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BloodBank.Data;
using BloodBank.Models;

namespace BloodBank.Areas.Admin.Pages
{
    public class DonationHistoryModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public DonationHistoryModel(BloodBankDbContext context)
        {
            _context = context;
        }

        public IList<Donation> Donation { get;set; }

        public async Task OnGetAsync()
        {
            Donation = await _context.Donations.Where(d=>d.Status==Status.Approved || d.Status==Status.Rejected)
                .Include(d => d.AppUser).ToListAsync();
        }
    }
}
