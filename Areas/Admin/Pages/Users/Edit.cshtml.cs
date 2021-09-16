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
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace BloodBank.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly BloodBankDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EditModel(BloodBankDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public class InputModel
        {
            [Required]
            public string FristName { get; set; }

            [Required]
            public string LastName { get; set; }
            [Required]
            public string UserName { get; set; }

            

            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [DataType(DataType.MultilineText)]
            public string Disease { get; set; }

            

            public IFormFile Picture { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (ApplicationUser == null)
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
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null) {
                string uniqueFileName = UploadedFile(Input);
                user.FirstName = Input.FristName;
            user.LastName = Input.LastName;
                user.Disease = Input.Disease;
                user.Picture = uniqueFileName;
                user.UserName = Input.UserName;
            user.PhoneNumber = Input.PhoneNumber;
            
            
            _context.Users.Update(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
            return NotFound();
        }

        private bool ApplicationUserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        private string UploadedFile(InputModel model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "assets\\img");
                uniqueFileName = Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
