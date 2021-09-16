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
    [Authorize(Roles ="Admin")]
    public class RequestHistoryModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public RequestHistoryModel(BloodBankDbContext context)
        {
            _context = context;
        }

        public IList<BloodRequest> BloodRequest { get;set; }

        public async Task OnGetAsync()
        {
            BloodRequest = await _context.BloodRequests.Where(r=>r.Status==Status.Approved || r.Status==Status.Rejected)
                .Include(b => b.AppUser).ToListAsync();
        }
    }
}
