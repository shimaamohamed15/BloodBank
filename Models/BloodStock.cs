using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBank.Models
{
    public class BloodStock
    {
        public int Id { get; set; }

        public BloodGroup BloodGroup { get; set; }


        public int Quantity { get; set; }
       
    }
}
