using BloodBank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        

        [DataType(DataType.MultilineText)]
        public string Disease { get; set; }

        [Required]
        public BloodGroup BloodGroup { get; set; }

        public string Picture { get; set; }



        public List<Donation> Donations { get; set; }
        public List<BloodRequest> BloodRequests { get; set; }

    }
}
