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
    public class BloodStockModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public BloodStockModel(BloodBankDbContext context)
        {
            _context = context;
        }

        public IList<BloodStock> BloodStock { get;set; }
        public BloodStock stock { get; set; }
        public async Task OnGetAsync()
        {
            BloodStock = await _context.BloodStocks.ToListAsync();
           
        }
    }
}
