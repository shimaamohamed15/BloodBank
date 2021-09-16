using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BloodBank.Data;
using BloodBank.Models;

namespace BloodBank.Areas.Admin.Pages
{
    public class CreateStockModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public CreateStockModel(BloodBankDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BloodStock BloodStock { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                
                return Page();
            }
            else
            {
                var stock = _context.BloodStocks.Where(s => s.BloodGroup == BloodStock.BloodGroup).FirstOrDefault();
                if (stock == null)
                {
                    _context.BloodStocks.Add(BloodStock);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/BloodStock");
                }
                else
                {
                    ViewData["Error"] = "This blood Type existed in stock";
                    return Page();
                }
            }
        }
    }
}
