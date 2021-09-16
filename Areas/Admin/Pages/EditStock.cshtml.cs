using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BloodBank.Data;
using BloodBank.Models;

namespace BloodBank.Areas.Admin.Pages
{
    public class EditStockModel : PageModel
    {
        private readonly BloodBankDbContext _context;

        public EditStockModel(BloodBankDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int Quantity { get; set; }
        public BloodStock BloodStock { get; set; }
       
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BloodStock = await _context.BloodStocks.FirstOrDefaultAsync(m => m.Id == id);
            
        

            if (BloodStock == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            BloodStock = await _context.BloodStocks.FirstOrDefaultAsync(s => s.Id == id);
            if (BloodStock != null)
            {
                BloodStock.Quantity = Quantity;
                _context.Attach(BloodStock).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodStockExists(BloodStock.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("/BloodStock");
            }
            return NotFound();
        }

        private bool BloodStockExists(int id)
        {
            return _context.BloodStocks.Any(e => e.Id == id);
        }
    }
}
