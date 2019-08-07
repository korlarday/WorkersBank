using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WorkersBank.Models.ViewModel;

namespace WorkersBank.Areas.Admin.Models
{
    public class AuthorizeVM
    {
        public UsersVM Customer { get; set; }
        [Display(Name = "Administrator")]
        public bool Admin { get; set; }
    }
}