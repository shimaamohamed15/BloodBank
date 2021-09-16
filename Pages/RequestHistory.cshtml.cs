using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BloodBank.Data;
using BloodBank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BloodBank.Pages
{
    [Authorize(Roles = "Admin,User")]
    public class RequestHistoryModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public RequestHistoryModel(BloodBankDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }
        public ApplicationUser user { get;set; }
        

        public async Task OnGetAsync()
        {
            var userName =  _usermanager.GetUserName(User);
            user = await _context.Users.Where(u=>u.UserName==userName).Include(b => b.BloodRequests).FirstOrDefaultAsync();
           
        }
    }
}
