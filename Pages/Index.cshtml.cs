using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BloodBank.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly BloodBankDbContext _context;

        public IndexModel(ILogger<IndexModel> logger,BloodBankDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
           
        }
    }
}
