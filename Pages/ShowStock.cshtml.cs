using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BloodBank.Data;
using BloodBank.Models;

namespace BloodBank.Pages
{
    public class ShowStockModel : PageModel
    {
        private readonly BloodBank.Data.BloodBankDbContext _context;

        public ShowStockModel(BloodBank.Data.BloodBankDbContext context)
        {
            _context = context;
        }

        public IList<BloodStock> BloodStock { get;set; }

        public async Task OnGetAsync()
        {
            BloodStock = await _context.BloodStocks.ToListAsync();
        }
    }
}
