using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class RegisterSeeker
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string StateOfOrigin { get; set; }

        [Required]
        [Display(Name = "Residential Address")]
        public string HomeAddress { get; set; }
    }
}