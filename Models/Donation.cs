using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBank.Models
{
    public enum BloodGroup
    {
        [Display(Name = "O+")]
        OPositive,
        [Display(Name = "A+")]
        APositive,
        [Display(Name = "B+")]
        BPositive,
        [Display(Name = "AB+")]
        ABPositive,
        [Display(Name = "AB-")]
        ABNegative,
        [Display(Name = "A-")]
        ANegative,
        [Display(Name = "B-")]
        BNegative,
        [Display(Name = "O-")]
        ONegative

    }
    public class Donation
    {
        public int Id { get; set; }


        [DataType(DataType.MultilineText)]
        public string Disease { get; set; }

        [Required]
        [Range(20,50)]
        public int Age { get; set; }

        [Required]
        
        public BloodGroup BloodGroup { get; set; }

        [Required]
        [Range(5,15 )]
        public int Quantity { get; set; }

        public DateTime DateOfDonation { get; set; }
        public Status Status { get; set; }

        [ForeignKey("UserId")]
        public int AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }

    }
}
