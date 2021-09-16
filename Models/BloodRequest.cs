using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBank.Models
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
    public class BloodRequest
    {
        public int Id { get; set; }

        [Required]
        public BloodGroup BloodGroup { get; set; }

        [Required]
        [Range(5, 50)]
        public int Quantity { get; set; }

        [Required]
        [Range(1,100)]
        public int Age { get; set; }

        [Required]
        public string Reason { get; set; }
        public DateTime DateOfRequest { get; set; }
        public Status Status { get; set; }

        [ForeignKey("UserId")]
        public int AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
    }
}
