﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkersBank.Areas.Admin.Models
{
    public class UserNavPartialVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PassportName { get; set; }
        public string Email { get; set; }
    }
}