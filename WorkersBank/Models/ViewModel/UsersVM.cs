using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WorkersBank.Models.Data;

namespace WorkersBank.Models.ViewModel
{
    public class UsersVM
    {
        public UsersVM()
        {

        }
        public UsersVM(UsersDTO row)
        {
            Id = row.Id;
            Username = row.Username;
            Password = row.Password;
            UserType = row.UserType;
            Firstname = row.Firstname;
            Middlename = row.Middlename;
            Surname = row.Surname;
            Gender = row.Gender;
            DOB = row.DOB;
            Email = row.Email;
            PhoneNumber = row.PhoneNumber;
            PassportName = row.PassportName;
            StateOfOrigin = row.StateOfOrigin;
            CV = row.CV;
            Address = row.Address;
        }

        public int Id { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string Middlename { get; set; }
        public string PassportName { get; set; }

        [Required]
        public string Username { get; set; }


        [Required]
        public string Firstname { get; set; }
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
        public string CV { get; set; }
        public string Address { get; set; }
    }
}