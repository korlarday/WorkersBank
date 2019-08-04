﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkersBank.Models.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}