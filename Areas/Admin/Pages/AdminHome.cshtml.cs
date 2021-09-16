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

namespace BloodBank.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminHomeModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public AdminHomeModel(BloodBankDbContext context)
        {
            _context = context;
        }

        public IList<BloodStock> BloodStock { get;set; }

        public async Task<ActionResult> OnGetAsync()
        {
            BloodStock = await _context.BloodStocks.ToListAsync();
            if (BloodStock == null)
            {
                ModelState.AddModelError(string.Empty , " Stock is empty");
                return Page();
            }
            else
            {  
            ViewData["TotalBloodReq"] = _context.BloodRequests.Count();
            ViewData["NumOfPendingReq"] = _context.BloodRequests.Where(R => R.Status == Status.Pending).Count();
            ViewData["NumOfApprovedReq"] = _context.BloodRequests.Where(R => R.Status == Status.Approved).Count();
            ViewData["NumOfRejectedReq"] = _context.BloodRequests.Where(R => R.Status == Status.Rejected).Count();
            ViewData["TotalBloodDon"] = _context.Donations.Count();
            ViewData["NumOfPendingDon"] = _context.Donations.Where(R => R.Status == Status.Pending).Count();
            ViewData["NumOfApprovedDon"] = _context.Donations.Where(R => R.Status == Status.Approved).Count();
            ViewData["NumOfRejectedDon"] = _context.Donations.Where(R => R.Status == Status.Rejected).Count();
            return Page();
        }
    }
}}
