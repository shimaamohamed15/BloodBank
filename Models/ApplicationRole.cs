using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Models
{
    public class ApplicationRole:IdentityRole<int>
    {
        public string Description { get; set; }
       
    }
}
