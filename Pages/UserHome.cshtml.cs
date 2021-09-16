using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BloodBank.Data;

namespace BloodBank.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class UserHomeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BloodBankDbContext _context;

        public int NumOfPendingReq { get; set; }
        public int NumOfRejectedReq { get; set; }
        public int NumOfApprovedReq { get; set; }
        public int NumOfPendingDon { get; set; }
        public int NumOfRejectedDon { get; set; }
        public int NumOfApprovedDon { get; set; }
        public ApplicationUser user { get; set; }
        public UserHomeModel(UserManager<ApplicationUser> userManager,BloodBankDbContext context)
        {
            _userManager = userManager;
            _context = context;
            NumOfPendingReq = 0;
            NumOfRejectedReq = 0;
            NumOfApprovedReq = 0;
            NumOfPendingDon = 0;
            NumOfRejectedDon = 0;
            NumOfApprovedDon = 0;
        }
       
        public async Task<IActionResult> OnGetAsync()
        {
            user =await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                NumOfApprovedReq = _context.BloodRequests.Where(u => u.Status == Status.Approved && u.AppUserId==user.Id).Count();
                    NumOfPendingReq = _context.BloodRequests.Where(u => u.Status == Status.Pending && u.AppUserId == user.Id).Count();
                    NumOfRejectedReq = _context.BloodRequests.Where(u => u.Status == Status.Rejected && u.AppUserId == user.Id).Count();
                
                
                    NumOfApprovedDon = _context.Donations.Where(u => u.Status == Status.Approved && u.AppUserId == user.Id).Count();
                    NumOfPendingDon = _context.Donations.Where(u => u.Status == Status.Pending && u.AppUserId == user.Id).Count();
                    NumOfRejectedDon = _context.Donations.Where(u => u.Status == Status.Rejected && u.AppUserId == user.Id).Count();
               
                return Page();
            }
            
        }

    }
}
